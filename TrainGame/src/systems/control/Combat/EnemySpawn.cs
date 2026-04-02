namespace TrainGame.Systems; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public static class LadderInteractSystem {
    public static void Register(World w) {
        InteractSystem.Register<Ladder>(w, (w, _) => {
            FloorSystem.GoToFloor(w, 1); 
        });
    }
}

public static class ReturnToSurfaceSystem {
    public static void Register(World w) {
        w.AddSystem((w) => {
            if (VirtualKeyboard.IsClicked(KeyBinds.ReturnToSurface)) {
                (Floor f, bool exists) = w.GetFirst<Floor>();
                if (exists && f > 0) {
                    FloorSystem.GoToFloor(w, 0); 
                }
            }
        });
    }
}

public class Floor {
    public int Value; 
    public Floor(int Value = 0) {
        this.Value = Value; 
    }

    public static implicit operator int(Floor f) {
        return f.Value; 
    }
}

public static class FloorSystem {
    public static void GoToFloor(World w, int floor) {
        if (floor == 0) {
            MakeMessage.Add<DrawCityMessage>(w, new DrawCityMessage(CityWrap.GetCityWithPlayer(w)));
        } else {
            Layout.DrawCombat(w);
        }

        PlayerWrap.SetFloor(w, floor); 
        int spawnEnt = EntityFactory.Add(w); 
        w.SetComponent<EnemySpawner>(spawnEnt, new EnemySpawner(floor));
    }
}

public static class EnemySpawnSystem {
    private static void spawn(World w, EnemySpawner spawn) {
        int sumDifficulties = spawn.Difficulty; 
        int maxDifficulty = (int)Math.Cbrt(sumDifficulties); 

        double difficultyD = 1d + (maxDifficulty - 1d)*Math.Pow(Util.NextDoublePositive(), 1d/maxDifficulty);
        int difficulty = Math.Min(12, (int)difficultyD); 
        spawn.TargetNumActive = Math.Max(spawn.TargetNumActive, (int)(difficultyD * 1.75d)); 
        int numToSpawn = spawn.NumActive < spawn.TargetNumActive - 1 ? 2 : 1;

        for (int i = 0; i < numToSpawn; i++) {
            EnemyType enemyType = EnemyID.GetRandomWithDifficulty(difficulty); 
            Vector2 topleft = w.GetCameraTopLeft(); 
            float cameraWidth = w.ScreenWidth; 
            float cameraHeight = w.ScreenHeight;
            
            float addX = Util.NextInt(2) == 1 ? 0f : cameraWidth; 
            float addY = Util.NextInt(2) == 1 ? 0f : cameraHeight; 
            addX += Util.NextFloat() * Constants.TileWidth * (addX == 0 ? 1 : -1);
            addY += Util.NextFloat() * Constants.TileWidth * (addY == 0 ? 1 : -1); 

            EnemyWrap.Draw(w, topleft + new Vector2(addX, addY), enemyType, LootWrap.GetDestination(w));

            spawn.Difficulty += difficulty; 
            spawn.NumActive++; 
        } 

        Globals.MaxFloor = Math.Max(Globals.MaxFloor, spawn.Difficulty); 
    }

    public static void Register(World w) {
        w.AddSystem([typeof(EnemySpawner), typeof(Active)], (w, e) => {
            EnemySpawner spawner = w.GetComponent<EnemySpawner>(e); 
            if (spawner.NumActive == 0) {
                spawn(w, spawner); 
            }
        });

        w.AddSystem([typeof(Enemy), typeof(Health), typeof(Expired), typeof(Active)], (w, e) => {
            (EnemySpawner spawner, bool spawnExists) = w.GetFirst<EnemySpawner>(); 
            spawner.NumActive--;
            spawn(w, spawner);
        });
    }
}

public static class RewardSpawnSystem {
    
    private static LootDistribution lootDist = new LootDistribution(
        new Dictionary<string, int>(){
            [ItemID.Credit] = 50,
            [ItemID.Cobalt] = 50
        },
        new Dictionary<string, int>(){
            [ItemID.Credit] = 100,
            [ItemID.Cobalt] = 100
        }
    );

    private static Func<World, int, Type> setLoot = (w, e) => {
        (string itemID, int count) = lootDist.GetRandom(); 
        w.SetComponent<TextBox>(e, new TextBox($"+{count} {itemID}"));
        w.SetComponent<Loot>(e, new Loot(itemID, count, LootWrap.GetDestination(w)));
        return typeof(Loot); 
    };

    private static Func<World, int, Type> setMaxAmmo = (w, e) => {
        w.SetComponent<MaxAmmo>(e, new MaxAmmo());
        w.SetComponent<TextBox>(e, new TextBox("+Max Ammo"));
        return typeof(MaxAmmo); 
    };

    private static Distribution<Type, Func<World, int, Type>> rewardDist = new Distribution<Type, Func<World, int, Type>>(
        new Dictionary<Type, int>(){
            [typeof(Loot)] = 50,
            [typeof(MaxAmmo)] = 50
        },
        new Dictionary<Type, Func<World, int, Type>>(){
            [typeof(Loot)] = setLoot,
            [typeof(MaxAmmo)] = setMaxAmmo
        }
    );

    private static Type setReward(World w, int e) {
        (Type t, Func<World, int, Type> setter) = rewardDist.GetRandom(); 
        return setter(w, e); 
    }

    public static void Register(World w) {
        w.AddSystem([typeof(Enemy), typeof(Health), typeof(Expired), typeof(Frame), typeof(Active)], (w, e) => {
            bool rewardOnGround = w.GetMatchingEntities([typeof(CombatReward), typeof(Active)]).Count > 0; 
            if (!rewardOnGround && Util.NextFloat() < Constants.RewardChance) {
                int[] es = {-1, -1};
                List<Type> ts = new();
                Vector2 pos = w.GetComponent<Frame>(e).Position;

                for (int i = 0; i < 2; i++) {
                    Vector2 curPos = pos + new Vector2(i * 2 * Constants.TileWidth, 0f);
                    int rewardEnt = EntityFactory.AddUI(w, curPos, Constants.TileWidth, Constants.TileWidth, 
                    setOutline: true, setInteractable: true);
                    w.SetComponent<CombatReward>(rewardEnt, new CombatReward(w.Time)); 
                    es[i] = rewardEnt;
                    ts.Add(setReward(w, rewardEnt)); 
                }

                if (ts[0] == ts[1]) {
                    if (ts[0] != typeof(Loot)) {
                        setLoot(w, es[0]); 
                    } else {
                        setMaxAmmo(w, es[0]);
                    }
                }
            }
        });
    }
}
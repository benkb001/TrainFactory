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

public enum CombatState {
    Fighting, 
    Reward,
    Cooldown
}

public class CombatReward {
    public bool Active; 
    public CombatReward() {
        Active = true; 
    }
}

public class CombatRewardCollectedMessage {}

public class EnemySpawner {
    private List<Health> hs = new(); 
    private List<CombatReward> rewards = new(); 
    private int round = 0;
    private WorldTime between_rounds = new WorldTime(minutes: 5); 
    private WorldTime next_round = new WorldTime(); 
    private CombatState state = CombatState.Cooldown;
    public int FloorDest = 1; 

    public int Round => round; 

    public bool CanSpawn(WorldTime now) {
        return state == CombatState.Cooldown && now.IsAfterOrAt(next_round); 
    }

    public void Update(WorldTime now) {
        if (state == CombatState.Fighting) {
            if (hs.Where(h => h.HP <= 0).ToList().Count == hs.Count) {
                hs.Clear(); 
                rewards.Clear();
                state = CombatState.Reward;
            }
        } else if (state == CombatState.Reward) {
            if (rewards.Count > 0 && rewards.Where(r => !r.Active).ToList().Count == rewards.Count) {
                next_round = now + between_rounds; 
                state = CombatState.Cooldown; 
            }
        } else if (state == CombatState.Cooldown) {
            if (hs.Count > 0) {
                state = CombatState.Fighting; 
            }
        }
    }

    public bool CanReward() {
        return state == CombatState.Reward && rewards.Count == 0; 
    }

    public void FinishRound() {
        hs.Clear(); 
        round++;
    } 

    public void Spawn(Health h) {
        hs.Add(h); 
    }

    public void Spawn(EnemyWrap ew) {
        Spawn(ew.GetHealth());
    }

    public void AddReward(CombatReward reward) {
        rewards.Add(reward); 
    }
}

public class Ladder {
    public int FloorDest;

    public Ladder(int FloorDest = 1) {
        this.FloorDest = FloorDest;
    }
}

public class LadderWrap {
    public static void AddMessage(World w, Vector2 pos, int FloorDest) {
        MakeMessage.Add<DrawLadderMessage>(w, new DrawLadderMessage(pos, FloorDest));
    }

    public static void Draw(World w, Vector2 pos, int FloorDest) {
        int e = EntityFactory.AddUI(w, pos, Constants.TileWidth, Constants.TileWidth, 
            setInteractable: true, text: FloorDest == 0 ? "Exit" : "Ladder", setOutline: true);
        w.SetComponent<Ladder>(e, new Ladder(FloorDest));
    }
}

//This is necessary because if interacting with a ladder 
//draws another ladder on the same frame, 
//bad things with modifying list during iteration
//on ladderInteractSystem entities
public class DrawLadderMessage {
    public Vector2 Pos; 
    public int FloorDest; 

    public DrawLadderMessage(Vector2 Pos, int FloorDest) {
        this.Pos = Pos; 
        this.FloorDest = FloorDest; 
    }
}

public static class DrawLadderSystem {
    public static void Register(World w) {
        DrawSystem.Register<DrawLadderMessage>(w, (w, e, dm) => {
            LadderWrap.Draw(w, dm.Pos, dm.FloorDest);
        });
    }
}

public static class EnemySpawnSystem {
    private const float armorThresh = 0.01f;
    private const float damageThresh = 0.02f; 
    private const float healthThresh = 0.07f; 
    private const float itemThresh = 1f;
    private const int numRewards = 1;

    public static void Register(World w) {
        Vector2 dx(int i) {
            return new Vector2((i * 110f) + 10f, 10f); 
        }

        w.AddSystem([typeof(EnemySpawner), typeof(Frame), typeof(Active)], (w, e) => {
            EnemySpawner spawner = w.GetComponent<EnemySpawner>(e); 
            Frame f = w.GetComponent<Frame>(e); 
            spawner.Update(w.Time); 
            int round = spawner.Round; 

            if (spawner.CanReward()) {

                for (int i = 0; i < numRewards; i++) {
                    Vector2 pos = f.Position + dx(i);

                    int rewardEnt = EntityFactory.AddUI(w, pos, Constants.TileWidth, 
                        Constants.TileWidth, setOutline: true, setInteractable: true);

                    CombatReward reward = new CombatReward(); 
                    w.SetComponent<CombatReward>(rewardEnt, reward); 
                    spawner.AddReward(reward); 

                    Loot loot = Loot.GetRandom(spawner.FloorDest, CityWrap.GetCityWithPlayer(w).Inv, w, difficulty: 3);
                    w.SetComponent<Loot>(rewardEnt, loot);
                    string rewardStr = $"{loot.GetItemID()}: {loot.Count}";

                    w.SetComponent<TextBox>(rewardEnt, new TextBox(rewardStr)); 
                }

                LadderWrap.Draw(w, f.Position + dx(numRewards), spawner.FloorDest);
            }
        });
    }
}

public static class LadderInteractSystem {
    public static void Register(World w) {
        InteractSystem.Register<Ladder>(w, (w, _, ladder) => {
            FloorSystem.GoToFloor(w, ladder.FloorDest); 
        });
    }
}

public static class FloorSystem {
    public static void GoToFloor(World w, int floor) {
        if (floor == 0) {
            MakeMessage.Add<DrawCityMessage>(w, new DrawCityMessage(CityWrap.GetCityWithPlayer(w)));
        } else {
            Layout.DrawRandom(w, floor);
        }

        Inventory inv = LootWrap.GetDestination(w);
        
        foreach (int e in w.GetMatchingEntities(EnemyWrap.EnemySignature)) {
            int difficulty = EnemyWrap.Enemies[w.GetComponent<Enemy>(e).Type].Difficulty; 
            w.SetComponent<Loot>(e, Loot.GetRandom(floor, inv, w, difficulty: difficulty));
        }

        foreach (int e in w.GetMatchingEntities([typeof(EnemySpawner), typeof(Active)])) {
            w.GetComponent<EnemySpawner>(e).FloorDest = floor + 1; 
        }

        Globals.MaxFloor = Math.Max(Globals.MaxFloor, floor); 
    }
}
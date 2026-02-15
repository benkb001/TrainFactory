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
    public static void Draw(World w, Vector2 pos, int FloorDest) {
        int e = EntityFactory.AddUI(w, pos, Constants.TileWidth, Constants.TileWidth, 
            setInteractable: true, text: FloorDest == 0 ? "Exit" : "Ladder", setOutline: true);
        w.SetComponent<Ladder>(e, new Ladder(FloorDest));
    }
}

public static class EnemySpawnSystem {
    private const float armorThresh = 0.05f; 
    private const float damageThresh = 0.01f; 
    private const float healthThresh = 0.5f; 
    private const float timeCrystalThresh = 1f;
    private const int numRewards = 2;

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

                    string rewardStr = ""; 
                    float rand = w.NextFloat(); 

                    //TODO: maybe some items should be rewards here? 
                    //like machines and such ? 
                    if (rand < armorThresh) {
                        rewardStr = "Armor +1"; 
                        w.SetComponent<TempArmor>(rewardEnt, new TempArmor(1)); 
                    } else if (rand < damageThresh) {
                        rewardStr = "Damage +1"; 
                        w.SetComponent<DamagePotion>(rewardEnt, new DamagePotion(1)); 
                    } else {
                        int hp = round; 
                        hp = 1 + (int)(w.NextFloat() * w.NextFloat() * hp); 
                        rewardStr = $"HP +{hp}";
                        w.SetComponent<HealthPotion>(rewardEnt, new HealthPotion(hp)); 
                    } 

                    w.SetComponent<TextBox>(rewardEnt, new TextBox(rewardStr)); 
                }

                LadderWrap.Draw(w, f.Position + dx(numRewards), spawner.FloorDest);
            }
        });
    }
}

public class Floor {
    private int number;
    
    public Floor() {
        number = 0; 
    }

    public void Reset() {
        number = 0;
    }

    public static implicit operator int(Floor f) {
        return f.number;
    }

    public static Floor operator ++(Floor f) {
        f.number = f.number + 1; 
        return f;
    }
}

public static class LadderInteractSystem {
    public static void Register(World w) {
        InteractSystem.Register<Ladder>(w, (w, _, ladder) => {
            if (ladder.FloorDest == 0) {
                MakeMessage.Add<DrawCityMessage>(w, new DrawCityMessage(CityWrap.GetCityWithPlayer(w)));
            } else {
                Layout.DrawRandom(w);
            }

            Inventory inv = LootWrap.GetDestination(w);
            
            foreach (int e in w.GetMatchingEntities(EnemyWrap.EnemySignature)) {
                w.SetComponent<Loot>(e, Loot.GetRandom(ladder.FloorDest, inv, w));
            }

            foreach (int e in w.GetMatchingEntities([typeof(EnemySpawner), typeof(Active)])) {
                w.GetComponent<EnemySpawner>(e).FloorDest = ladder.FloorDest + 1; 
            }
        });
    }
}
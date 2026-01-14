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

public class EnemySpawner {
    private List<Health> hs = new(); 
    private int round = 0; 
    public int Round => round; 

    public bool CanSpawn() {
        return (hs.Where(h => h.HP <= 0).ToList().Count == hs.Count); 
    }

    public void FinishRound() {
        hs.Clear(); 
        round++;
    } 

    public void Spawn(Health h) {
        hs.Add(h); 
    }
}

public static class EnemySpawnSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(EnemySpawner), typeof(Frame), typeof(Active)], (w, e) => {
            EnemySpawner spawner = w.GetComponent<EnemySpawner>(e); 
            if (spawner.CanSpawn()) {
                spawner.FinishRound(); 
                Frame f = w.GetComponent<Frame>(e); 
                for (int i = 0; i < Math.Min(5, spawner.Round); i++) {
                    float xRand = w.NextFloat(); 
                    float yRand = w.NextFloat(); 
                    float x = f.GetWidth() * xRand;
                    float y = f.GetHeight() * yRand; 
                    Vector2 pos = f.Position + new Vector2(x, y); 

                    int enemyEnt = EntityFactory.AddUI(w, pos, Constants.EnemySize, 
                        Constants.EnemySize, setOutline: true);
                    Health h = new Health(10);
                    w.SetComponent<Health>(enemyEnt, h); 
                    spawner.Spawn(h); 
                    w.SetComponent<Shooter>(enemyEnt, new Shooter(bulletDamage: spawner.Round)); 
                    w.SetComponent<Enemy>(enemyEnt, new Enemy()); 
                    w.SetComponent<Movement>(enemyEnt, new Movement()); 
                    w.SetComponent<Collidable>(enemyEnt, new Collidable()); 
                    w.SetComponent<Loot>(enemyEnt, new Loot(ItemID.TimeCrystal, spawner.Round, InventoryWrap.GetPlayerInv(w)));
                }
            }
        });
    }
}
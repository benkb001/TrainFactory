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
            int difficulty = EnemyID.Enemies[w.GetComponent<Enemy>(e).Type].Difficulty; 
            w.SetComponent<Loot>(e, Loot.GetRandom(floor, inv, w, difficulty: difficulty));
        }

        foreach (int e in w.GetMatchingEntities([typeof(EnemySpawner), typeof(Active)])) {
            w.GetComponent<EnemySpawner>(e).FloorDest = floor + 1; 
        }

        Globals.MaxFloor = Math.Max(Globals.MaxFloor, floor); 
    }
}
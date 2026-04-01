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
    public static void Register(World w) {
        w.AddSystem([typeof(EnemySpawner), typeof(Active)], (w, e) => {
            EnemySpawner spawn = w.GetComponent<EnemySpawner>(e); 

            if (w.Time.IsAfterOrAt(spawn.NextSpawn)) {
                int sumDifficulties = spawn.Difficulty; 
                int maxDifficulty = (int)Math.Cbrt(sumDifficulties); 

                double difficultyD = 1d + (maxDifficulty - 1d)*Math.Pow(Util.NextDoublePositive(), 1d/maxDifficulty);
                int difficulty = Math.Min(12, (int)difficultyD); 
                
                EnemyType enemyType = EnemyID.GetRandomWithDifficulty(difficulty); 
                Vector2 topleft = w.GetCameraTopLeft(); 
                float cameraWidth = w.ScreenWidth; 
                float cameraHeight = w.ScreenHeight;
                
                float addX = Util.NextInt(2) == 1 ? 0f : cameraWidth; 
                float addY = Util.NextInt(2) == 1 ? 0f : cameraHeight; 
                addX += Util.NextFloat() * Constants.TileWidth;
                addY += Util.NextFloat() * Constants.TileWidth; 

                EnemyWrap.Draw(w, topleft + new Vector2(addX, addY), enemyType, LootWrap.GetDestination(w));

                spawn.Difficulty += difficulty; 
                Globals.MaxFloor = Math.Max(Globals.MaxFloor, spawn.Difficulty); 

                float secondsToSpawnF = (float)(1f + 60f*(1f/(1f + .005*sumDifficulties))*((difficulty)/12f));
                int secondsToSpawn = (int)secondsToSpawnF;
                spawn.NextSpawn = w.Time + new WorldTime(minutes: secondsToSpawn);
            }
        });
    }
}
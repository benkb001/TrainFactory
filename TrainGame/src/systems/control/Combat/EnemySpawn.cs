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

/*
TODO: Will probably add this back in
but should query for enemySpawner existing, not floor
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
*/

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
            Layout.DrawCombat(w, floor);
        }
    }
}

public static class EnemySpawnSystem {
    private static void spawn(World w, EnemySpawner spawn) {
        int sumDifficulties = spawn.Difficulty; 
        double maxDifficultyD = Math.Cbrt(sumDifficulties / 2); 
        int maxDifficulty = (int)maxDifficultyD;
        spawn.TargetNumActive = Math.Max(spawn.TargetNumActive, (int)(maxDifficultyD * 1.5d)); 
        int difficulty = 1 + Util.NextInt(maxDifficulty);
        //double difficultyD = 1d + (maxDifficulty - 1d)*Math.Pow(Util.NextDoublePositive(), 1d/maxDifficulty);
        //int difficulty = (int)difficultyD;
        
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
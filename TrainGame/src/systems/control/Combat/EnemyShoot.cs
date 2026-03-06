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

public static class EnemyShootSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Enemy), typeof(Shooter), typeof(Frame), typeof(Active)], (w, e) => {
            Shooter shooter = w.GetComponent<Shooter>(e);
            int playerEnt = PlayerWrap.GetRPGEntity(w); 
            Vector2 playerPos = w.GetComponent<Frame>(playerEnt).Position; 
            Frame f = w.GetComponent<Frame>(e);
            ShooterWrap.TryShoot(w, shooter, f, playerPos, ShooterType.Enemy); 
        });
    }
}
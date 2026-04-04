namespace TrainGame.Systems;

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Utils;
using TrainGame.Constants;

public class ShotBy {
    public int Entity; 
    public ShotBy(int e) {
        this.Entity = e; 
    }
}

public static class ShooterWrap {
    
    //returns the velocity needed to shoot at targetPos from pos with speed speed
    public static Vector2 Aim(Vector2 pos, Vector2 targetPos, float speed) {
        return targetPos == pos ? Vector2.Zero : (Vector2.Normalize(targetPos - pos)) * speed;
    }

    public static int Add<U>(World w, Vector2 pos, Vector2 targetPos, BulletContainer bc, int shooterEnt) 
    where U : IFlag<U> {
        float width = bc.Width;
        float height = bc.Height;
        
        int e = EntityFactory.AddUI(w, pos, width, height, setOutline: true);
        w.SetComponent<Velocity>(e, new Velocity(Aim(pos, targetPos, bc.Speed)));
        w.SetComponent<Bullet>(e, bc.GetBullet());
        w.SetComponent<U>(e, U.Get());
        w.SetComponent<ShotBy>(e, new ShotBy(shooterEnt));

        foreach (IBulletTrait bt in bc.GetBulletTraits()) {
            BulletTraitRegistry.Add(w, bt, e);
        }

        return e; 
    }
}
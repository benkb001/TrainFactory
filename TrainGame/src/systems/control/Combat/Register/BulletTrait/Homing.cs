namespace TrainGame.Systems;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Utils;
using TrainGame.Constants;

public static class HomingWrap {
    public static void RegisterTrait() {
        BulletTraitRegistry.Register<Homing>((w, homing, e) => {
            //ICKY: we could have a component that returns a targetable 
            //entity

            int trackedEntity = -1;
            if (w.ComponentContainsEntity<Player>(e) && w.ComponentContainsEntity<Frame>(e)) {
                Frame f = w.GetComponent<Frame>(e);
                float rangeSize = Constants.TileWidth * 12f; 
                Frame range = new Frame(f.Position - new Vector2(rangeSize / 2f, rangeSize / 2f), rangeSize, rangeSize);
                
                trackedEntity = MovementSystem.GetIntersectingEntities(w, range)
                .Where(e => w.ComponentContainsEntity<Health>(e) && w.ComponentContainsEntity<Enemy>(e))
                .FirstOrDefault();
            } else if (w.ComponentContainsEntity<Enemy>(e)) {
                trackedEntity = TargetableWrap.GetFirst(w);
            }

            w.SetComponent<Homing>(e, new Homing(trackedEntity, homing.Speed));
        });
    }
}
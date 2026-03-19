namespace TrainGame.Systems;

using System;
using System.Collections.Generic;

using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Utils;

public static class HomingWrap {
    public static void RegisterTrait() {
        BulletTraitRegistry.Register<Homing>((w, _, e) => {
            //ICKY: we could have a component that returns a targetable 
            //entity

            int trackedEntity = -1;
            if (w.ComponentContainsEntity<Player>(e)) {
                trackedEntity = EnemyWrap.GetFirst(w);
            } else if (w.ComponentContainsEntity<Enemy>(e)) {
                trackedEntity = TargetableWrap.GetFirst(w);
            }

            w.SetComponent<Homing>(e, new Homing(trackedEntity));
        });
    }
}
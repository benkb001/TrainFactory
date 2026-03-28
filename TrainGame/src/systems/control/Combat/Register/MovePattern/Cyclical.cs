namespace TrainGame.Systems;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TrainGame.Components;
using TrainGame.ECS;

public static class RegisterCyclicalMovement {
    public static void Register() {
        MovementRegistry.Register<CyclicalMovePattern>((w, m, e) => {
            w.SetComponent<CyclicalMovePattern>(e, m);
            w.SetComponent<MoveTiming>(e, new MoveTiming(w.Time));
        });
    }
}
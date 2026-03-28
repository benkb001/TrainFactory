namespace TrainGame.Systems;

using System.Collections.Generic;
using TrainGame.Components;
using TrainGame.ECS;

public static class RegisterDefaultMovementType {
    public static void Register() {
        MovementRegistry.Register<DefaultMovePattern>((w, m, e) => {
            w.SetComponent<DefaultMovePattern>(e, m);
            w.SetComponent<MoveTiming>(e, new MoveTiming(w.Time));
        });
    }
}
namespace TrainGame.Systems;

using System;
using TrainGame.Components;
using TrainGame.ECS;

public static class RegisterParametricBullet {
    public static void Register() {
        BulletTraitRegistry.Register<ParametricCurve>((w, sp, e) => {
            w.SetComponent<ParametricCurve>(e, sp.Clone());
        });
    }
}
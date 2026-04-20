namespace TrainGame.Systems;

using System;
using TrainGame.Components;
using TrainGame.ECS;

public static class FractalBulletTrait {
    public static void Register() {
        BulletTraitRegistry.Register<Fractal>((w, f, e) => {
            w.SetComponent<Fractal>(e, f.Clone());
        });
    }
}
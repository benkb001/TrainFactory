namespace TrainGame.Systems;

using System;
using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Constants;

public static class VampiricBulletTrait {
    public static void Register() {
        BulletTraitRegistry.Register<Vampiric>((w, sp, e) => {
            w.SetComponent<Vampiric>(e, sp); 
            w.SetComponent<Outline>(e, new Outline(Colors.Vampiric));
        });
    }
}
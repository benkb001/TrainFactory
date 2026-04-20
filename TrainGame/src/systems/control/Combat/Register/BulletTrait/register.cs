namespace TrainGame.Systems;

using System;
using TrainGame.Components;
using TrainGame.ECS;

public static class RegisterBulletTraits {
    private static void register<T>() where T : IBulletTrait {
        BulletTraitRegistry.Register<T>((w, bt, e) => w.SetComponent<T>(e, bt));
    }

    public static void All() {
        HomingWrap.RegisterTrait(); 
        WarnedWrap.RegisterTrait();
        RegisterParametricBullet.Register();
        VampiricBulletTrait.Register();
        AppliesKnockbackBulletTrait.Register();
        FractalBulletTrait.Register();
        register<RemoveOnCollision>();
        register<Split>();
    }
}

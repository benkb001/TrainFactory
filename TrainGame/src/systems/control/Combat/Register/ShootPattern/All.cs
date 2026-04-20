namespace TrainGame.Systems;

using System;
using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Utils;

public static class ShootPatternRegistry {
    private static CallbackRegistry<World, IShootPattern, int> registry = new(); 
    private static CallbackRegistry<World, IShootPattern, int> remove = new(); 

    public static void Register<T>(Action<World, T, int> callback) where T : IShootPattern {
        registry.Register<T>(callback);
        remove.Register<T>((w, sp, e) => {
            w.RemoveComponent<T>(e); 
        });
    }

    public static void Add(World w, IShootPattern p, int e) {
        if (w.ComponentContainsEntity<IShootPattern>(e)) {
            remove.Callback(w, w.GetComponent<IShootPattern>(e), e);
        }
        registry.Callback(w, p, e);
        w.SetComponent<IShootPattern>(e, p);
    }
}

public static class RegisterShootPattern {
    public static void Default<T>() where T : IShootPattern {
        ShootPatternRegistry.Register<T>((w, sp, e) => {
            w.SetComponent<T>(e, sp);
        });
    }
}

public static class RegisterShootPatterns {
    public static void All() {
        RegisterShootPattern.Default<DefaultShootPattern>();
        RegisterShootPattern.Default<RadialShootPattern>();
        RegisterShootPattern.Default<MeleeShootPattern>();
        RegisterShootPattern.Default<ShotgunShootPattern>();
        RegisterShootPattern.Default<GridShootPattern>();
        RegisterShootPattern.Default<RandomShotgunShootPattern>();
        RegisterShootPattern.Default<ShapeShootPattern>();
    }
}
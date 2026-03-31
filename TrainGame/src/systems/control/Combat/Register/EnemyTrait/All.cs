namespace TrainGame.Systems;

using System;
using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Utils;

public static class EnemyTraitRegistry {
    private static CallbackRegistry<World, IEnemyTrait, int> registry = new(); 

    public static void Register<T>(Action<World, T, int> callback) where T : IEnemyTrait {
        registry.Register<T>(callback);
    }

    public static void Add(World w, IEnemyTrait p, int e) {
        registry.Callback(w, p, e);
    }
}

public static class RegisterEnemyTraits {
    public static void register<T>() where T : IEnemyTrait {
        EnemyTraitRegistry.Register<T>((w, et, e) => {
            w.SetComponent<T>(e, et);
            Console.WriteLine($"registered {typeof(T)}");
        });
    }

    public static void All() {
        register<Split>();
    }
}
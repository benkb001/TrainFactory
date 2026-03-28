namespace TrainGame.Systems;

using System;
using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Utils;

public static class MovementRegistry {
    private static CallbackRegistry<World, IMovementType, int> registry = new(); 

    public static void Register<T>(Action<World, T, int> callback) where T : IMovementType {
        registry.Register<T>(callback); 
    }

    public static void AddMovement(World w, IMovementType t, int e) {
        registry.Callback(w, t, e);
    }
}

public static class RegisterMovementTypes {
    public static void All() {
        RegisterDefaultMovementType.Register();
        RegisterChaseMovementType.Register();
        RegisterCyclicalMovement.Register();
    }
}
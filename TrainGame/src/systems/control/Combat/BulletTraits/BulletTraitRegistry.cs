namespace TrainGame.Systems;

using System;
using System.Collections.Generic;

using TrainGame.Components;
using TrainGame.ECS;

public static class BulletTraitRegistry {

    private static Dictionary<Type, Action<World, IBulletTrait, int>> callbacks = new();

    public static void Register<T>(Action<World, T, int> callback) where T : IBulletTrait {
        Type t = typeof(T); 
        callbacks[t] = (World w, IBulletTrait trait, int e) => {
            if (trait is T component) {
                callback(w, component, e); 
            }
        };
    }

    public static void AddTrait(World w, IBulletTrait trait, int bulletEnt) {
        Type t = trait.GetType();
        if (callbacks.ContainsKey(t)) {
            callbacks[t](w, trait, bulletEnt);
        } else {
            throw new InvalidOperationException($"{t} has not been registered with BulletTraitRegistry");
        }
    }
}
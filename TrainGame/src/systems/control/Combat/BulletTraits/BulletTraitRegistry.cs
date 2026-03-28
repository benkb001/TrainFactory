namespace TrainGame.Systems;

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TrainGame.Components;
using TrainGame.ECS;

public class CallbackRegistry<CONTEXT, INTERFACE, OBJECT> {
    private Dictionary<Type, Action<CONTEXT, INTERFACE, OBJECT>> callbacks = new(); 

    public void Register<IMPLEMENTING>(Action<CONTEXT, IMPLEMENTING, OBJECT> callback) where IMPLEMENTING : INTERFACE {
        Type x = typeof(IMPLEMENTING); 

        callbacks[x] = (CONTEXT w, INTERFACE inter, OBJECT obj) => {
            if (inter is IMPLEMENTING imp) {
                callback(w, imp, obj);
            }
        };
    }

    public void Callback(CONTEXT w, INTERFACE t, OBJECT obj) {
        Type type = t.GetType(); 

        if (callbacks.ContainsKey(type)) {
            callbacks[type](w, t, obj);
        }
    }
}

public static class BulletTraitRegistry {
    private static CallbackRegistry<World, IBulletTrait, int> registry = new(); 

    public static void Register<T>(Action<World, T, int> callback) where T : IBulletTrait {
        registry.Register<T>(callback); 
    }

    public static void Add(World w, IBulletTrait trait, int bulletEnt) {
        registry.Callback(w, trait, bulletEnt);
    }
}

public static class MovementRegistry {
    private static CallbackRegistry<World, IMovementType, int> registry = new(); 

    public static void Register<T>(Action<World, T, int> callback) where T : IMovementType {
        registry.Register<T>(callback); 
    }

    public static void AddMovement(World w, IMovementType t, int e) {
        registry.Callback(w, t, e);
    }
}

public static class ShootPatternRegistry {
    private static CallbackRegistry<World, IShootPattern, int> registry = new(); 

    public static void Register<T>(Action<World, T, int> callback) where T : IShootPattern {
        registry.Register<T>(callback);
    }

    public static void Add(World w, IShootPattern p, int e) {
        registry.Callback(w, p, e);
    }
}
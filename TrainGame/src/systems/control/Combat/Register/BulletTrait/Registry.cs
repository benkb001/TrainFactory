namespace TrainGame.Systems;

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Utils;

public static class BulletTraitRegistry {
    private static CallbackRegistry<World, IBulletTrait, int> registry = new(); 

    public static void Register<T>(Action<World, T, int> callback) where T : IBulletTrait {
        registry.Register<T>(callback); 
    }

    public static void Add(World w, IBulletTrait trait, int bulletEnt) {
        registry.Callback(w, trait, bulletEnt);
    }
}
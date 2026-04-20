namespace TrainGame.Systems;
using System;
using Microsoft.Xna.Framework;
using TrainGame.Components;
using TrainGame.ECS;

public static class KnockbackSystem {
    public static void RegisterAdd(World w) {
        w.AddSystem([typeof(Knockback), typeof(Active)], (w, e) => {
            Vector2 velocity = w.ComponentContainsEntity<Velocity>(e) ? w.GetComponent<Velocity>(e) : Vector2.Zero; 
            Knockback kb = w.GetComponent<Knockback>(e); 
            velocity += kb; 
            w.SetComponent<Velocity>(e, new Velocity(velocity));
        });
    }

    public static void RegisterRemove(World w) {
        w.AddSystem([typeof(Knockback), typeof(Velocity), typeof(Active)], (w, e) => {
            Knockback kb = w.GetComponent<Knockback>(e); 
            w.GetComponent<Velocity>(e).Vector -= kb; 
            kb.V *= 0.9f; 
            if (kb.V.Length() < 0.1f) {
                w.RemoveComponent<Knockback>(e);
            }
        }); 
    }
}
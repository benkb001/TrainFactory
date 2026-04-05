namespace TrainGame.Systems;
using System;
using TrainGame.Components;
using TrainGame.ECS;

public class ApplyKnockbackSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(HitMessage), typeof(AppliesKnockback), typeof(Active)], (w, e) => {
            int hitEnt = w.GetComponent<HitMessage>(e).Entity; 
            w.SetComponent<Knockback>(hitEnt, new Knockback(w.GetComponent<AppliesKnockback>(e).V));
        });
    }
}
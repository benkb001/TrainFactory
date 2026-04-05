namespace TrainGame.Systems;
using System;
using TrainGame.Components;
using TrainGame.ECS;

public static class AppliesKnockbackBulletTrait {
    public static void Register() {
        BulletTraitRegistry.Register<AppliesKnockback>((w, applies, e) => {
            if (w.ComponentContainsEntity<Velocity>(e)) {
                applies.V = w.GetComponent<Velocity>(e).Vector * applies.Multiplier; 
                w.SetComponent<AppliesKnockback>(e, applies);
            }
        });
    }
}
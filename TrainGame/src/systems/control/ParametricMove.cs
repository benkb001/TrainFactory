namespace TrainGame.Systems;

using System;
using Microsoft.Xna.Framework;
using TrainGame.Components;
using TrainGame.ECS;

public class ParametricMovementSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Frame), typeof(Active), typeof(Velocity), typeof(ParametricCurve)], (w, e) => {
            ParametricCurve p = w.GetComponent<ParametricCurve>(e); 
            Vector2 d = p.GetDelta();

            w.SetComponent<Velocity>(e, new Velocity(d));
            p.T++;
        });
    }
}
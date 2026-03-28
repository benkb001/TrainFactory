namespace TrainGame.Components;

using System;
using Microsoft.Xna.Framework;

public class ParametricCurve : IBulletTrait {
    public int T;

    private Func<int, float> px; 
    private Func<int, float> py; 

    public ParametricCurve(Func<int, float> px, Func<int, float> py) {
        this.px = px; 
        this.py = py; 
        this.T = 0; 
    }

    public Vector2 GetDelta() => new Vector2(px(T) - px(T - 1), py(T) - py(T - 1)); 
    public ParametricCurve Clone() => new ParametricCurve(px, py);
}
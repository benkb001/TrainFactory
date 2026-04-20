namespace TrainGame.Components;

using System;
using Microsoft.Xna.Framework;

public class ParametricCurve : IBulletTrait {
    public int T;
    public int Range;

    private Func<int, float> px; 
    private Func<int, float> py; 

    public ParametricCurve(Func<int, float> px, Func<int, float> py, int Range = 100) {
        this.px = px; 
        this.py = py; 
        this.T = 0; 
        this.Range = 100;
    }

    public ParametricCurve(Func<int, (float, float)> p, int Range = 100) {
        this.Range = Range; 
        this.T = 0; 
        this.px = (t) => {
            (float x, float _) = p(t); 
            return x; 
        };

        this.py = (t) => {
            (float _, float y) = p(t); 
            return y; 
        };
    }

    public Vector2 GetDelta() => new Vector2(px(T) - px(T - 1), py(T) - py(T - 1)); 
    public Vector2 GetDelta(int T1, int T2) => new Vector2(px(T2) - px(T1), py(T2) - py(T1));
    public ParametricCurve Clone() => new ParametricCurve(px, py, Range);
}
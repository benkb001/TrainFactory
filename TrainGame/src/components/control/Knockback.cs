namespace TrainGame.Components;

using Microsoft.Xna.Framework; 

public class Knockback {
    public Vector2 V; 

    public Knockback(Vector2 v) {
        this.V = v; 
    }

    public static implicit operator Vector2(Knockback kb) {
        return kb.V;
    }
}
namespace TrainGame.Components;

using Microsoft.Xna.Framework; 

public class AppliesKnockback : IBulletTrait {
    public Vector2 V; 
    public float Multiplier;
    public AppliesKnockback(float Multiplier = 1f) {
        V = Vector2.Zero; 
        this.Multiplier = Multiplier; 
    }
}
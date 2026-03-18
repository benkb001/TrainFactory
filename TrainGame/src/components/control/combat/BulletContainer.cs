namespace TrainGame.Components;

using Microsoft.Xna.Framework;
using TrainGame.Utils;

public class BulletContainer {
    private Vector2 pos; 
    private Vector2 velocity; 
    private float width;
    private Bullet b; 

    public float GetWidth() => width;
    public Vector2 GetVelocity() => velocity; 
    public Vector2 GetPosition() => pos; 
    public Bullet GetBullet() => b;

    public readonly WorldTime WarningDuration;
    public bool IsWarned => WarningDuration != null;

    public BulletContainer(Bullet b, Vector2 pos, Vector2 vel, float w, WorldTime warningDuration = null) {
        this.b = b; 
        this.pos = pos; 
        this.velocity = vel; 
        this.width = w;
        this.WarningDuration = warningDuration;
    }
}
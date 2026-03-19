namespace TrainGame.Components;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TrainGame.Utils;
using TrainGame.Constants;

public class BulletContainer {
    private Vector2 pos = Vector2.Zero; 
    private Vector2 velocity = Vector2.Zero; 
    private float width;
    private float speed; 
    private Bullet b; 
    private IEnumerable<IBulletTrait> traits;

    public float GetWidth() => width;
    public Vector2 GetVelocity() => velocity; 
    public Vector2 GetPosition() => pos; 
    public Bullet GetBullet() => b;
    public float GetBulletSpeed() => speed;
    public IEnumerable<IBulletTrait> GetTraits() => traits;

    public BulletContainer(Bullet b, float width = Constants.DefaultBulletSize, float BulletSpeed = Constants.DefaultBulletSpeed, 
        IEnumerable<IBulletTrait> traits = null) {
        this.b = b; 
        this.width = width;
        this.speed = BulletSpeed; 
        if (traits == null) {
            traits = new List<IBulletTrait>();
        }
        this.traits = traits;
    }

    public BulletContainer Clone() {
        return new BulletContainer(b.Clone(), width, speed, traits);
    }

    public void SetVelocity(Vector2 v) {
        this.velocity = v; 
    }

    public void SetPosition(Vector2 v) {
        this.pos = v; 
    }

    public IEnumerable<IBulletTrait> GetBulletTraits() {
        return traits;
    }
}
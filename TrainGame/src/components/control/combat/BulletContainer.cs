namespace TrainGame.Components;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TrainGame.Utils;
using TrainGame.Constants;

public class BulletContainer {
    public readonly float Width;
    public readonly float Speed; 
    private Bullet b; 
    private IEnumerable<IBulletTrait> traits;

    public Bullet GetBullet() => b.Clone();
    public IEnumerable<IBulletTrait> GetTraits() => traits;

    public BulletContainer(Bullet b, float width = Constants.DefaultBulletSize, float BulletSpeed = Constants.DefaultBulletSpeed, 
        IEnumerable<IBulletTrait> traits = null) {
        
        this.b = b; 
        this.Width = width;
        this.Speed = BulletSpeed; 

        if (traits == null) {
            traits = new List<IBulletTrait>();
        }

        this.traits = traits;
    }

    public BulletContainer Clone() {
        return new BulletContainer(b.Clone(), Width, Speed, traits);
    }

    public IEnumerable<IBulletTrait> GetBulletTraits() {
        return traits;
    }
}
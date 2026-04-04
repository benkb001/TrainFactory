namespace TrainGame.Components;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TrainGame.Utils;
using TrainGame.Constants;

public class BulletContainer {
    public float Width;
    public float Height;
    public float Speed; 
    private Bullet b; 
    private List<IBulletTrait> traits;

    public Bullet GetBullet() => b.Clone();
    public List<IBulletTrait> GetTraits() => traits;

    public BulletContainer(Bullet b, Frame f = null, float BulletSpeed = Constants.DefaultBulletSpeed, 
        List<IBulletTrait> traits = null) {
        
        this.b = b; 
        this.Speed = BulletSpeed; 
        
        if (f == null) {
            Width = Constants.DefaultBulletSize;
            Height = Constants.DefaultBulletSize;
        } else {
            Width = f.GetWidth(); 
            Height = f.GetHeight();
        }

        if (traits == null) {
            traits = new List<IBulletTrait>();
        }

        this.traits = traits;
    }

    public void AddDamage(int dmg) {
        b.Damage += dmg; 
    }

    public BulletContainer Clone() {
        return new BulletContainer(b.Clone(), new Frame(Width, Height), Speed, traits);
    }

    public List<IBulletTrait> GetBulletTraits() {
        return traits;
    }

    public void AddTrait(IBulletTrait t) {
        traits.Add(t);
    }
}
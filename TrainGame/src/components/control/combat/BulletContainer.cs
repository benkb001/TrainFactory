namespace TrainGame.Components;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TrainGame.Utils;
using TrainGame.Constants;

public class BulletContainer {
    public float Width => frame.GetWidth();
    public float Height => frame.GetHeight(); 
    public float Rotation => frame.GetRotation(); 
    public readonly float Speed; 
    public Frame GetFrame() => frame; 
    private Frame frame; 
    private Bullet b; 
    private IEnumerable<IBulletTrait> traits;

    public Bullet GetBullet() => b.Clone();
    public IEnumerable<IBulletTrait> GetTraits() => traits;

    public BulletContainer(Bullet b, Frame f = null, float BulletSpeed = Constants.DefaultBulletSpeed, 
        IEnumerable<IBulletTrait> traits = null) {
        
        this.b = b; 
        this.Speed = BulletSpeed; 
        
        if (f == null) {
            f = new Frame(Constants.DefaultBulletSize, Constants.DefaultBulletSize); 
        }

        this.frame = f; 

        if (traits == null) {
            traits = new List<IBulletTrait>();
        }

        this.traits = traits;
    }

    public void AddTempDamage(int dmg) {
        b.TempDMG += dmg; 
    }

    public void Reset() {
        b.TempDMG = 0;
    }

    public BulletContainer Clone() {
        return new BulletContainer(b.Clone(), frame, Speed, traits);
    }

    public IEnumerable<IBulletTrait> GetBulletTraits() {
        return traits;
    }
}
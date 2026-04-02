namespace TrainGame.Components; 

using System;
using System.Collections.Generic;

public class ShotgunShootPattern : IShootPattern {
    public readonly double Radians; 
    public readonly BulletContainer Bullet; 
    public readonly int BulletsPerShot; 

    public ShotgunShootPattern(BulletContainer bc, int BulletsPerShot, double Radians) {
        this.Bullet = bc; 
        this.BulletsPerShot = BulletsPerShot;
        this.Radians = Radians;
    }

    public IShootPattern Clone() {
        return new ShotgunShootPattern(Bullet, BulletsPerShot, Radians);
    }
    public IEnumerable<BulletContainer> GetBulletContainers() => new List<BulletContainer>(){ Bullet };
}
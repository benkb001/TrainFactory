namespace TrainGame.Components;

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TrainGame.Systems;
using TrainGame.Constants;
using TrainGame.Utils;

public class RadialShootPattern : IShootPattern {
    public readonly int BulletsPerShot; 
    public readonly double OffsetRadians;
    public readonly BulletContainer Bullet; 

    public RadialShootPattern(int BulletsPerShot, BulletContainer Bullet, double OffsetRadians = 0d) {
        this.BulletsPerShot = BulletsPerShot; 
        this.Bullet = Bullet;
        this.OffsetRadians = OffsetRadians; 
    }

    public IShootPattern Clone() {
        return new RadialShootPattern(BulletsPerShot, Bullet.Clone(), OffsetRadians);
    }
    public IEnumerable<BulletContainer> GetBulletContainers() => new List<BulletContainer>(){ Bullet };
}
namespace TrainGame.Components;

using System;
using System.Collections.Generic;

public class ShapeShootPattern : IShootPattern {
    public BulletContainer Bullet;
    public ParametricCurve Shape; 
    public int BulletsPerShot; 

    public ShapeShootPattern(BulletContainer Bullet, ParametricCurve p, int bulletsPerShot) {
        this.Bullet = Bullet;
        this.Shape = p; 
        this.BulletsPerShot = bulletsPerShot;
    }

    public IShootPattern Clone() => new ShapeShootPattern(Bullet.Clone(), Shape, BulletsPerShot);
    public IEnumerable<BulletContainer> GetBulletContainers() => new List<BulletContainer>(){ Bullet };
}
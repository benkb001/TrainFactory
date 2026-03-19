namespace TrainGame.Components;

using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TrainGame.Systems;
using TrainGame.Constants;
using TrainGame.Utils;

public class DefaultShootPattern : IShootPattern {

    private BulletContainer bulletContainer; 

    public readonly float BulletSpeed; 
    public readonly int Damage;
    public readonly float Inaccuracy;

    public int GetBulletsShot() => 1; 

    public DefaultShootPattern(BulletContainer bulletContainer, float Inaccuracy = 0f) {
        this.bulletContainer = bulletContainer;
        this.BulletSpeed = bulletContainer.GetBulletSpeed();
    }

    public IEnumerable<BulletContainer> Shoot(Vector2 position, Vector2 targetPosition) {
        List<BulletContainer> bs = new();
        BulletContainer bc = bulletContainer.Clone();
        float offset = (float)(Inaccuracy * Util.NextDouble()); 
        targetPosition += new Vector2(offset, offset); 
        Vector2 velocity = ShooterWrap.Aim(position, targetPosition, BulletSpeed);
        bc.SetPosition(position);
        bc.SetVelocity(velocity);
        bs.Add(bc);
        return bs;
    }

    public IShootPattern Clone() {
        return new DefaultShootPattern(bulletContainer, Inaccuracy);
    }
}
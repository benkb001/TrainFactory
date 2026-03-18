namespace TrainGame.Components;

using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TrainGame.Systems;
using TrainGame.Constants;
using TrainGame.Utils;

public class DefaultShootPattern : IShootPattern {
    public readonly float BulletSpeed; 
    public readonly int Damage;
    public readonly float Inaccuracy;

    public int GetBulletsShot() => 1; 

    public DefaultShootPattern(float BulletSpeed = 2f, int Damage = 1, float Inaccuracy = 0f) {
        this.BulletSpeed = BulletSpeed; 
        this.Damage = Damage;
        this.Inaccuracy = Inaccuracy;
    }

    public IEnumerable<BulletContainer> Shoot(Vector2 position, Vector2 targetPosition) {
        List<BulletContainer> bs = new();
        float offset = (float)(Inaccuracy * Util.NextDouble()); 
        targetPosition += new Vector2(offset, offset); 
        Vector2 velocity = ShooterWrap.Aim(position, targetPosition, BulletSpeed);
        Bullet b = new Bullet(Damage);
        bs.Add(new BulletContainer(b, position, velocity, Constants.DefaultBulletSize));
        return bs;
    }

    public IShootPattern Clone() {
        return new DefaultShootPattern(BulletSpeed, Damage);
    }
}
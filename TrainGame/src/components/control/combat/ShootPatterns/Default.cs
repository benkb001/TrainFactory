namespace TrainGame.Components;

using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TrainGame.Systems;
using TrainGame.Constants;
using TrainGame.Utils;

public class DefaultShootPattern : IShootPattern {

    public readonly float Inaccuracy;
    public readonly BulletContainer Bullet;

    public DefaultShootPattern(BulletContainer Bullet, float Inaccuracy = 0) {
        this.Inaccuracy = Inaccuracy;
        this.Bullet = Bullet;
    }

    public IShootPattern Clone() {
        return new DefaultShootPattern(Bullet.Clone(), Inaccuracy);
    }

    public IEnumerable<BulletContainer> GetBulletContainers() => new List<BulletContainer>(){ Bullet };
}
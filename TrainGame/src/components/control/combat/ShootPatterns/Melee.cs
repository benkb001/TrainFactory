namespace TrainGame.Components;

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TrainGame.Systems;
using TrainGame.Constants;
using TrainGame.Utils;

public class MeleeShootPattern : IShootPattern {
    public BulletContainer Bullet;

    public MeleeShootPattern(BulletContainer Bullet) {
        this.Bullet = Bullet;
    }

    public IShootPattern Clone() => new MeleeShootPattern(Bullet);
}
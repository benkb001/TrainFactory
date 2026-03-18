namespace TrainGame.Components;

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TrainGame.Systems;
using TrainGame.Constants;
using TrainGame.Utils;

public class MeleeShootPattern : IShootPattern {
    public readonly int Damage;
    public readonly WorldTime WarningDuration;
    public readonly float Width;
    public readonly float ShooterWidth;

    public MeleeShootPattern(int Damage, WorldTime WarningDuration, float Width = Constants.TileWidth, float ShooterWidth = Constants.TileWidth) {
        this.Damage = Damage;
        this.Width = Width;
        this.WarningDuration = WarningDuration;
        this.ShooterWidth = ShooterWidth;
    }

    public int GetBulletsShot() => 1; 

    public IEnumerable<BulletContainer> Shoot(Vector2 position, Vector2 _) {
        Bullet b = new Bullet(Damage, maxFramesActive: 10);

        float delta = (Width - ShooterWidth) / 2f; 
        Vector2 bulletPos = new Vector2(position.X - delta, position.Y - delta);
        BulletContainer bc = new BulletContainer(b, bulletPos, Vector2.Zero, Width, WarningDuration);
        return new List<BulletContainer>() { bc };
    }

    public IShootPattern Clone() => new MeleeShootPattern(Damage, WarningDuration, Width);
}
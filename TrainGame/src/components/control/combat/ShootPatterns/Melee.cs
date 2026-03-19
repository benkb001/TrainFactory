namespace TrainGame.Components;

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TrainGame.Systems;
using TrainGame.Constants;
using TrainGame.Utils;

public class MeleeShootPattern : IShootPattern {
    private BulletContainer bulletContainer;

    public readonly float Width;
    public readonly float ShooterWidth;

    public MeleeShootPattern(BulletContainer bulletContainer, float ShooterWidth = Constants.TileWidth) {
        this.bulletContainer = bulletContainer;
        this.Width = bulletContainer.GetWidth();
        this.ShooterWidth = ShooterWidth;
    }

    public int GetBulletsShot() => 1; 

    public IEnumerable<BulletContainer> Shoot(Vector2 position, Vector2 _) {
        //ICKY: I don't like that these classes have to know to clone the bulletContainers. 
        //Could pass a BulletContainer.Factory object instead and make bulletContainer
        // constructor private? 
        BulletContainer bc = bulletContainer.Clone();

        float delta = (Width - ShooterWidth) / 2f; 
        Vector2 bulletPos = new Vector2(position.X - delta, position.Y - delta);
        bc.SetPosition(bulletPos); 
        bc.SetVelocity(Vector2.Zero);
        return new List<BulletContainer>() { bc };
    }

    public IShootPattern Clone() => new MeleeShootPattern(bulletContainer, ShooterWidth);
}
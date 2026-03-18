namespace TrainGame.Components;

using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Systems;

public interface IShootPattern {
    int GetBulletsShot();
    IShootPattern Clone();
    IEnumerable<BulletContainer> Shoot(Vector2 pos, Vector2 targetPos);
}
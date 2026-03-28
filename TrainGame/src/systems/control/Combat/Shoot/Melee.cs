namespace TrainGame.Systems;

using Microsoft.Xna.Framework;
using TrainGame.Components;
using TrainGame.ECS;

public static class MeleeShootSystem {
    public static void Register<U>(World w) where U : IFlag<U> {
        ShootSystem.Register<MeleeShootPattern, U>(w, (w, sp, f, targetPosition, e) => {
            float width = sp.Bullet.Width;
            float shooterWidth = f.GetWidth();
            Vector2 position = f.Position; 

            float delta = (width - shooterWidth) / 2f; 
            Vector2 bulletPos = new Vector2(position.X - delta, position.Y - delta);
            ShooterWrap.Add<U>(w, bulletPos, bulletPos, sp.Bullet, e);
            return 1; 
        });
    }
}
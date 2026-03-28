namespace TrainGame.Systems;

using Microsoft.Xna.Framework;
using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Utils;

public static class DefaultShootSystem {
    public static void Register<U>(World w) 
    where U : IFlag<U> {
        ShootSystem.Register<DefaultShootPattern, U>(w, (w, sp, f, targetPosition, e) => {
            float offset = (float)(sp.Inaccuracy * Util.NextDouble()); 
            targetPosition += new Vector2(offset, offset); 
            ShooterWrap.Add<U>(w, f.Position, targetPosition, sp.Bullet);
            return 1; 
        });
    }
}
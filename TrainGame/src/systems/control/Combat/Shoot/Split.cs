namespace TrainGame.Systems;

using Microsoft.Xna.Framework;
using TrainGame.ECS;
using TrainGame.Components;

//required order: register collided/expired -> splitSystem -> shootSystem -> removeOnCollided/removeOnExpired

public static class SplitBulletSystem {
    private static void register<T, U>(World w) where U : ISplitter {
        w.AddSystem([typeof(T), typeof(U), typeof(Frame), typeof(Velocity), typeof(Split)], (w, e) => {
            IShootPattern sp = w.GetComponent<U>(e).GetPattern();
            //ICKY, but we add Shooter so that it passes the check for ShootSystem
            Vector2 target = w.GetComponent<Frame>(e).Position + w.GetComponent<Velocity>(e); 
            w.SetComponent<Shooter>(e, new Shooter());
            w.SetComponent<ShotMessage>(e, new ShotMessage(target));
            ShootPatternRegistry.Add(w, sp, e);
            w.RemoveComponent<U>(e);
        });
    }

    public static void Register(World w) {
        register<Collided, Split>(w);
        register<Expired, Split>(w);
    }
}
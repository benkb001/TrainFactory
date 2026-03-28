namespace TrainGame.Systems;

using TrainGame.Components;
using TrainGame.ECS;

public static class RemoveShotMessageSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(ShotMessage), typeof(Active)], (w, e) => {
            w.RemoveComponent<ShotMessage>(e);
        });
    }
}
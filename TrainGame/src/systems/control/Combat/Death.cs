namespace TrainGame.Systems; 

using TrainGame.Components; 
using TrainGame.ECS; 

public static class DeathSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Health), typeof(Active)], (w, e) => {
            if (w.GetComponent<Health>(e).HP <= 0) {
                w.RemoveEntity(e); 
            }
        });
    }
}
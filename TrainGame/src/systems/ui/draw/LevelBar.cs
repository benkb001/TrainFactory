namespace TrainGame.Systems;

using TrainGame.Components;
using TrainGame.ECS;

public class LevelBar {
    public IExperienceTracker Tracker; 
    public LevelBar(IExperienceTracker Tracker) {
        this.Tracker = Tracker;
    }
}

public static class LevelBarDrawSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(ProgressBar), typeof(LevelBar), typeof(Active)], (w, e) => {
            IExperienceTracker track = w.GetComponent<LevelBar>(e).Tracker;
            float completion = (float)track.GetXP() / track.GetXPToNextLevel(); 
            w.GetComponent<ProgressBar>(e).Completion = completion; 
        });
    }
}
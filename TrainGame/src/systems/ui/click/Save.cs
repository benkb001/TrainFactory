namespace TrainGame.Systems; 

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants;
using TrainGame.Utils; 

public static class SaveClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<SaveButton>(w, (w, e) => {
            PersistentState.Save(w, Constants.DefaultSaveFile); 
        });
    }
}

public static class LoadClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<LoadButton>(w, (w, e) => {
            PersistentState.Load(w, Constants.DefaultSaveFile); 
        });
    }
}
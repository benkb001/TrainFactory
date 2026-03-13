namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 

public class PauseButtonSystem() {
    private static Type[] ts = [typeof(PauseButton), typeof(Button), typeof(Active)]; 
    private static Action<World, int> tf = (w, e) => {
        if (w.GetComponent<Button>(e).Clicked) {
            WorldTimeWrap.SetTimePassPaused(w); 
        }
    }; 

    public static void Register(World w) {
        w.AddSystem(ts, tf); 
    }
}

public class UnpauseButtonSystem() {
    public static void Register(World w) {
        ClickSystem.Register<UnpauseButton>(w, (w, e) => WorldTimeWrap.SetTimePassSlow(w)); 
    }
}

public class SpeedTimeClickSystem() {
    public static void Register(World w) {
        ClickSystem.Register<SpeedTimeButton>(w, (w, e) => {
            WorldTimeWrap.SetTimePassFast(w); 
        }); 
    }
}

public class SlowTimeClickSystem() {
    public static void Register(World w) {
        ClickSystem.Register<SlowTimeButton>(w, (w, e) => {
            WorldTimeWrap.SetTimePassSlow(w); 
        }); 
    }
}

public static class WorldTimeWrap {
    public static void SetTimePassSlow(World w) {
        w.SetMiliticksPerUpdate(1000); 
    }

    public static void SetTimePassPaused(World w) {
        w.SetMiliticksPerUpdate(0); 
    }

    public static void SetTimePassFast(World w) {
        w.SetMiliticksPerUpdate(3000); 
    }
}
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
            w.SetMiliticksPerUpdate(0); 
        }
    }; 

    public static void Register(World w) {
        w.AddSystem(ts, tf); 
    }
}

public class UnpauseButtonSystem() {
    private static Type[] ts = [typeof(UnpauseButton), typeof(Button), typeof(Active)]; 
    private static Action<World, int> tf = (w, e) => {
        w.SetMiliticksPerUpdate(1000); 
    }; 

    public static void Register(World w) {
        w.AddSystem(ts, tf); 
    }
}

public class SpeedTimeClickSystem() {
    public static void Register(World w) {
        ClickSystem.Register<SpeedTimeButton>(w, (w, e) => {
            w.SetMiliticksPerUpdate(3000); 
        }); 
    }
}

public class SlowTimeClickSystem() {
    public static void Register(World w) {
        ClickSystem.Register<SlowTimeButton>(w, (w, e) => {
            w.SetMiliticksPerUpdate(1000); 
        }); 
    }
}
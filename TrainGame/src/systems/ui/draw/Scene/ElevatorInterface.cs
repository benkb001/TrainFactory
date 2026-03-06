namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.ECS;
using TrainGame.Constants; 
using TrainGame.Callbacks; 
using TrainGame.Utils;

public class ElevatorButton {
    private Stepper step; 
    public int Floor => step.Value;

    public ElevatorButton(Stepper step) {
        this.step = step; 
    }
}

public static class DrawElevatorInterfaceSystem {
    public static void Register(World w) {
        DrawInterfaceSystem.Register<ElevatorInterfaceData>(w, (w, e, data) => {
            LinearLayoutContainer outer = LinearLayoutWrap.AddOuter(w, "Elevator"); 
            StepperContainer step = StepperWrap.Draw(
                w,
                100f, 
                200f, 
                "Go To Floor", 
                defaultVal: 0, 
                min: 0, 
                max: Globals.MaxFloor - (Globals.MaxFloor % 5), 
                step: 5
            );
            outer.AddChild(step.ContainerEnt, w); 
            w.SetComponent<ElevatorButton>(step.SubmitEnt, new ElevatorButton(step.Step));
        });
    }
}

public static class ElevatorSystem {
    public static void Register(World w) {
        ClickSystem.Register<ElevatorButton>(w, (w, e, btn) => {
            FloorSystem.GoToFloor(w, btn.Floor); 
        });
    }
}
namespace TrainGame.Components;

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Systems;

public class PauseTrainProgramButton {
    private Train train; 
    public Train GetTrain() => train; 

    public PauseTrainProgramButton(Train train) {
        this.train = train; 
    }
}

public static class PauseTrainProgramButtonWrap {
    public static string PauseMessage = "Pause train program?"; 
    public static string UnpauseMessage = "Unpause train program?";

    public static int Add(World w, Train t, float width, float height) {
        if (t.Executable != null) {
            bool paused = t.Executable.Paused; 
            string message = paused ? UnpauseMessage : PauseMessage; 

            int e = EntityFactory.AddUI(w, Vector2.Zero, width, height, setButton: true, setOutline: true, 
            text: message);
            w.SetComponent<PauseTrainProgramButton>(e, new PauseTrainProgramButton(t)); 
            return e; 
        }

        throw new InvalidOperationException("Cannot create a pause train program button for a train without a program"); 
    }
}

public class PauseTrainProgramButtonClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<PauseTrainProgramButton>(w, (w, e, pb) => {
            Train t = pb.GetTrain(); 
            if (t.Executable.Paused) {
                t.Executable.Unpause(); 
                w.GetComponent<TextBox>(e).Text = PauseTrainProgramButtonWrap.PauseMessage;
            } else {
                t.Executable.Pause(); 
                w.GetComponent<TextBox>(e).Text = PauseTrainProgramButtonWrap.UnpauseMessage;
            }
        });
    }
}
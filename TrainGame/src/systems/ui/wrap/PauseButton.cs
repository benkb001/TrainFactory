namespace TrainGame.Systems;

using System;

using Microsoft.Xna.Framework;

using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Constants;

public static class PauseTrainProgramButtonWrap {
    public static string PauseMessage = "Pause train program?"; 
    public static string UnpauseMessage = "Unpause train program?";

    public static int Add(World w, Train t, float width, float height) {
        if (t.Executable != null) {
            bool paused = t.Executable.Paused(); 
            string message = paused ? UnpauseMessage : PauseMessage; 

            int e = EntityFactory.AddUI(w, Vector2.Zero, width, height, setButton: true, setOutline: true, 
            text: message);
            w.SetComponent<PauseTrainProgramButton>(e, new PauseTrainProgramButton(t)); 
            return e; 
        }

        throw new InvalidOperationException("Cannot create a pause train program button for a train without a program"); 
    }
}
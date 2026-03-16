namespace TrainGame.Systems;

using System;

using Microsoft.Xna.Framework;

using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Constants;

public static class PauseTrainProgramButtonWrap {
    public static string PauseMessage = "Pause train program?"; 
    public static string UnpauseMessage = "Unpause train program?";

    public static int Add(World w, TALBody<Train, City> exe, float width, float height) {
        
        bool paused = exe.Paused(); 
        string message = paused ? UnpauseMessage : PauseMessage; 

        int e = EntityFactory.AddUI(w, Vector2.Zero, width, height, setButton: true, setOutline: true, text: message);
        w.SetComponent<PauseTrainProgramButton>(e, new PauseTrainProgramButton(exe)); 
        return e; 
    }
}
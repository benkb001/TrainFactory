namespace TrainGame.Systems;

using System;

using TrainGame.Components;
using TrainGame.ECS;

public static class PauseTrainProgramButtonClickSystem {
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
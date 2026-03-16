namespace TrainGame.Systems;

using System;

using TrainGame.Components;
using TrainGame.ECS;

public static class PauseTrainProgramButtonClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<PauseTrainProgramButton>(w, (w, e, pb) => {
            TALBody<Train, City> exe = pb.Executable; 
            if (exe.Paused()) {
                exe.Unpause(); 
                w.GetComponent<TextBox>(e).Text = PauseTrainProgramButtonWrap.PauseMessage;
            } else {
                exe.Pause(); 
                w.GetComponent<TextBox>(e).Text = PauseTrainProgramButtonWrap.UnpauseMessage;
            }
        });
    }
}
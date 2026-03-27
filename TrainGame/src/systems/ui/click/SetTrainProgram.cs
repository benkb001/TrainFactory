namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public static class SetTrainProgramClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<SetTrainProgramButton>(w, (w, e) => {
            SetTrainProgramButton btn = w.GetComponent<SetTrainProgramButton>(e); 

            Train t = btn.GetTrain(); 
            int trainEntity = btn.TrainEntity;
            (City comingFrom, bool hasComingFrom) = TrainWrap.GetComingFrom(w, trainEntity);
            
            if (!hasComingFrom) {
                return;
            }

            Inventory inv = comingFrom.Inv;
            string program = btn.Program;
            string programName = btn.ProgramName; 

            TAL.BuyTrainProgram(program, t, inv, btn.TrainEntity, w, programName);
        });
    }
}
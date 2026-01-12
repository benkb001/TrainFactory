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
            Inventory inv = t.ComingFrom.Inv; 
            string program = btn.Program;
            string programName = btn.ProgramName; 

            TAL.BuyTrainProgram(program, t, w, programName);
            Console.WriteLine(programName); 
        });
    }
}
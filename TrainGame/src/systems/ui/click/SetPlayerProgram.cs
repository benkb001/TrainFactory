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

public static class SetPlayerProgramClickSystem {
    public static void Register(World w) {
        ClickSystem.Register([typeof(SetPlayerProgramButton), typeof(SetTrainProgramButton)], w, (w, e) => {
            SetPlayerProgramButton playerBtn = w.GetComponent<SetPlayerProgramButton>(e); 
            SetTrainProgramButton programBtn = w.GetComponent<SetTrainProgramButton>(e); 

            string programName = playerBtn.ProgramNameInput.Text; 
            string program = playerBtn.ProgramInput.Text; 
            TAL.PlayerScripts[programName] = program; 

            programBtn.SetProgram(programName, program);
        });
    }
}
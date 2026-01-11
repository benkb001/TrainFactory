namespace TrainGame.Components; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class SetPlayerProgramButton {
    public readonly TextInput ProgramNameInput; 
    public readonly TextInput ProgramInput;

    public SetPlayerProgramButton(TextInput ProgramNameInput, TextInput ProgramInput) {
        this.ProgramNameInput = ProgramNameInput; 
        this.ProgramInput = ProgramInput; 
    }
}
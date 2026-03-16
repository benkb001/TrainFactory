namespace TrainGame.Components;

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Systems;

public class PauseTrainProgramButton {
    public readonly TALBody<Train, City> Executable;
    
    public PauseTrainProgramButton(TALBody<Train, City> Executable) {
        this.Executable = Executable;
    }
}
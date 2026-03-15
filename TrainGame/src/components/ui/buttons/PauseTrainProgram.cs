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
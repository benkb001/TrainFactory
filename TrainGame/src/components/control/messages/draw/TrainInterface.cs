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
using TrainGame.Callbacks; 

public class DrawTrainInterfaceMessage {
    private Train train; 

    public DrawTrainInterfaceMessage(Train train) {
        this.train = train; 
    }

    public Train GetTrain() {
        return train; 
    }
}
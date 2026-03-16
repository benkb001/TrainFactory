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
    private int trainEntity;
    public int TrainEntity => trainEntity; 

    public DrawTrainInterfaceMessage(Train train, int trainEntity) {
        this.train = train; 
        this.trainEntity = trainEntity;
    }

    public Train GetTrain() {
        return train; 
    }
}
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

public class EnterInterfaceButton<T> {
    public readonly T Data; 
    public EnterInterfaceButton(T Data) {
        this.Data = Data;
    }
}

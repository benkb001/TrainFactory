namespace TrainGame.Components; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants;
using TrainGame.Callbacks; 

public class DrawCityInterfaceMessage {
    private City city; 
    public City GetCity() => city; 

    public DrawCityInterfaceMessage(City city) {
        this.city = city; 
    }
}
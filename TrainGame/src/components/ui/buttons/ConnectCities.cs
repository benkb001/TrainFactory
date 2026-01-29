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

public class ConnectCitiesButton {
    private City c1; 
    private City c2; 
    private Dictionary<string, int> cost; 

    public City GetOrigin() => c1; 

    public ConnectCitiesButton(City c1, City c2, Dictionary<string, int> cost) {
        this.c1 = c1; 
        this.c2 = c2; 
        this.cost = cost; 
    }

    public void TryConnect() {
        if (c1.Inv.TakeRecipe(cost)) {
            c1.AddConnection(c2); 
        }
    }
}
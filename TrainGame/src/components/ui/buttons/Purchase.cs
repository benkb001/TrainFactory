namespace TrainGame.Components; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Callbacks; 

public class PurchaseButton<T> where T : IBuyable {

    public readonly T Buyable;
    public Dictionary<string, int> Cost => Buyable.GetCost(); 

    public PurchaseButton(T Buyable) {
        this.Buyable = Buyable;
    }
}

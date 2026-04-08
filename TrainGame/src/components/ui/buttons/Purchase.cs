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
    public Dictionary<string, int> Cost;
    public readonly Inventory Source;

    public PurchaseButton(T Buyable, Dictionary<string, int> Cost, Inventory Source) {
        this.Buyable = Buyable;
        this.Cost = Cost;
        this.Source = Source;
    }
}

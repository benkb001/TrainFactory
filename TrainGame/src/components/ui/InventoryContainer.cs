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

public class InventoryContainer<T> where T : IInventorySource {
    public int Index; 
    private T src; 

    public InventoryContainer(T src) {
        this.src = src; 
        Index = 0; 
    }

    public List<Inventory> GetInventories() {
        return src.GetInventories();
    }
}
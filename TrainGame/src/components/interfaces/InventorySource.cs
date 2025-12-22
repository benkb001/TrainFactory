namespace TrainGame.Components; 

using System.Collections.Generic;

public interface IInventorySource {
    List<Inventory> GetInventories(); 
}

using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class ChestInteractSystemTest {
    
    [Fact]
    public void ChestInteractSystem_ShouldDrawAChestInventoryAndAPlayerInventory() {
        World w = WorldFactory.Build(); 

        int playerInvEntity = EntityFactory.Add(w);
        int chestInvEntity = EntityFactory.Add(w);

        Inventory chestInv = new Inventory("Chest", 2, 2); 
        Inventory playerInv = new Inventory("Player", 3, 3); 

        w.SetComponent<Inventory>(playerInvEntity, playerInv); 
        w.SetComponent<Inventory>(chestInvEntity, chestInv); 

        int chestEntity = EntityFactory.Add(w);
        w.SetComponent<Interactable>(chestEntity, new Interactable(true)); 

        Chest chest = new Chest(chestInv, playerInv); 
        w.SetComponent<Chest>(chestEntity, chest); 

        w.Update(); 
        
        List<int> es = w.GetMatchingEntities([typeof(Inventory), typeof(LinearLayout)]); 
        Assert.Equal(2, es.Count);
        Assert.Single(es, e => w.GetComponent<Inventory>(e) == chestInv); 
        Assert.Single(es, e => w.GetComponent<Inventory>(e) == playerInv); 
    }
}

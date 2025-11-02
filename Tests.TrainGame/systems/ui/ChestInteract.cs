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
    
    private void RegisterDependencies(World w) {

        InteractSystem.RegisterInteract(w); 

        ChestInteractSystem.Register(w); 
        //removed so that it does not consume generated drawInventoryMessages 
        //InventoryUISystem.RegisterBuild(w); 
        
        InventoryControlSystem.RegisterUpdate(w); 
        LinearLayoutSystem.Register(w); 

        InventoryControlSystem.RegisterOrganize(w); 

        InteractSystem.RegisterUninteract(w); 
    }
    [Fact]
    public void ChestInteractSystem_ShouldGenerateTwoDrawInventoryMessagesWithCorrespondingInventories() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterDependencies(w); 

        int playerInvEntity = w.AddEntity(); 
        int chestInvEntity = w.AddEntity();

        Inventory chestInv = new Inventory("Chest", 2, 2); 
        Inventory playerInv = new Inventory("Player", 3, 3); 

        w.SetComponent<Inventory>(playerInvEntity, playerInv); 
        w.SetComponent<Inventory>(chestInvEntity, chestInv); 

        int chestEntity = w.AddEntity(); 
        w.SetComponent<Interactable>(chestEntity, new Interactable(true)); 

        Chest chest = new Chest(chestInv, chestInvEntity, playerInv, playerInvEntity); 
        w.SetComponent<Chest>(chestEntity, chest); 

        w.Update(); 
        
        List<KeyValuePair<int, DrawInventoryMessage>> msg_ls = w.GetComponentArray<DrawInventoryMessage>().ToList(); 
        Assert.Equal(2, msg_ls.Count);

        DrawInventoryMessage msg1 = msg_ls[0].Value; 
        DrawInventoryMessage msg2 = msg_ls[1].Value; 
        
        Assert.True(msg1.Inv == chestInv || msg2.Inv == chestInv); 
        Assert.True(msg1.Inv == playerInv || msg2.Inv == playerInv); 
    }
}

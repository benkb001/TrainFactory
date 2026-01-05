
using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Callbacks; 

//sequential because global state (mouse)
[Collection("Sequential")]
public class InvetoryPickUpUISystemTest {
    [Fact]
    public void InventoryPickUpUISystem_ShouldSetDepthOfHeldCell() {
        VirtualMouse.Reset(); 
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 
        
        Inventory inv = new Inventory("Test1", 2, 2); 

        Inventory.Item apple = new Inventory.Item(ItemId: "Apple", Count: 2); 
        inv.Add(apple, 1, 1); 

        int invEntity = EntityFactory.Add(w); 

        w.SetComponent<Inventory>(invEntity, inv); 

        DrawInventoryCallback.Create(w, inv, Vector2.Zero, 100, 100, Entity: invEntity);

        w.Update(); 

        VirtualMouse.SetCoordinates(5, 5); 
        VirtualMouse.LeftPress(); 
        w.Update(); 

        LinearLayout ll1 = w.GetComponent<LinearLayout>(invEntity); 
        LinearLayout row1 = w.GetComponent<LinearLayout>(ll1.GetChildren()[0]); 
        int heldEntity = row1.GetChildren()[0]; 

        TextBox heldTB = w.GetComponent<TextBox>(heldEntity); 
        Outline heldOutline = w.GetComponent<Outline>(heldEntity); 
        Background heldBG = w.GetComponent<Background>(heldEntity); 

        Assert.Equal(Constants.InventoryHeldTextBoxDepth, heldTB.Depth); 
        Assert.Equal(Constants.InventoryHeldBackgroundDepth, heldBG.Depth); 
        Assert.Equal(Constants.InventoryHeldOutlineDepth, heldOutline.Depth); 

        VirtualMouse.Reset(); 
    }
}

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
public class InvetoryDropUISystemTest {
    [Fact]
    public void InventoryDropUISystem_ShouldResetDepthOfHeldCell() {
        VirtualMouse.Reset(); 
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 
        
        Inventory inv = new Inventory("Test1", 2, 2); 

        Inventory.Item apple = new Inventory.Item(ItemId: "Apple", Count: 2); 
        inv.Add(apple, 1, 1); 

        InventoryView invView = DrawInventoryCallback.Draw(w, inv, Vector2.Zero, 100, 100);
        int invEntity = invView.GetInventoryEntity(); 

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

        VirtualMouse.LeftRelease(); 

        w.Update(); 
        
        Assert.Equal(Constants.InventoryCellTextBoxDepth, heldTB.Depth); 
        Assert.Equal(Constants.InventoryCellBackgroundDepth, heldBG.Depth); 
        Assert.Equal(Constants.InventoryCellOutlineDepth, heldOutline.Depth); 

        VirtualMouse.Reset(); 
    }
}
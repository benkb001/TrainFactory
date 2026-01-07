
using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 
using TrainGame.Callbacks; 

public class InventoryUISystemTest {
    [Fact]
    public void InventoryUISystem_ShouldAddInventoryToInvEntity() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        Inventory inv = new Inventory("Test", 5, 5); 

        InventoryView invView = DrawInventoryCallback.Draw(w, inv, Vector2.Zero, 100, 100);

        Assert.Equal(inv, w.GetComponent<Inventory>(invView.GetInventoryEntity())); 
    }

    [Fact]
    public void InventoryUISystem_ShouldBuildUIWithCorrectDimensions() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        Inventory inv = new Inventory("Test", 5, 5); 
        
        InventoryView invView = DrawInventoryCallback.Draw(w, inv, Vector2.Zero, 100, 200f);
        w.Update(); 

        Frame f = w.GetComponent<Frame>(invView.GetParentEntity()); 
        Assert.Equal(100f, f.GetWidth()); 
        Assert.Equal(200f, f.GetHeight()); 
    }

    [Fact]
    public void InventoryUISystem_ShouldCreateLinearLayoutWithNumberOfChildrenEqualToRows() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        Inventory inv = new Inventory("Test", 10, 5); 
        
        InventoryView invView = DrawInventoryCallback.Draw(w, inv, Vector2.Zero, 100, 200);

        w.Update(); 

        LinearLayout ll = w.GetComponent<LinearLayout>(invView.GetInventoryEntity()); 
        Assert.Equal(10, ll.GetChildren().Count); 
    }

    [Fact]
    public void InventoryUISystem_EachChildOfCreatedLinearLayoutShouldHaveChildrenEqualToNumColumns() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        Inventory inv = new Inventory("Test", 10, 5); 

        InventoryView invView = DrawInventoryCallback.Draw(w, inv, Vector2.Zero, 100, 200);

        w.Update(); 

        LinearLayout ll = w.GetComponent<LinearLayout>(invView.GetInventoryEntity()); 
        bool rowsNumChildrenAndNumColsAreEqual = true; 

        foreach (int c in ll.GetChildren()) {
            LinearLayout rowLL = w.GetComponent<LinearLayout>(c); 
            if (rowLL.GetChildren().Count != inv.GetCols()) {
                rowsNumChildrenAndNumColsAreEqual = false; 
                break;
            }
        }

        Assert.True(rowsNumChildrenAndNumColsAreEqual); 
    }

    [Fact]
    public void InventoryUISystem_DrawShouldUseSpecifiedInventory() {
        World w = WorldFactory.Build(); 
        Inventory inv = new Inventory("Test", 2, 2); 
        InventoryView invView = DrawInventoryCallback.Draw(w, inv, Vector2.Zero, 0f, 0f); 
        Assert.Equal(inv, invView.GetInventory());
    }
    //TODO: Handle possibility of making it with padding too much to be drawn and it makes row widths negative
}
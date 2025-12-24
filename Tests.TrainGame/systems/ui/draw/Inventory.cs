
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

//TODO: rename this cuz its actually testing DrawInventoryCallback
public class InventoryUISystemTest {
    [Fact]
    public void InventoryUISystem_ShouldAddLinearLayoutAndFrameToEntity() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int inventoryEntity = EntityFactory.Add(w);
        Inventory inv = new Inventory("Test", 5, 5); 

        DrawInventoryCallback.Create(w, inv, Vector2.Zero, 100, 100, Entity: inventoryEntity);

        w.Update(); 

        Assert.True(w.ComponentContainsEntity<LinearLayout>(inventoryEntity)); 
        Assert.True(w.ComponentContainsEntity<Frame>(inventoryEntity)); 
    }

    [Fact]
    public void InventoryUISystem_ShouldBuildUIWithCorrectDimensions() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int inventoryEntity = EntityFactory.Add(w); 
        Inventory inv = new Inventory("Test", 5, 5); 
        
        DrawInventoryCallback.Create(w, inv, Vector2.Zero, 100, 200f, Entity: inventoryEntity);
        w.Update(); 

        Frame f = w.GetComponent<Frame>(inventoryEntity); 
        Assert.Equal(100f, f.GetWidth()); 
        Assert.Equal(200f, f.GetHeight()); 
    }

    [Fact]
    public void InventoryUISystem_ShouldCreateLinearLayoutWithNumberOfChildrenEqualToRows() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int inventoryEntity = EntityFactory.Add(w); 
        Inventory inv = new Inventory("Test", 10, 5); 
        
        DrawInventoryCallback.Create(w, inv, Vector2.Zero, 100, 200, Entity: inventoryEntity);

        w.Update(); 

        LinearLayout ll = w.GetComponent<LinearLayout>(inventoryEntity); 
        Assert.Equal(10, ll.GetChildren().Count); 
    }

    [Fact]
    public void InventoryUISystem_EachChildOfCreatedLinearLayoutShouldHaveChildrenEqualToNumColumns() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int inventoryEntity = EntityFactory.Add(w); 
        Inventory inv = new Inventory("Test", 10, 5); 

        DrawInventoryCallback.Create(w, inv, Vector2.Zero, 100, 200, Entity: inventoryEntity);

        w.Update(); 

        LinearLayout ll = w.GetComponent<LinearLayout>(inventoryEntity); 
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
    public void InventoryUISystem_CreatedRowWidthShouldBeWidthMinusTwoTimesPadding() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int inventoryEntity = EntityFactory.Add(w); 
        Inventory inv = new Inventory("Test", 10, 5); 
        
        float inventoryWidth = 100f; 
        float inventoryPadding = 5f; 

        DrawInventoryCallback.Create(w, inv, Vector2.Zero, inventoryWidth, 200, Entity: inventoryEntity, Padding: inventoryPadding);

        w.Update(); 

        LinearLayout ll = w.GetComponent<LinearLayout>(inventoryEntity); 
        bool allRowsHaveCorrectWidth = true; 

        float correctWidth = inventoryWidth - (2 * inventoryPadding); 
        foreach (int c in ll.GetChildren()) {
            Frame rowFrame = w.GetComponent<Frame>(c);
             
            if (rowFrame.GetWidth() != correctWidth) {
                allRowsHaveCorrectWidth = false;
            }
        }

        Assert.True(allRowsHaveCorrectWidth); 
    }

    [Fact] 
    public void InventoryUISystem_CreatedRowHeightShouldBeCorrect() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int inventoryEntity = EntityFactory.Add(w); 
        Inventory inv = new Inventory("Test", 10, 5); 
        
        float inventoryHeight = 200f; 
        float inventoryPadding = 5f; 

        //the total available height is height - (padding * (numRows + 1))
        //so each row should be availableHeight/numRows
        float correctRowHeight = (inventoryHeight - (inventoryPadding * (inv.GetRows() + 1))) / inv.GetRows(); 
        float inventoryWidth = 100f; 

        DrawInventoryCallback.Create(w, inv, Vector2.Zero, inventoryWidth, 
            inventoryHeight, Entity: inventoryEntity, Padding: inventoryPadding);
        w.Update(); 

        LinearLayout ll = w.GetComponent<LinearLayout>(inventoryEntity); 
        bool allRowsHaveCorrectHeight = true; 

        foreach (int c in ll.GetChildren()) {
            Frame rowFrame = w.GetComponent<Frame>(c);
             
            if (rowFrame.GetHeight() != correctRowHeight) { 
                allRowsHaveCorrectHeight = false;
            }
        }

        Assert.True(allRowsHaveCorrectHeight); 
    }

    [Fact]
    public void InventoryUISystem_CreatedCellsShouldHaveCorrectWidthAndHeight() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int inventoryEntity = EntityFactory.Add(w); 
        Inventory inv = new Inventory("Test", 10, 5); 
        
        float inventoryHeight = 300f; 
        float inventoryWidth = 600f; 
        float inventoryPadding = 15f; 

        float correctRowHeight = (inventoryHeight - (inventoryPadding * (inv.GetRows() + 1))) / inv.GetRows(); 
        float correctRowWidth = inventoryWidth - (2 * inventoryPadding); 

        float correctCellWidth = (correctRowWidth - (inventoryPadding * (inv.GetCols() + 1))) / inv.GetCols(); 
        float correctCellHeight = correctRowHeight - (2 * inventoryPadding); 

        DrawInventoryCallback.Create(w, inv, Vector2.Zero, inventoryWidth, inventoryHeight, 
            Entity: inventoryEntity, Padding: inventoryPadding);
        
        w.Update(); 

        bool allCellWidthCorrect = true; 
        bool allCellHeightCorrect = true; 

        foreach (int r in w.GetComponent<LinearLayout>(inventoryEntity).GetChildren()) {
            foreach (int cell in w.GetComponent<LinearLayout>(r).GetChildren()) {
                Frame cellFrame = w.GetComponent<Frame>(cell); 
                if (cellFrame.GetWidth() != correctCellWidth) {
                    allCellWidthCorrect = false; 
                }

                if (cellFrame.GetHeight() != correctCellHeight) {
                    allCellHeightCorrect = false; 
                }
            }
        }

        Assert.True(allCellHeightCorrect); 
        Assert.True(allCellWidthCorrect); 
    }

    
    [Fact]
    public void InventoryUISystem_CreatedFramesShouldHaveCorrectCoordinates() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int inventoryEntity = EntityFactory.Add(w); 
        Inventory inv = new Inventory("Test", 10, 5); 
        
        float inventoryHeight = 300f; 
        float inventoryWidth = 600f; 
        float inventoryPadding = 15f; 

        float rowHeight = (inventoryHeight - (inventoryPadding * (inv.GetRows() + 1))) / inv.GetRows(); 
        float rowWidth = inventoryWidth - (2 * inventoryPadding); 

        float cellWidth = (rowWidth - (inventoryPadding * (inv.GetCols() + 1))) / inv.GetCols(); 
        float cellHeight = rowHeight - (2 * inventoryPadding); 

        float invX = 25f; 
        float invY = 55f; 

        DrawInventoryCallback.Create(w, inv, new Vector2(invX, invY), inventoryWidth, 
            inventoryHeight, Entity: inventoryEntity, Padding: inventoryPadding);
        w.Update(); 

        LinearLayout ll = w.GetComponent<LinearLayout>(inventoryEntity); 
        List<int> rowEntities = ll.GetChildren(); 

        bool allRowXCorrect = true; 
        bool allRowYCorrect = true; 

        bool allCellXCorrect = true; 
        bool allCellYCorrect = true; 

        for (int i = 0; i < rowEntities.Count; i++) {
            Frame rowFrame = w.GetComponent<Frame>(rowEntities[i]); 
            if (rowFrame.GetX() != invX + inventoryPadding) {
                allRowXCorrect = false; 
            }
            if (rowFrame.GetY() != invY + (inventoryPadding * (i + 1)) + (rowHeight * i)) {
                allRowYCorrect = false;
            }
            List<int> cellEntities = w.GetComponent<LinearLayout>(rowEntities[i]).GetChildren(); 

            for (int j = 0; j < cellEntities.Count; j++) {
                Frame cellFrame = w.GetComponent<Frame>(cellEntities[j]); 

                if (cellFrame.GetX() != invX + (inventoryPadding * (j + 2)) + (cellWidth * j)) {
                    allCellXCorrect = false; 
                }

                if (cellFrame.GetY() != invY + (inventoryPadding * (i + 2)) + (rowHeight * i)) {
                    float expectedY = invY + (inventoryPadding * (i + 2)) + (rowHeight * i); 
                    allCellYCorrect = false; 
                }
            }
        }

        Assert.True(allRowXCorrect); 
        Assert.True(allRowYCorrect); 
        Assert.True(allCellXCorrect); 
        Assert.True(allCellYCorrect); 
    }   
    
    [Fact]
    public void InventoryUISystem_DrawShouldReturnAnEntityWithAnInventoryComponent() {
        World w = WorldFactory.Build(); 
        Inventory inv = new Inventory("Test", 2, 2); 
        int e = DrawInventoryCallback.Draw(w, inv, Vector2.Zero, 0f, 0f); 
        Assert.Equal(inv, w.GetComponent<Inventory>(e));
    }
    //TODO: Handle possibility of making it with padding too much to be drawn and it makes row widths negative
}
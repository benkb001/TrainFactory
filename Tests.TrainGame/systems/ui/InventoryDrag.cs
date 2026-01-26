
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

//sequential because global state (mouse)
[Collection("Sequential")]
public class InventoryDragSystemTest {

    private static int mousePos = 10; 

    private void RegisterDependencies(World w) {
        ButtonSystem.RegisterClick(w);
        ButtonSystem.RegisterHold(w); 

        DrawCallbackSystem.Register(w); 
        InventoryControlSystem.RegisterUpdate(w); 
        
        LinearLayoutSystem.Register(w); 

        DragSystem.Register(w); 
        InventoryDragSystem.Register(w); 
        //InventoryControlSystem.RegisterOrganize(w); 
        //not registering the above so i can test it generates the correct thing
        ButtonSystem.RegisterUnclick(w);
    }

    private (World, Inventory, Inventory, Inventory.Item, int, int) init() {
        VirtualMouse.Reset(); 
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterDependencies(w); 
        
        Inventory inv1 = new Inventory("Test1", 2, 2); 
        Inventory inv2 = new Inventory("Test2", 2, 2); 

        Inventory.Item apple = new Inventory.Item(ItemId: "Apple", Count: 2); 

        inv1.Add(apple, 1, 1); 

        InventoryView invView1 = DrawInventoryCallback.Draw(w, inv1, Vector2.Zero, 100, 100);
        InventoryView invView2 = DrawInventoryCallback.Draw(w, inv2, new Vector2(100, 0), 100, 100);

        int invEnt1 = invView1.GetInventoryEntity(); 
        int invEnt2 = invView2.GetInventoryEntity();

        return (w, inv1, inv2, apple, invEnt1, invEnt2);
    }
    
    [Fact]
    public void InventoryDragSystem_ShouldMakeAnOrganizeInventoryMessageWhenItemDroppedOverANewSlot() {
        (World w, Inventory inv1, Inventory inv2, Inventory.Item apple, int invEntity1, int invEntity2) = init();
        VirtualMouse.Reset(); 

        w.Update(); 

        VirtualMouse.SetCoordinates(mousePos, mousePos); 
        VirtualMouse.LeftPress(); 
        w.Update(); 

        VirtualMouse.SetCoordinates(105, 5); 
        VirtualMouse.LeftRelease(); 
        w.Update(); 

        Assert.Single(w.GetComponentArray<InventoryOrganizeMessage>());

        VirtualMouse.Reset(); 
    }

    [Fact]
    public void InventoryDragSystem_ShouldSpecifyCorrectParameters() {
        VirtualMouse.Reset(); 
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterDependencies(w); 
        
        Inventory inv1 = new Inventory("Test1", 2, 2); 
        Inventory inv2 = new Inventory("Test2", 2, 2); 

        string curItemId = "apple"; 
        string targetItemId = "banana"; 
        Inventory.Item curItem = new Inventory.Item(ItemId: curItemId, Count: 2); 
        Inventory.Item targetItem = new Inventory.Item(ItemId: targetItemId, Count: 1); 

        int curCol = 0; 
        int curRow = 0; 
        int targetRow = 0; 
        int targetCol = 0; 

        inv1.Add(curItem, curRow, curCol); 
        inv2.Add(targetItem, targetRow, targetCol); 

        InventoryView invView1 = DrawInventoryCallback.Draw(w, inv1, Vector2.Zero, 100, 100);
        InventoryView invView2 = DrawInventoryCallback.Draw(w, inv2, new Vector2(100, 0), 100, 100);

        w.Update(); 

        LinearLayout ll1 = w.GetComponent<LinearLayout>(invView1.GetInventoryEntity()); 
        LinearLayout row1 = w.GetComponent<LinearLayout>(ll1.GetChildren()[curRow]); 
        Draggable curDraggable = w.GetComponent<Draggable>(row1.GetChildren()[curCol]); 

        LinearLayout ll2 = w.GetComponent<LinearLayout>(invView2.GetInventoryEntity()); 
        LinearLayout row2 = w.GetComponent<LinearLayout>(ll2.GetChildren()[targetRow]); 
        Vector2 targetVector = w.GetComponent<Frame>(row2.GetChildren()[targetCol]).Position; 

        VirtualMouse.SetCoordinates(mousePos, mousePos); 
        VirtualMouse.LeftPress(); 
        w.Update(); 

        VirtualMouse.SetCoordinates(105, 5); 
        VirtualMouse.LeftRelease(); 
        w.Update(); 

        List<KeyValuePair<int, InventoryOrganizeMessage>> msgList = w.GetComponentArray<InventoryOrganizeMessage>().ToList(); 
        InventoryOrganizeMessage msg = msgList[0].Value; 

        Assert.Equal(curRow, msg.CurRow); 
        Assert.Equal(curCol, msg.CurColumn); 
        Assert.Equal(targetRow, msg.TargetRow); 
        Assert.Equal(targetCol, msg.TargetColumn); 
        Assert.Equal(curItem.ItemId, msg.CurItem.ItemId);
        Assert.Equal(targetItem.ItemId, msg.TargetItem.ItemId);
        Assert.Equal(curDraggable, msg.CurDraggable);
        Assert.Equal(targetVector, msg.TargetVector);
        VirtualMouse.Reset(); 
    }

    [Fact]
    public void InventoryDragSystem_ShouldNotMakeMessagesIfItemIsHeld() {
        (World w, Inventory inv1, Inventory inv2, Inventory.Item apple, int invEntity1, int invEntity2) = init();

        w.Update(); 

        VirtualMouse.SetCoordinates(mousePos, mousePos); 
        VirtualMouse.LeftPress(); 
        w.Update(); 

        VirtualMouse.SetCoordinates(105, 5); 
        w.Update(); 

        Assert.Empty(w.GetComponentArray<InventoryOrganizeMessage>());

        VirtualMouse.Reset(); 
    }

    [Fact]
    public void InventoryDragSystem_ShouldNotMakeMessagesIfItemIsDroppedFurtherThanThreshold() {
        (World w, Inventory inv1, Inventory inv2, Inventory.Item apple, int invEntity1, int invEntity2) = init();

        w.Update(); 

        VirtualMouse.SetCoordinates(mousePos, mousePos); 
        VirtualMouse.LeftPress(); 
        w.Update(); 

        VirtualMouse.SetCoordinates(105, 0 - (int)InventoryDragSystem.Threshold); 
        VirtualMouse.LeftRelease(); 
        w.Update(); 

        Assert.Empty(w.GetComponentArray<InventoryOrganizeMessage>());

        VirtualMouse.Reset(); 
    }
}
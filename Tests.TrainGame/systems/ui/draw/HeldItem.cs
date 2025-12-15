using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Callbacks; 

[Collection("Sequential")]
public class HeldItemDrawSystemTest() {
    private (World, Inventory, int, int) init() {
        World w = WorldFactory.Build(); 
        Inventory inv = new Inventory("Player", 1, 10); 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 2), 0, 1);
        int invEntity = EntityFactory.Add(w); 

        DrawInventoryCallback.Create(w, inv, Vector2.Zero, 800, 80, Entity: invEntity);

        int playerEntity = EntityFactory.Add(w);
        w.SetComponent<Frame>(playerEntity, new Frame(150, 150, 100, 100)); 
        w.SetComponent<HeldItem>(playerEntity, new HeldItem(inv, invEntity)); 

        return (w, inv, invEntity, playerEntity); 
    }

    [Fact]
    public void HeldItemDrawSystem_ShouldNotDrawWhenNoItemIsHeld() {
        VirtualMouse.Reset();
        (World w, Inventory inv, int invEntity, int playerEntity) = init(); 
        
        //index defaults to zero, no item in zero index in inventory, see above
        w.Update(); 
        Assert.False(w.EntityExists(w.GetComponent<HeldItem>(playerEntity).LabelEntity));

        VirtualMouse.Reset(); 
    }

    [Fact]
    public void HeldItemDrawSystem_ShouldSetTextToCorrectItemID() {
        VirtualMouse.Reset();

        (World w, Inventory inv, int invEntity, int playerEntity) = init(); 
        w.Update(); 

        VirtualMouse.ScrollUp(); 
        w.Update(); 
        int labelEntity = w.GetComponent<HeldItem>(playerEntity).LabelEntity; 
        Assert.Equal("Apple", w.GetComponent<TextBox>(labelEntity).Text);

        VirtualMouse.Reset(); 
    }

    [Fact]
    public void HeldItemDrawSystem_ShouldHighlightOnlyTheHeldCellInInventory() {
        VirtualMouse.Reset();
        (World w, Inventory inv, int invEntity, int playerEntity) = init(); 

        w.Update(); 
        LinearLayout llRow = w.GetComponent<LinearLayout>(invEntity); 
        LinearLayout ll = w.GetComponent<LinearLayout>(llRow.GetChildren()[0]);
        List<int> children = ll.GetChildren(); 

        Assert.Equal(0, w.GetComponent<HeldItem>(playerEntity).InvIndex); 

        bool valid = true; 
        if (w.GetComponent<Outline>(children[0]).GetColor() != Colors.InventoryHeld) {
            valid = false; 
        }

        for (int i = 1; i < children.Count; i++) {
            if (w.GetComponent<Outline>(children[i]).GetColor() != Colors.InventoryNotHeld) {
                valid = false; 
            }
        }

        Assert.True(valid);
        VirtualMouse.Reset(); 
    }
}
using TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Callbacks; 

public class PurchaseClickSystemTest {
    private static string productID = ItemID.Armor1; 
    private static Dictionary<string, int> cost = new() {
        [ItemID.Iron] = 10, 
        [ItemID.Wood] = 5
    }; 

    private static (World, int, Inventory) init() {
        World w = WorldFactory.Build(); 
        int btnEnt = EntityFactory.AddUI(w, Vector2.Zero, 100, 100); 
        w.SetComponent<Button>(btnEnt, new Button(true)); 
        Inventory inv = new Inventory("Test", 2, 2); 
        
        PurchaseButton pb = new PurchaseButton(productID, cost, inv);
        w.SetComponent<PurchaseButton>(btnEnt, pb); 
        return (w, btnEnt, inv); 
    }

    [Fact]
    public void PurchaseClick_ShouldDoNothingIfInvDoesNotHaveRequiredItems() {
        (World w, int btnEnt, Inventory inv) = init(); 
        w.Update(); 
        Assert.Equal(0, inv.ItemCount(productID));
    }

    [Fact]
    public void PurchaseClick_ShouldAddProductToInvIfItHasRequiredItems() {
        (World w, int btnEnt, Inventory inv) = init(); 
        inv.Add(cost); 
        w.Update(); 
        Assert.Equal(1, inv.ItemCount(productID)); 
    }
}

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class CartAssemblerTest {
    [Fact]
    public void CartAssembler_ShouldAddCartToCity() {
        World w = WorldFactory.Build(); 

        Inventory inv = new Inventory("Test", 2, 2); 
        Machine m = new Machine(inv, new Dictionary<string, int>(), "", 0, minTicks: 1, produceInfinite: true); 
        City c = new City("Test", inv); 

        CartAssembler asm = new CartAssembler(c, m);
        asm.Assemble(); 
        Assert.Single(c.Carts); 
    }

    [Fact]
    public void CartAssembler_InventoriesShouldBeAbleToBeAddedTo() {
        World w = WorldFactory.Build(); 

        Inventory inv = new Inventory("Test", 2, 2); 
        Machine m = new Machine(inv, new Dictionary<string, int>(), "", 0, minTicks: 1, produceInfinite: true); 
        City c = new City("Test", inv); 

        CartAssembler asm = new CartAssembler(c, m);
        asm.Assemble(); 

        foreach (KeyValuePair<string, Cart> kvp in c.Carts) {
            Cart cart = kvp.Value; 
            Inventory cartInv = cart.Inv; 
            Inventory.Item item = new Inventory.Item(ItemId: ItemID.Wood, Count: 1); 
            Assert.Equal(1, cartInv.Add(item, 1, 1));
        }
    }
}
namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Constants;
using TrainGame.Systems;

public class Cart {
    public readonly CartType Type; 
    public static CartType[] AllTypes = [CartType.Freight, CartType.Liquid]; 

    public Cart(CartType type) {
        this.Type = type; 
    }
}

public class CartWrap {
    public static Dictionary<CartType, Inventory> GetTestInventories() {
        return new(){
            [CartType.Freight] = InventoryWrap.GetTest(),
            [CartType.Liquid] = InventoryWrap.GetTest()
        };
    }
}
namespace TrainGame.Utils; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.ECS;
using TrainGame.Constants; 
using TrainGame.Callbacks; 
using TrainGame.Systems;

public static class Player {
    private static Inventory inv; 

    public static void AddToInv(string itemID, int count) {
        inv.Add(itemID, count); 
    }

    public static void SetInventory(Inventory i) {
        inv = i; 
    }

    public static Inventory GetInventory() {
        return inv; 
    }
}
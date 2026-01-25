using TrainGame.Systems; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public class LootSystemTest {
    [Fact]
    public void LootSystem_ShouldAddItemsToInventoryOnDeath() {
        World w = WorldFactory.Build(); 

        int e = EntityFactory.AddUI(w, Vector2.Zero, 10, 10); 
        w.SetComponent<Health>(e, new Health(0)); 
        Inventory inv = InventoryWrap.GetDefault(); 
        w.SetComponent<Loot>(e, new Loot(ItemID.Iron, 10, inv));

        w.Update(); 

        Assert.Equal(10, inv.ItemCount(ItemID.Iron)); 
    }
}
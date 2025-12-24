
using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Systems; 

public class UpgradeTrainClickSystemTest {

    private (World, Inventory, City, Train, UpgradeTrainButton, int) init() {
        World w = WorldFactory.Build(); 

        Inventory inv = new Inventory(Constants.PlayerInvID, 2, 2);
        City c = new City("Test", inv); 
        Train t = new Train(inv, c); 
        UpgradeTrainButton btn = new UpgradeTrainButton(t, inv);

        int e = EntityFactory.Add(w); 
        w.SetComponent<Button>(e, new Button(true)); 
        w.SetComponent<UpgradeTrainButton>(e, btn); 

        return (w, inv, c, t, btn, e); 
    }

    [Fact]
    public void UpgradeTrainClickSystem_ShouldNotUpgradeIfInventoryDoesNotHaveTrainUpgrade() {
        (World w, Inventory inv, City c, Train t, UpgradeTrainButton btn, int e) = init(); 
        float mph = t.MilesPerHour; 
        w.Update(); 
        Assert.Equal(mph, t.MilesPerHour); 
    }

    [Fact]
    public void UpgradeTrainClickSystem_ShouldIncreaseMPHWhenClickedIfInvHasTrainUpgrade() {
        (World w, Inventory inv, City c, Train t, UpgradeTrainButton btn, int e) = init(); 
        float mph = t.MilesPerHour; 

        inv.Add(new Inventory.Item(ItemId: ItemID.TrainUpgrade, Count: 1)); 
        w.Update(); 
        Assert.True(t.MilesPerHour > mph); 
    }

    [Fact]
    public void UpgradeTrainClickSystem_ShouldConsumeATrainUpgradeWhenClicked() {
        (World w, Inventory inv, City c, Train t, UpgradeTrainButton btn, int e) = init(); 
        float mph = t.MilesPerHour; 

        inv.Add(new Inventory.Item(ItemId: ItemID.TrainUpgrade, Count: 1)); 
        w.Update(); 
        Assert.Equal(0, inv.ItemCount(ItemID.TrainUpgrade)); 
    }
}
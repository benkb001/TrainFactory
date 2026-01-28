using TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants;
using TrainGame.Callbacks; 

public class UpgradeDepotClickSystemTest {
    [Fact]
    public void UpgradeDepotClickSystem_ShouldUpgradeCityInvIfCityHasADepotUpgrade() {
        World w = WorldFactory.Build(); 
        int e = EntityFactory.Add(w); 
        w.SetComponent<Button>(e, new Button(true)); 
        City city = CityWrap.GetTest(); 
        city.Inv.Add(ItemID.DepotUpgrade, 1); 
        w.SetComponent<UpgradeDepotButton>(e, new UpgradeDepotButton(city)); 
        int prevLevel = city.Inv.Level; 
        w.Update(); 
        Assert.Equal(prevLevel + 1, city.Inv.Level); 
    }
}
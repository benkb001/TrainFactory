namespace TrainGame.Systems; 

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

public class UpgradeDepotButton {
    private City city; 
    public City GetCity() => city; 

    public UpgradeDepotButton(City city) {
        this.city = city; 
    }
}

public static class UpgradeDepotClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<UpgradeDepotButton>(w, (w, e, btn) => {
            City city = btn.GetCity(); 
            if (city.Inv.Take(ItemID.DepotUpgrade, 1).Count == 1) {
                city.Inv.Upgrade(); 
            }
        }); 
    }
}
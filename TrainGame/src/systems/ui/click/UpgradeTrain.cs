namespace TrainGame.Systems; 

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

public class UpgradeTrainClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<UpgradeTrainButton>(w, (w, e) => {
            UpgradeTrainButton btn = w.GetComponent<UpgradeTrainButton>(e); 
            Inventory pInv = btn.PlayerInv; 
            Train t = btn.UpgradingTrain; 

            if (pInv.Take(ItemID.TrainUpgrade, 1).Count == 1) {
                t.UpgradePower(Constants.UpgradePowerStep); 
            }

            MakeMessage.Add<DrawTrainInterfaceMessage>(w, new DrawTrainInterfaceMessage(t)); 
        }); 
    }
}
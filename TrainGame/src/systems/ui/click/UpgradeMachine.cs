namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Constants; 
using TrainGame.Utils; 

public static class UpgradeMachineClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<UpgradeMachineButton>(w, (w, e) => {
            Machine m = w.GetComponent<UpgradeMachineButton>(e).GetMachine(); 
            if (m.Inv.ItemCount(m.UpgradeItemID) >= 1) {
                m.Inv.Take(m.UpgradeItemID, 1); 
                m.Upgrade(1); 

                MakeMessage.Add<DrawMachineInterfaceMessage>(w, new DrawMachineInterfaceMessage(m)); 
            }
        }); 
    }
}
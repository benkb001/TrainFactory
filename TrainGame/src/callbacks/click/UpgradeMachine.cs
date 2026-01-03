namespace TrainGame.Callbacks; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.Constants; 
using TrainGame.ECS; 
using TrainGame.Utils;
using System.Reflection;

public class UpgradeMachineOnClick {
    public static Action Create(World w, Machine m) {
        return () => {
            if (m.Inv.ItemCount(m.UpgradeItemID) >= 1) {
                m.Inv.Take(m.UpgradeItemID, 1); 
                m.Upgrade(1); 

                PopFactory.Build(w, scene: 0, late: true, delay: 1); 
                MakeMessage.Add<DrawMachineInterfaceMessage>(w, new DrawMachineInterfaceMessage(m, playerAtMachine: true)); 
                
            }
        }; 
    }
}
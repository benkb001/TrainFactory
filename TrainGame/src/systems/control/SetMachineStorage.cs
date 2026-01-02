namespace TrainGame.Systems;

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils;
using TrainGame.Constants;

public static class SetMachineStorageSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(MachineStorageStepper), typeof(Button), typeof(Active)], (w, e) => {
            if (w.GetComponent<Button>(e).Clicked) {
                MachineStorageStepper storageStep = w.GetComponent<MachineStorageStepper>(e); 
                Stepper step = storageStep.GetStepper(); 
                Machine m = storageStep.GetMachine(); 
                int storageSize = step.Value;
                m.SetStorageSize(storageSize); 
            }
        });
    }
}


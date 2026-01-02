namespace TrainGame.Components; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.ECS;
using TrainGame.Constants; 
using TrainGame.Callbacks; 

public class DrawMachineInterfaceMessage {
    private bool playerAtMachine; 
    private Machine machine; 

    public bool PlayerAtMachine => playerAtMachine; 

    public Machine GetMachine() {
        return machine; 
    }

    public DrawMachineInterfaceMessage(Machine machine, bool playerAtMachine = false) {
        this.machine = machine; 
        this.playerAtMachine = playerAtMachine; 
    }
}
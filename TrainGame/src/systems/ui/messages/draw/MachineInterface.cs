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
    private Machine machine; 
    private City city; 

    public Machine GetMachine() {
        return machine; 
    }

    public City GetCity() => city; 

    public DrawMachineInterfaceMessage(Machine machine, City city = null) {
        this.machine = machine; 
        this.city = city; 
    }
}
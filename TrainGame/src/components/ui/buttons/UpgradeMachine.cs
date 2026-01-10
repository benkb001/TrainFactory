namespace TrainGame.Components; 

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

public class UpgradeMachineButton {
    private Machine machine; 
    public Machine GetMachine() => machine; 

    public UpgradeMachineButton(Machine m) {
        this.machine = m; 
    }
}
namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

//needs to be clickSystem -> push -> drawSystem
public class MachineUIClickSystem() {
    public static void Register(World world) {
        Type[] ts = [typeof(MachineUI), typeof(Button), typeof(Frame), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            if (w.GetComponent<Button>(e).Clicked) {
                Machine m = w.GetComponent<MachineUI>(e).GetMachine();
                MakeMessage.Add<DrawMachineInterfaceMessage>(w, new DrawMachineInterfaceMessage(m)); 
            }
        }; 
        world.AddSystem(ts, tf); 
    }
}
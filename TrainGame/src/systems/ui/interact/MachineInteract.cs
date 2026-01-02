namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

//TODO: Test
public class MachineInteractSystem() {
    private static Type[] ts = [typeof(MachineUI), typeof(Interactable), typeof(Active)]; 
    private static Action<World, int> tf = (w, e) => {
        Interactable mInteractable = w.GetComponent<Interactable>(e); 
        if (mInteractable.Interacted) {
            View.EnterMenu(w); 
            Machine m = w.GetComponent<MachineUI>(e).GetMachine(); 
            MakeMessage.Add<DrawMachineInterfaceMessage>(w, new DrawMachineInterfaceMessage(m, playerAtMachine: true)); 
            PushFactory.Build(w); 
        }
    }; 

    public static void Register(World world) {
        world.AddSystem(ts, tf); 
    }
}
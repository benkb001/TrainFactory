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
                View.EnterMenu(w); 
                Machine m = w.GetComponent<MachineUI>(e).GetMachine(); 
                DrawMachineRequestMessage dm = new DrawMachineRequestMessage(
                    machine: m, 
                    Width: w.ScreenWidth / 5f, 
                    Height: w.ScreenWidth / 2f, 
                    Position: w.GetCameraTopLeft() + new Vector2(w.ScreenWidth / 5f, 10f),
                    Margin: 5f, 
                    SetMenu: true
                ); 
                int dmEntity = EntityFactory.Add(w); 
                w.SetComponent<DrawMachineRequestMessage>(dmEntity, dm); 

                int pushEntity = EntityFactory.Add(w);
                w.SetComponent<PushSceneMessage>(pushEntity, PushSceneMessage.Get()); 
            }
        }; 
        world.AddSystem(ts, tf); 
    }
}
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

//needs to be clickCity -> push -> drawMachine
public class DrawMachinesViewSystem() {
    public static void Register(World world) {

        Type[] ts = [typeof(DrawMachinesViewMessage)]; 
        Action<World, int> tf = (w, e) => {
            DrawMachinesViewMessage dm = w.GetComponent<DrawMachinesViewMessage>(e); 
            List<Machine> machines = dm.Machines; 
            int machinesViewEntity = EntityFactory.Add(w); 
            LinearLayout mvLL = new LinearLayout("Vertical", "alignLow"); 
            mvLL.Padding = dm.Padding; 
            w.SetComponent<LinearLayout>(machinesViewEntity, mvLL); 

            w.SetComponent<Frame>(machinesViewEntity, new Frame(dm.Position, dm.Width, dm.Height));
            w.SetComponent<Outline>(machinesViewEntity, new Outline()); 

            foreach (Machine machine in machines) {
                int mEntity = EntityFactory.Add(w); 

                w.SetComponent<MachineUI>(mEntity, new MachineUI(machine)); 
                w.SetComponent<Outline>(mEntity, new Outline()); 
                w.SetComponent<TextBox>(mEntity, new TextBox(machine.Id)); 
                w.SetComponent<Button>(mEntity, new Button()); 
                mvLL.AddChild(mEntity); 
            }
            LinearLayoutWrap.ResizeChildren(machinesViewEntity, w); 
            w.RemoveEntity(e); 
        }; 

        world.AddSystem(ts, tf); 
    }
}
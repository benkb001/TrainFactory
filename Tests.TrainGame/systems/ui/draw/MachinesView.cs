
using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class DrawMachinesViewSystemTest {
    [Fact] 
    public void DrawMachinesViewSystem_ShouldMakeAMachineUIForEachMachineInDrawMessage() {
        World w = new World(); 
        RegisterComponents.All(w); 
        DrawMachinesViewSystem.Register(w); 

        Machine m1 = new Machine(null, null, "", 0, 0);
        Machine m2 = new Machine(null, null, "", 0, 0);

        List<Machine> ms = [m1, m2];
        DrawMachinesViewMessage dm = new DrawMachinesViewMessage(ms, 0f, 0f, Vector2.Zero);
        int dmEntity = EntityFactory.Add(w); 
        w.SetComponent<DrawMachinesViewMessage>(dmEntity, dm); 
        w.Update(); 

        LinearLayout ll = w.GetComponentArray<LinearLayout>().First().Value; 
        Assert.Equal(2, ll.ChildCount);
        Assert.Equal(2, w.GetMatchingEntities([typeof(MachineUI)]).Count); 
    }
}
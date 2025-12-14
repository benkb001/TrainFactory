using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

//sequential because global state (mouse)
[Collection("Sequential")]
public class MachineRequestClickSystemTest {
    [Fact]
    public void MachineRequestClickSystem_ShouldRequestNumberPlayerInput() {
        VirtualMouse.Reset(); 
        World w = WorldFactory.Build(); 
        Inventory inv = new Inventory("Test", 2, 2);
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 2));
        inv.Add(new Inventory.Item(ItemId: "Orange", Count: 2)); 

        Dictionary<string, int> recipe = new() {
            ["Apple"] = 1, 
            ["Orange"] = 1
        }; 

        int msg = EntityFactory.Add(w);  

        Machine m = new Machine(inv, recipe, "Smoothie", 1, 60, "Blender"); 
        int machineEntity = EntityFactory.Add(w, setData: true); 
        w.SetComponent<Machine>(machineEntity, m); 
        
        int req_msg = EntityFactory.Add(w); 
        w.SetComponent(req_msg, new DrawMachineRequestMessage(m, 100, 350, Vector2.Zero, 5f)); 

        w.Update(); 

        int requestEntity = w.GetMatchingEntities([typeof(MachineRequestButton)])[0]; 
        MachineRequestButton rb = w.GetComponent<MachineRequestButton>(requestEntity); 
        int stepperEntity = rb.GetStepperEntity(); 
        Stepper stepper = w.GetComponent<Stepper>(stepperEntity); 
        stepper.Value = 15; 
        Frame requestFrame = w.GetComponent<Frame>(requestEntity); 
        VirtualMouse.LeftClick(requestFrame.Position + new Vector2(1, 1)); 

        w.Update(); 
        Assert.Equal(15, m.RequestedAmount); 

        VirtualMouse.Reset(); 
    }


}
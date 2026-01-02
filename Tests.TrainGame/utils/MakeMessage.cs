using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.ECS;
using TrainGame.Constants; 
using TrainGame.Callbacks; 
using TrainGame.Utils; 

public class MakeMessageTest {
    [Fact]
    public void MakeMessage_DrawInventory_ShouldDrawAnInventory() {
        World w = WorldFactory.Build(); 
        Inventory inv = new Inventory("Test", 2, 2); 
        
        MakeMessage.DrawInventory(inv, w, Vector2.Zero, 100f, 100f); 
        w.Update(); 

        Assert.Single(w.GetMatchingEntities([typeof(Inventory), typeof(LinearLayout), typeof(Frame), typeof(Active)])); 
    }

    [Fact]
    public void MakeMessage_DrawMachineInterface_ShouldMakeProgressBarAndInventoryAndRequestStepper() {
        World w = WorldFactory.Build(); 
        Inventory inv = new Inventory("Test", 2, 2); 
        Dictionary<string, int> recipe = new() {
            ["Apple"] = 2
        }; 

        Machine m = new Machine(inv, recipe, "Smoothie", 2, 10); 
        MakeMessage.Add<DrawMachineInterfaceMessage>(w, new DrawMachineInterfaceMessage(m)); 

        w.Update(); 
        
        Assert.Single(w.GetMatchingEntities([typeof(Stepper)])); 
        Assert.Single(w.GetMatchingEntities([typeof(Inventory)])); 
        Assert.Single(w.GetMatchingEntities([typeof(ProgressBar)])); 
    }
}
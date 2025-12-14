
using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

public class MachineInteractSystemTest {
    [Fact]
    public void MachineInteractSystem_ShouldOpenMachineInterfaceWhenInteractedWith() {
        World w = WorldFactory.Build(); 

        Inventory inv = new Inventory("Test", 1, 1); 
        Machine m = new Machine(inv, new Dictionary<string, int>(), "Test", 1, 1); 
        int e = EntityFactory.Add(w); 
        w.SetComponent<MachineUI>(e, new MachineUI(m)); 
        w.SetComponent<Interactable>(e, new Interactable(true)); 
        w.Update(); 
        Assert.Single(w.GetMatchingEntities([typeof(ProgressBar)])); 
        Assert.True(w.GetMatchingEntities([typeof(TextBox)]).Count >= 2); 
    }
}
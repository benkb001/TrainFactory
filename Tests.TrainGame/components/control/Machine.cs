using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 

public class MachineTest {
    [Fact]
    public void Machine_CraftTimeFunctionShouldCorrectlyDecreaseWithLevel() {
        Inventory inv = new Inventory("Test", 2, 2); 
        Machine m = new Machine(inv, new Dictionary<string, int>(), "Smoothie", 1, minTicks: 10, slowFactor: 100, startFactor: 1); 
        Assert.Equal(110, m.CraftTicks);
        m.Upgrade(9);
        Assert.Equal(20, m.CraftTicks);  
    }
}

using System.Collections.Generic;
using System; 
using System.Drawing; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class DrawTrainInterfaceSystemTest {
    [Fact]
    public void DrawTrainInterfaceSystem_ShouldDrawEmbarkButtonsForEachCity() {
        World w = WorldFactory.Build(); 
        Inventory inv = new Inventory("Test", 2, 2); 
        City c1 = new City("C1", inv); 
        City c2 = new City("C2", inv); 
        City c3 = new City("C3", inv); 
        Train t = new Train(inv, c1); 

        c1.AddConnection(c2); 
        c1.AddConnection(c3); 

        MakeMessage.Add<DrawTrainInterfaceMessage>(w, new DrawTrainInterfaceMessage(t)); 
        w.Update(); 

        Assert.Equal(2, w.GetMatchingEntities([typeof(EmbarkButton), typeof(Button)]).Count); 
    }   
}
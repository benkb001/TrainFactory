

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Systems; 

public class DrawEmbarkSystemTest {
    [Fact]
    public void DrawEmbarkSystem_ShouldMakeALinearLayoutWithAnEmbarkButtonForEachAdjacentCity() {

        World w = WorldFactory.Build(); 

        Inventory inv = new Inventory("Test", 1, 1); 
        City c = new City("Test", inv, 100f, 150f);

        City c1 = new City("C1", inv, 0f, 0f); 
        City c2 = new City("C2", inv , 0f, 0f);

        Train t = new Train(inv, c); 

        c.AddConnections([c1, c2]); 

        int msg = EntityFactory.Add(w); 
        w.SetComponent<DrawEmbarkMessage>(msg, new DrawEmbarkMessage(
            t, 
            Vector2.Zero
        )); 
        w.Update(); 
        LinearLayout ll = w.GetComponent<LinearLayout>(w.GetMatchingEntities([typeof(LinearLayout)])[0]); 
        List<EmbarkButton> btnList = ll.GetChildren().Select(
            e => w.GetComponent<EmbarkButton>(e)).ToList(); 
        
        Assert.Single(btnList, btn => c1 == btn.GetDestination());
        Assert.Single(btnList, btn => c2 == btn.GetDestination()); 
        Assert.Equal(2, btnList.Count); 
    }
}
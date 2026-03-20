

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

        Train t = TrainWrap.GetTest();
        int trainEntity = EntityFactory.AddData<Train>(w, t);

        w.SetComponent<ComingFromCity>(trainEntity, new ComingFromCity(c));
        c.AddConnections([c1, c2]); 

        int msg = EntityFactory.Add(w); 
        w.SetComponent<DrawEmbarkMessage>(msg, new DrawEmbarkMessage(
            t, 
            trainEntity,
            c,
            Vector2.Zero
        )); 
        w.Update(); 
        LinearLayout ll = w.GetComponent<LinearLayout>(w.GetMatchingEntities([typeof(LinearLayout)])[0]); 
        List<int> es = ll.GetChildren(); 
        
        Assert.Single(es, e => {
            if (w.ComponentContainsEntity<EmbarkButton>(e)) {
                return (w.GetComponent<EmbarkButton>(e).GetDestination() == c1);
            }
            return false; 
        });

        Assert.Single(es, e => {
            if (w.ComponentContainsEntity<EmbarkButton>(e)) {
                return (w.GetComponent<EmbarkButton>(e).GetDestination() == c2);
            }
            return false; 
        }); 
    }
}
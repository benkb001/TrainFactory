
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

public class DrawTrainsViewSystemTest {
    [Fact] 
    public void DrawTrainsViewSystem_ShouldMakeATrainUIForEachTrainInDrawMessage() {
        World w = new World(); 
        RegisterComponents.All(w); 
        DrawTrainsViewSystem.Register(w); 

        City c = new City("", null);
        Train t1 = new Train(null, c);
        Train t2 = new Train(null, c);

        List<Train> ts = [t1, t2];
        DrawTrainsViewMessage dm = new DrawTrainsViewMessage(ts, 0f, 0f, Vector2.Zero);
        int dmEntity = EntityFactory.Add(w); 
        w.SetComponent<DrawTrainsViewMessage>(dmEntity, dm); 
        w.Update(); 

        LinearLayout ll = w.GetComponentArray<LinearLayout>().First().Value;
        Assert.Equal(2, ll.ChildCount);
        Assert.Equal(2, w.GetMatchingEntities([typeof(TrainUI)]).Count); 
    }
}

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.Components; 
using TrainGame.ECS; 

public class CraftProgressBarUpdateSystemTest {
    [Fact]
    public void CraftProgressBarUpdateSystem_ShouldSetCompletionToCraftCompletion() {
        World w = WorldFactory.Build(); 

        Inventory inv = new Inventory("Test", 2, 2); 
        Dictionary<string, int> recipe = new(); 
        Machine m = new Machine(inv, recipe, "smoothie", 1, 10, produceInfinite: true);
        ProgressBar pb = new ProgressBar(MaxWidth: 100f); 
        Backgrounds bgs = new Backgrounds(); 
        bgs.Add(new Background(Color.White), new Frame(0, 0, 10, 10));
        bgs.Add(new Background(Color.White), new Frame(0, 0, 10, 10));

        int e = EntityFactory.Add(w); 

        w.SetComponent<Machine>(e, m);
        w.SetComponent<ProgressBar>(e, pb); 
        w.SetComponent<Backgrounds>(e, bgs);
        w.SetComponent<Data>(e, Data.Get());  

        //takes ten ticks to craft so should be about 10%
        w.Update(); 
        Assert.Equal(0.1f, pb.Completion); 
        Assert.Equal(m.Completion, pb.Completion); 
    }
}
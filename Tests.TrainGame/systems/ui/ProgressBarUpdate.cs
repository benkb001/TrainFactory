
using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Constants;

public class ProgressBarUpdateSystemTest {
    [Fact]
    public void ProgressBarUpdateSystem_ShouldSetFrameWidthProportionalToCompletion() {
        World w = WorldFactory.Build(); 

        ProgressBar pb = new ProgressBar(MaxWidth: 100f); 
        Backgrounds bgs = new Backgrounds(); 
        bgs.Add(new Background(Colors.Placebo), new Frame(0, 0, 100, 10));
        bgs.Add(new Background(Colors.UIAccent), new Frame(0, 0, 0, 0));

        int e = EntityFactory.Add(w); 

        w.SetComponent<ProgressBar>(e, pb); 
        w.SetComponent<Backgrounds>(e, bgs);

        pb.Completion = 0.1f; 
        w.Update(); 
        (Background _, Frame f) = w.GetComponent<Backgrounds>(e).Ls[1]; 
        Assert.Equal(10f, f.GetWidth()); 
    }
}
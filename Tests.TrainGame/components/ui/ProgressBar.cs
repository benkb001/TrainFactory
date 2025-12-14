
using TrainGame.Components; 

public class ProgressBarTest {
    [Fact]
    public void ProgressBar_ShouldRespectConstructors() {
        ProgressBar pb = new ProgressBar(MaxWidth: 105f, Completion: 1f);
        Assert.Equal(105f, pb.MaxWidth); 
        Assert.Equal(1f, pb.Completion);  
    }
}
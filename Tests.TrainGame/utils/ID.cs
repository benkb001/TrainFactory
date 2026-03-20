using TrainGame.Utils; 
using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Callbacks; 
using TrainGame.Constants; 

public class IDTest {
    [Fact]
    public void ID_ShouldNotReturnUsedIDs() {
        string id1 = ID.GetNext("test"); 
        string id2 = ID.GetNext("test"); 
        Assert.NotEqual(id1, id2);
    }
}
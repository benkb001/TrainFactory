using TrainGame.Utils; 

public class IDTest {
    [Fact]
    public void ID_ShouldNotReturnUsedIDs() {
        string id1 = ID.GetNext("test"); 
        string id2 = ID.GetNext("test"); 
        Assert.NotEqual(id1, id2);
    }
}
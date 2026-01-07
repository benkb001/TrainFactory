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

    [Fact]
    public void ID_GetComponentShouldReturnComponentWithAssociatedID() {
        World w = WorldFactory.Build(); 
        Inventory inv = new Inventory("Test", 2, 2); 
        City c = CityWrap.GetTest(); 
        Train t = new Train(inv, c, Id: "T0"); 
        int e = EntityFactory.Add(w, setData: true); 
        w.SetComponent<Train>(e, t); 
        Assert.Equal(t, ID.GetComponent<Train>("T0", w)); 
    }

    [Fact]
    public void ID_GetComponentShouldThrowWhenUnassociatedIDIsPassed() {
        World w = WorldFactory.Build(); 
        Assert.Throws<InvalidOperationException>(() => ID.GetComponent<Train>("Bad", w));
    }
}
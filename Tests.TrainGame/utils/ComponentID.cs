using TrainGame.Utils; 
using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Callbacks; 
using TrainGame.Constants; 

public class ComponentIDTest {
    [Fact]
    public void ID_GetComponentShouldReturnComponentWithAssociatedID() {
        World w = WorldFactory.Build(); 
        Train t = TrainWrap.GetTest();
        int e = EntityFactory.AddData<Train>(w, t);
        Assert.Equal(t, ComponentID.GetComponent<Train>(t.ID, w)); 
    }

    [Fact]
    public void ID_GetComponentShouldThrowWhenUnassociatedIDIsPassed() {
        World w = WorldFactory.Build(); 
        Assert.Throws<InvalidOperationException>(() => ComponentID.GetComponent<Train>("Bad", w));
    }
}
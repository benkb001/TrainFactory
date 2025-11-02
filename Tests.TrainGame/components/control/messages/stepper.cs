using TrainGame.Components; 

public class StepperMessageTest {
    [Fact]
    public void StepperMessage_ShouldRespectConstructorArguments() {
        StepperMessage sm = new StepperMessage(10, 20); 
        Assert.Equal(10, sm.Entity);
        Assert.Equal(20, sm.Delta);
    }
}
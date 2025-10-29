namespace TrainGame.Components;

public class StepperTest {
    [Fact]
    public void Stepper_ShouldRespectConstructorArguments() {
        Stepper s = new Stepper(10); 
        Assert.Equal(10, s.Value);
    }
}


public class StepperMessageTest {
    [Fact]
    public void StepperMessage_ShouldRespectConstructorArguments() {
        StepperMessage sm = new StepperMessage(10, 20); 
        Assert.Equal(10, sm.Entity);
        Assert.Equal(20, sm.Delta);
    }
}
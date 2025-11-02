namespace TrainGame.Components;

public class StepperTest {
    [Fact]
    public void Stepper_ShouldRespectConstructorArguments() {
        Stepper s = new Stepper(10); 
        Assert.Equal(10, s.Value);
    }
}
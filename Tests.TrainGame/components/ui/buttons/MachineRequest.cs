
using TrainGame.Components; 

public class MachineRequestButtonTest {
    [Fact]
    public void MachineRequestButton_ShouldRespectConstructorArguments() {
        Machine m = new Machine(null, null, "", 0, 0);
        MachineRequestButton rb = new MachineRequestButton(m, 0); 
        Assert.Equal(m, rb.GetMachine()); 
        Assert.Equal(0, rb.GetStepperEntity());
    }
}
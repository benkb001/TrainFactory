using TrainGame.Components; 

public class MachineUITest {
    [Fact]
    public void MachineUI_ShouldRespectConstructors() {
        Machine m = new Machine(null, null, "", 0, 0);
        MachineUI mUI = new MachineUI(m); 
        Assert.Equal(m, mUI.GetMachine()); 
    }
}
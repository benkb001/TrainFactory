using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.ECS; 

public class MachineUIClickSystemTest {
    [Fact]
    public void MachineUIClickSystem_ShouldMakeADrawMessageWithItsMachine() {
        World w = new World(); 
        RegisterComponents.All(w); 
        MachineUIClickSystem.Register(w); 
        int e = EntityFactory.Add(w);

        w.SetComponent<Button>(e, new Button(true)); 
        Machine m = Machine.GetDefault(); 
        w.SetComponent<MachineUI>(e, new MachineUI(m)); 
        w.Update(); 

        DrawMachineInterfaceMessage dm = w.GetComponentArray<DrawMachineInterfaceMessage>().FirstOrDefault().Value; 
        Assert.Equal(m, dm.GetMachine()); 
    }
}
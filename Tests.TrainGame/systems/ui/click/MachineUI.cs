using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.ECS; 

public class MachineUIClickSystemTest {
    [Fact]
    public void MachineUIClickSystem_ShouldPushSceneAndMakeADrawMachineRequestMessageWhenClicked() {
        World w = new World(); 
        RegisterComponents.All(w); 
        MachineUIClickSystem.Register(w); 

        int e = EntityFactory.Add(w); 
        w.SetComponent<Frame>(e, new Frame(0, 0, 100, 100));
        w.SetComponent<Button>(e, new Button(true)); 
        w.SetComponent<MachineUI>(e, new MachineUI(null)); 
        
        w.Update(); 

        Assert.Single(w.GetComponentArray<PushSceneMessage>());
        Assert.Single(w.GetComponentArray<DrawMachineRequestMessage>());
    }
}
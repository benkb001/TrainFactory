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

        Inventory inv = new Inventory("Test", 2, 2);
        Dictionary<string, int> recipe = new() {
            ["Apple"] = 2, 
            ["Banana"] = 1
        }; 

        Machine m = new Machine(Inv: inv, recipe: recipe, productItemId: "Smoothie", productCount: 1, minTicks: 0);
        w.SetComponent<MachineUI>(e, new MachineUI(m));
        
        w.Update(); 

        Assert.Single(w.GetComponentArray<PushSceneMessage>());
        Assert.Single(w.GetComponentArray<DrawMachineRequestMessage>());
    }
}
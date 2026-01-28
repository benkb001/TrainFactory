using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.ECS; 

public class TestAssembler : IAssembler<Test> {
    private Machine m; 
    public int Assembled = 0; 

    public TestAssembler(Machine m) {
        this.m = m; 
    }

    public Test Assemble() {
        Assembled++; 
        return new Test(); 
    }

    public Machine GetMachine() {
        return m; 
    }
}

public class AssemblerTest {
    [Fact]
    public void AssemblerSystem_ShouldAssembleWhenCraftingIsComplete() {
        World w = new World(); 
        w.AddComponentType<Machine>(); 
        w.AddComponentType<Data>(); 
        w.AddComponentType<TestAssembler>(); 
        w.AddComponentType<Test>(); 
        MachineUpdateSystem.Register(w); 
        AssemblerSystem.Register<TestAssembler, Test>(w); 

        Inventory inv = new Inventory("Test", 2, 2); 
        Machine m = new Machine(inv, new Dictionary<string, int>(), "", 1, minTicks: 1, level: 1, numRecipeToStore: 2); 
        TestAssembler asm = new TestAssembler(m);
        int e = EntityFactory.Add(w, setData: true); 
        w.SetComponent<TestAssembler>(e, asm);
        w.SetComponent<Machine>(e, m); 

        w.Update(); 

        Assert.Equal(2, asm.Assembled); 
    }
}
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.ECS; 

public class MachineUIClickSystemTest {
    [Fact]
    public void MachineUIClickSystem_ShouldMakeADrawMessageWithItsMachine() {
        World w = WorldFactory.Build();
        
        int e = EntityFactory.Add(w);

        w.SetComponent<Button>(e, new Button(true)); 
        Machine m = Machine.GetDefault(); 
        City c = CityWrap.GetTest();
        w.SetComponent<EnterInterfaceButton<MachineInterfaceData>>(e, 
            new EnterInterfaceButton<MachineInterfaceData>(
                new MachineInterfaceData(m, c)
            )
        ); 
        w.Update(); 

        DrawInterfaceMessage<MachineInterfaceData> dm = 
            w.GetComponentArray<DrawInterfaceMessage<MachineInterfaceData>>().FirstOrDefault().Value; 
        Assert.Equal(m, dm.Data.GetMachine()); 
    }
}
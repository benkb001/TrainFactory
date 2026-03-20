using TrainGame.Systems; 

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class SetTrainProgramClickSystemTest {
    [Fact]
    public void SetTrainProgramClickSystem_ShouldSetTrainProgramWhenClicked() {
        World w = WorldFactory.Build(); 
        Bootstrap.InitWorld(w); 
        int e = EntityFactory.Add(w); 
        w.SetComponent<Button>(e, new Button(true)); 
        Train t = TrainWrap.GetTestTrain(); 
        int trainEnt = EntityFactory.AddData<Train>(w, t);
        t.Inv.Add(ItemID.Motherboard, 1); 
        w.SetComponent<SetTrainProgramButton>(e, 
            new SetTrainProgramButton("Iron To Factory", t, trainEnt, TAL.Scripts["Iron To Factory"]));
        w.Update(); 
        Assert.Equal(TAL.Scripts["Iron To Factory"], t.Program); 
        (TALBody<Train, City> _, bool hasExe) = w.GetComponentSafe<TALBody<Train, City>>(trainEnt); 
    }
}
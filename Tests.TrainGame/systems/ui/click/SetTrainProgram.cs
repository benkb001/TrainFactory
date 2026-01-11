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
        t.Inv.Add(ItemID.Motherboard, 1); 
        w.SetComponent<SetTrainProgramButton>(e, 
            new SetTrainProgramButton(TAL.IronFactoryLoop, t, TAL.Scripts[TAL.IronFactoryLoop]));
        w.Update(); 
        Assert.Equal(TAL.Scripts[TAL.IronFactoryLoop], t.Program); 
        Assert.NotNull(t.Executable); 
    }
}
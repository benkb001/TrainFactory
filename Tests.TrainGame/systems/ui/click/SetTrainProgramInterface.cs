using TrainGame.Systems; 

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Constants; 
using TrainGame.Utils; 

public class SetTrainProgramInterfaceClickSystemTest {
    [Fact]
    public void SetTrainProgramInterfaceClickSystem_ShouldMakeADrawSetTrainProgramInterfaceMessageWhenClicked() {
        World w = new World(); 
        RegisterComponents.All(w); 
        SetTrainProgramInterfaceClickSystem.Register(w); 
        int e = EntityFactory.Add(w); 
        w.SetComponent<Button>(e, new Button(true)); 
        Train t = TrainWrap.GetTestTrain(); 
        int trainEnt = EntityFactory.AddData<Train>(w, t);
        w.SetComponent<SetTrainProgramInterfaceButton>(e, new SetTrainProgramInterfaceButton(t, trainEnt));
        w.Update(); 
        DrawSetTrainProgramInterfaceMessage dm = w.GetComponentArray<DrawSetTrainProgramInterfaceMessage>(
        ).FirstOrDefault().Value; 
        Assert.Equal(t, dm.GetTrain()); 
    }
}
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
        w.SetComponent<SetTrainProgramInterfaceButton>(e, new SetTrainProgramInterfaceButton(t));
        w.Update(); 
        DrawSetTrainProgramInterfaceMessage dm = w.GetComponentArray<DrawSetTrainProgramInterfaceMessage>(
        ).FirstOrDefault().Value; 
        Assert.Equal(t, dm.GetTrain()); 
    }
}
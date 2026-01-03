namespace TrainGame.Systems; 

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Constants; 
using TrainGame.Utils; 

public static class SetTrainProgramInterfaceClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<SetTrainProgramInterfaceButton>(w, (w, e) => {
            SetTrainProgramInterfaceButton b = w.GetComponent<SetTrainProgramInterfaceButton>(e); 
            Train t = b.GetTrain(); 

            MakeMessage.Add<DrawSetTrainProgramInterfaceMessage>(w, new DrawSetTrainProgramInterfaceMessage(t)); 
        }); 
    }
}
namespace TrainGame.Components;

using TrainGame.Systems;

public class ElevatorInterfaceData : IInterfaceData {
    
    public ElevatorInterfaceData() {}

    public SceneType GetSceneType() {
        return SceneType.ElevatorInterface; 
    }

    public Menu GetMenu() {
        return new Menu(); 
    }
}
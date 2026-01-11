namespace TrainGame.Components; 

using TrainGame.Systems; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public interface IInterfaceData {
    public Menu GetMenu(); 
    public SceneType GetSceneType(); 
}
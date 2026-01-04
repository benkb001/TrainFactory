namespace TrainGame.Components;

using TrainGame.Systems;

public class Scene {
    private SceneType type; 
    public SceneType Type => type; 
    public Scene(SceneType type = SceneType.RPG) {
        this.type = type; 
    }
}
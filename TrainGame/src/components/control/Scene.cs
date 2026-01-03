namespace TrainGame.Components;

using TrainGame.Systems;

public class Scene {
    public int Value = 0; 
    private SceneType type; 
    public SceneType Type => type; 
    public Scene(int v = 0, SceneType type = SceneType.RPG) {
        Value = v; 
        this.type = type; 
    }
}
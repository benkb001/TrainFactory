namespace TrainGame.Components;

public class Scene {
    public int Value = 0; 
    private SceneType type; 
    public SceneType Type => type; 
    public Scene(int v, SceneType type = SceneType.RPG) {
        Value = v; 
        type = type; 
    }
}
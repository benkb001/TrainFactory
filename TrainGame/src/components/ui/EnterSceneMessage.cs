namespace TrainGame.Components;

public class EnterSceneMessage {
    private SceneType type; 
    public SceneType Type => type; 

    public EnterSceneMessage(SceneType type) {
        this.type = type; 
    }
}

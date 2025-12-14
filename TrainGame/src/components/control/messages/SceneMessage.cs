namespace TrainGame.Components;

public class PushSceneMessage {
    private static PushSceneMessage inst; 
    public static PushSceneMessage Get() {
        if (inst is null) {
            inst = new PushSceneMessage(); 
        }
        return inst; 
    }
}

public class PopSceneMessage {
    private static PopSceneMessage inst; 
    public static PopSceneMessage Get() {
        if (inst is null) {
            inst = new PopSceneMessage(); 
        }
        return inst; 
    }
}

public class PopLateMessage {
    public int Scene; 
    public int FrameDelay; 

    public PopLateMessage(int Scene = 0, int FrameDelay = 0) {
        this.Scene = Scene; 
        this.FrameDelay = FrameDelay; 
    }
}
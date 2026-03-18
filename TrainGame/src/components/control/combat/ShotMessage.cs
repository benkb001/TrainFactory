namespace TrainGame.Components;

public class ShotMessage {
    public readonly Vector2 TargetPosition;

    public ShotMessage(Vector2 targetPos) {
        TargetPosition = targetPos;
    }
}
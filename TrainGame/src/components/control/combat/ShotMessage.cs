namespace TrainGame.Components;

using Microsoft.Xna.Framework;

//This is put on the shooting entity to say that
//basically it tried to shoot at this position
public class ShotMessage {
    public readonly Vector2 TargetPosition;

    public ShotMessage(Vector2 targetPos) {
        TargetPosition = targetPos;
    }
}
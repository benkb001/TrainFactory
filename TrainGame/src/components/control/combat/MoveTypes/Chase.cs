namespace TrainGame.Components;

public class ChaseMovePattern : IMovementType {
    public float Speed; 
    public readonly int SecondsToChase;

    public ChaseMovePattern(float Speed, int SecondsToChase = 2) {
        this.Speed = Speed; 
        this.SecondsToChase = SecondsToChase;
    }

    public IMovementType Clone() => new ChaseMovePattern(Speed);
}
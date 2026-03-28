namespace TrainGame.Components;

public class ChaseMovePattern : IMovementType {
    public float Speed; 

    public ChaseMovePattern(float Speed) {
        this.Speed = Speed; 
    }

    public IMovementType Clone() => new ChaseMovePattern(Speed);
}
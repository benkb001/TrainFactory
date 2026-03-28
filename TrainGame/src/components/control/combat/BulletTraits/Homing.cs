namespace TrainGame.Components;

public class Homing : IBulletTrait {

    public int TrackedEntity;
    public float Speed; 
    public Homing(int trackedEntity = -1, float Speed = 1f) {
        this.TrackedEntity = trackedEntity;
        this.Speed = Speed;
    }
}
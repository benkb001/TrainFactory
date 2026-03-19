namespace TrainGame.Components;

public class Homing : IBulletTrait {

    public int TrackedEntity;
    public Homing(int trackedEntity = -1) {
        this.TrackedEntity = trackedEntity;
    }
}
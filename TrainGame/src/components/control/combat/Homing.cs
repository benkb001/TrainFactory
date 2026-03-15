namespace TrainGame.Components;

public class Homing {
    private int trackedEntity;
    public int TrackedEntity => trackedEntity; 

    public Homing(int e) {
        this.trackedEntity = e; 
    }
}
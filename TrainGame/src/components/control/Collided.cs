namespace TrainGame.Components;

public class Collided {
    public readonly int CollidedEntity;
    public readonly Frame CollidedFrame;

    public Collided(int c, Frame f) {
        this.CollidedEntity = c; 
        this.CollidedFrame = f;
    }
}
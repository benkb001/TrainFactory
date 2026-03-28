namespace TrainGame.Components;

using TrainGame.Utils;

public class MoveTiming {

    public WorldTime CanMove; 
    public WorldTime StopMove; 

    public MoveTiming(WorldTime now) {
        CanMove = new WorldTime();
        StopMove = new WorldTime();
    }

    public void Update(WorldTime now, WorldTime timeToMove, WorldTime timeToWait) {
        CanMove = now + timeToMove + timeToWait; 
        StopMove = now + timeToMove; 
    }
}
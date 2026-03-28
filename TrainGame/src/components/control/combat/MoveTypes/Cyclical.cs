namespace TrainGame.Components;

using System.Collections.Generic; 
using Microsoft.Xna.Framework;
using TrainGame.Utils; 

public class CyclicalMovePattern : IMovementType {
    private List<Vector2> directions; 
    private List<WorldTime> waitTimes; 

    public int MoveIndex; 
    public CircularList<Vector2> Directions; 
    public CircularList<WorldTime> WaitTimes; 
    public WorldTime TimeToMove; 
    public readonly float Speed; 

    public CyclicalMovePattern(List<Vector2> directions, List<WorldTime> waitTimes, 
        WorldTime timeToMove, float Speed) {
        this.MoveIndex = 0; 
        this.Directions = new CircularList<Vector2>(directions); 
        this.Speed = Speed; 
        this.WaitTimes = new CircularList<WorldTime>(waitTimes);
        this.TimeToMove = timeToMove; 
        this.directions = directions; 
        this.waitTimes = waitTimes; 
    }

    public IMovementType Clone() {
        return new CyclicalMovePattern(directions, waitTimes, TimeToMove, Speed);
    }
}
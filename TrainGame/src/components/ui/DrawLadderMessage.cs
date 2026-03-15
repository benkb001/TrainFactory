namespace TrainGame.Components;

using Microsoft.Xna.Framework;
//This is necessary because if interacting with a ladder 
//draws another ladder on the same frame, 
//bad things with modifying list during iteration
//on ladderInteractSystem entities
public class DrawLadderMessage {
    public Vector2 Pos; 
    public int FloorDest; 

    public DrawLadderMessage(Vector2 Pos, int FloorDest) {
        this.Pos = Pos; 
        this.FloorDest = FloorDest; 
    }
}

namespace TrainGame.Components; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class DrawTrainsViewMessage {
    public List<Train> Trains; 
    public float Width; 
    public float Height; 
    public Vector2 Position;
    public float Padding; 

    public DrawTrainsViewMessage(List<Train> Trains, float Width, float Height, Vector2 Position, float Padding = 0f) {
        this.Trains = Trains; 
        this.Width = Width; 
        this.Height = Height; 
        this.Position = Position; 
        this.Padding = Padding; 
    }
}
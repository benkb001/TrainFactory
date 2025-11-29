namespace TrainGame.Components; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class DrawMachinesViewMessage {
    public List<Machine> Machines; 
    public float Width; 
    public float Height; 
    public Vector2 Position;
    public float Padding; 

    public DrawMachinesViewMessage(List<Machine> Machines, float Width, float Height, Vector2 Position, float Padding = 0f) {
        this.Machines = Machines; 
        this.Width = Width; 
        this.Height = Height; 
        this.Position = Position; 
        this.Padding = Padding; 
    }
}
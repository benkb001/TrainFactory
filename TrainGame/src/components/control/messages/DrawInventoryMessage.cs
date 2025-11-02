namespace TrainGame.Components;

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class DrawInventoryMessage {
    public Inventory Inv; 
    public Vector2 Position; 
    public float Width; 
    public float Height; 
    public float Padding; 
    public int Entity; 

    public DrawInventoryMessage(
        float Width, 
        float Height, 
        Vector2 Position,
        Inventory Inv,
        int Entity,
        float Padding = 0f
    ) {
        this.Inv = Inv;
        this.Position = Position; 
        this.Width = Width; 
        this.Height = Height; 
        this.Padding = Padding; 
        this.Entity = Entity; 
    }
}
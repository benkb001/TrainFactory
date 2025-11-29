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
    public bool SetMenu; 
    public bool DrawLabel; 

    public DrawInventoryMessage(
        float Width, 
        float Height, 
        Vector2 Position,
        Inventory Inv,
        int Entity = -1,
        float Padding = 0f, 
        bool SetMenu = true, 
        bool DrawLabel = false
    ) {
        this.Inv = Inv;
        this.Position = Position; 
        this.Width = Width; 
        this.Height = Height; 
        this.Padding = Padding; 
        this.Entity = Entity; 
        this.SetMenu = SetMenu; 
        this.DrawLabel = DrawLabel; 
    }
}
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
        float w, 
        float h, 
        Vector2 pos,
        Inventory inv,
        int e,
        float padding = 0f
    ) {
        Inv = inv;
        Position = pos; 
        Width = w; 
        Height = h; 
        Padding = padding; 
        Entity = e; 
    }
}
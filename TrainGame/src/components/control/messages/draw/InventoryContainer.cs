namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

public class DrawInventoryContainerMessage<T> where T : IInventorySource {
    public T InvSource; 
    public Vector2 Position; 
    public float Width;  
    public float Height; 
    public bool SetMenu; 
    public bool DrawLabel; 

    public DrawInventoryContainerMessage(T InvSource, Vector2 Position, float Width, float Height, 
        bool SetMenu = true, bool DrawLabel = false) {
        this.InvSource = InvSource; 
        this.Position = Position; 
        this.Width = Width; 
        this.Height = Height; 
        this.SetMenu = SetMenu; 
        this.DrawLabel = DrawLabel; 
    }
}
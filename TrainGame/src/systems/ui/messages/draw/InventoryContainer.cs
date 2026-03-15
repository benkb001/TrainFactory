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
    public int Entity; //this is the entity with the Inventory component
    public int Index; 

    public DrawInventoryContainerMessage(T InvSource, Vector2 Position, float Width, float Height, 
        bool SetMenu = true, int Entity = -1, int Index = 0) {
        this.InvSource = InvSource;
        this.Position = Position; 
        this.Width = Width; 
        this.Height = Height; 
        this.SetMenu = SetMenu; 
        this.Entity = Entity; 
        this.Index = Index; 
    }
}
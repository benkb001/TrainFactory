namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

public class DrawButtonMessage<T> where T : IClickable {
    public readonly T Button; 
    public Vector2 Position; 
    public float Width; 
    public float Height; 
    public string Text; 

    public DrawButtonMessage(T Button, Vector2 Position, float Width, float Height) {
        this.Button = Button; 
        this.Position = Position; 
        this.Width = Width; 
        this.Height = Height; 
    }
}

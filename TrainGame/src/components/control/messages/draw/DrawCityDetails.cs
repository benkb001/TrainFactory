namespace TrainGame.Components;

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

public class DrawCityDetailsMessage {
    public string Detail; 
    public Vector2 Position; 

    public DrawCityDetailsMessage(string Detail, Vector2 Position) {
        this.Detail = Detail; 
        this.Position = Position; 
    }
}
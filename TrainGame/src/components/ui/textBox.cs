namespace TrainGame.Components;

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 

public class TextBox {
    public string Text; 
    public Color TextColor; 
    public float Depth; 
    public float Padding; 
    public float Scale; 

    public TextBox(string t, float Depth = 0f, float Padding = 0f, float Scale = 1f) {
        Text = t; 
        TextColor = Color.White; 
        this.Depth = Depth;
        this.Padding = Padding; 
        this.Scale = Scale; 
    }
}
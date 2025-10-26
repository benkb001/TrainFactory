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

public class Message {
    public string message; 
    public Color color; 
    public float rotation; 
    public Vector2 scale; 
    public float depth; 

    public Message(string m) {
        message = m; 
        color = Color.White; 
        rotation = 0f; 
        scale = new Vector2(1f, 1f); 
        depth = 0f; 
    }
}
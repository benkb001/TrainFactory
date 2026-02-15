namespace TrainGame.Components;

using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Utils;

public class Velocity {
    public Vector2 Vector; 

    public Velocity() {
        Vector = Vector2.Zero; 
    }
    
    public Velocity(float dx, float dy) {
        Vector = new Vector2(dx, dy); 
    }

    public Velocity(Vector2 v) {
        Vector = v; 
    }
}
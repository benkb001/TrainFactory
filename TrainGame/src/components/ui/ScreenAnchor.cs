namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.ECS;

public class ScreenAnchor {
    public Vector2 Position; 

    public ScreenAnchor(Vector2 Position) {
        this.Position = Position; 
    }
}
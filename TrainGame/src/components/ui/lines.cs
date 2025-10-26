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

//TODO: Test
public class Lines {
    //each vector is a point, each pair of points will be connected
    public List<(Vector2, Vector2, Color)> Ls; 

    public Lines() {
        Ls = new(); 
    }
    public void AddLine(Vector2 p1, Vector2 p2) {
        Ls.Add((p1, p2, Color.White)); 
    }

    public void AddLine(Vector2 p1, Vector2 p2, Color c) {
        Ls.Add((p1, p2, c)); 
    }
}
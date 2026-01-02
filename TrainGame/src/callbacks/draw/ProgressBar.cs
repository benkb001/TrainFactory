namespace TrainGame.Callbacks; 

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
using TrainGame.Utils; 

public class DrawProgressBarCallback {
    public static int Draw(World w, Vector2 Position, float Width, float Height) {
        Frame pbFrame = new Frame(Position, Width, Height); 
        ProgressBar pb = new ProgressBar(Width); 
        
        Backgrounds bs = new Backgrounds(); 
        bs.Add(new Background(Colors.Placebo, 1f), pbFrame); 
        bs.Add(new Background(Color.Green, 0.5f), pbFrame); 

        int pbEntity = EntityFactory.Add(w); 
        w.SetComponent<ProgressBar>(pbEntity, pb);
        w.SetComponent<Backgrounds>(pbEntity, bs); 
        w.SetComponent<Frame>(pbEntity, pbFrame); 

        return pbEntity; 
    }

    public static void Create(World w, Vector2 Position, float Width, float Height) {
        int e = EntityFactory.Add(w); 
        w.SetComponent<DrawCallback>(e, new DrawCallback(() => {
            Draw(w, Position, Width, Height);
        })); 
    }
}
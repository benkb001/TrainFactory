namespace TrainGame.Systems; 

using System.Collections.Generic;
using System; 
using System.Linq;
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color; 

using _Color = System.Drawing.Color; 
using _Rectangle = System.Drawing.Rectangle;

using TrainGame.Components; 
using TrainGame.ECS; 

public class ClearLLSystem {
    public static void Register(World w) {
        w.AddSystem(
            [typeof(ClearLLMessage)], 
            (w, e) => {
                LinearLayoutWrap.Clear(w.GetComponent<ClearLLMessage>(e).Entity, w); 
            }
        );
    }
}
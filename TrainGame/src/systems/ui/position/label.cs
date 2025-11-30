namespace TrainGame.Systems; 

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

public class LabelPositionSystem {
    private static Type[] ts = [typeof(Label), typeof(Frame), typeof(Active)]; 
    private static Action<World, int> tf = (w, e) => {
        Label l = w.GetComponent<Label>(e); 
        if (w.EntityExists(l.BodyEntity) && w.ComponentContainsEntity<Frame>(l.BodyEntity)) {
            Frame bodyFrame = w.GetComponent<Frame>(l.BodyEntity);
            Frame labelFrame = w.GetComponent<Frame>(e); 
            labelFrame.SetCoordinates(bodyFrame.GetX(), bodyFrame.GetY() - labelFrame.GetHeight()); 
        } else {
            w.RemoveEntity(e); 
        }
    }; 

    public static void Register(World world) {
        world.AddSystem(ts, tf); 
    }
}
namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

public class TrainMapPositionSystem() {
    public static void Register(World world) {
        Type[] ts = [typeof(TrainUI), typeof(MapUIFlag), typeof(Frame), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            Train t = w.GetComponent<TrainUI>(e).GetTrain(); 
            Vector2 pos = t.GetMapPosition(w.Time);
            w.GetComponent<Frame>(e).SetCoordinates(w.GetCameraTopLeft() + pos); 
            if (!t.IsTraveling()) {
                w.RemoveEntity(e); 
            }
        }; 
        world.AddSystem(ts, tf); 
    }
}
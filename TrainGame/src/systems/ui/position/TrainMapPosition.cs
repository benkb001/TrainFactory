namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

public static class TrainMapPositionSystem {
    public static void Register(World world) {
        Type[] ts = [typeof(TrainUI), typeof(MapUIFlag), typeof(Frame), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            TrainUI tUI = w.GetComponent<TrainUI>(e);
            Train t = tUI.GetTrain(); 
            int trainEnt = tUI.TrainEntity;

            float completion = t.JourneyCompletion;
            City comingFrom = w.GetComponent<ComingFromCity>(trainEnt);
            City goingTo = w.GetComponent<GoingToCity>(trainEnt);
            Vector2 comingFromMapPosition = comingFrom.MapPosition;
            Vector2 goingToMapPosition = goingTo.MapPosition;
            Vector2 pos = comingFromMapPosition + ((goingToMapPosition - comingFromMapPosition) * completion);

            w.GetComponent<Frame>(e).SetCoordinates(w.GetCameraTopLeft() + pos); 
            if (!t.IsTraveling()) {
                w.RemoveEntity(e); 
            }
        }; 
        world.AddSystem(ts, tf); 
    }
}
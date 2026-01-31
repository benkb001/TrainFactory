namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 
using TrainGame.Utils; 

public class TrainTravelSystem() {
    private static Type[] ts = [typeof(Train), typeof(Data)]; 
    private static Action<World, int> tf = (w, e) => {
        Train t = w.GetComponent<Train>(e); 
        if (t.IsArriving()) {
            if (t.HasPlayer) {
                //if train is arriving and it has the player, draw the city the player just chose to go to
                MakeMessage.Add<DrawCityMessage>(w, new DrawCityMessage(t.GoingTo)); 
            } else if (SceneSystem.CurrentScene == SceneType.CityInterface && 
                w.GetMatchingEntities([typeof(Menu), typeof(Active)]).Where(
                e => w.GetComponent<Menu>(e).GetCity() == t.GoingTo).ToList().Count > 0) {
                
                //if player is in the city interface and a train arrives that is going to that city, redraw
                //so the train will appear there 
                MakeMessage.Add<DrawCityInterfaceMessage>(w, new DrawCityInterfaceMessage(t.GoingTo)); 
            }
        }
        t.Update(); 
    }; 

    public static void Register(World world) {
        world.AddSystem(ts, tf); 
    }

    public static void RegisterMove(World w) {
        w.AddSystem([typeof(City), typeof(Data)], (w, e) => {
            City c = w.GetComponent<City>(e); 
            Dictionary<City, List<Train>> tsDict = c.TrainsEnRoute; 
            
            foreach(KeyValuePair<City, List<Train>> kvp in tsDict) {
                List<Train> ts = kvp.Value;

                for (int i = ts.Count - 1; i > 0; i--) {
                    Train cur = ts[i]; 
                    Train inFront = ts[i - 1]; 

                    cur.Move(w.Time, inFront);
                }

                if (ts.Count > 0) {
                    Train t = ts[0];
                    t.Move(w.Time);
                }
            }
        });
    }
}
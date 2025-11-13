namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class DrawCitySystem() {
    
    private static Dictionary<string, Action<World>> layouts = new() {
        [CityID.Factory] = (w) => {
            
        }, 
        [CityID.Greenhouse] = (w) => {

        }, 
        [CityID.Mine] = (w) => {

        }, 
        [CityID.Coast] = (w) => {

        }, 
        [CityID.Reservoir] = (w) => {

        }, 
        [CityID.Collisseum] = (w) => {

        }
    }; 

    private static Type[] ts = [typeof(DrawCityMessage)];
    private static Action<World, int> tf = (w, e) => {
        layouts[w.GetComponent<DrawCityMessage>(e).GetCity().CityId](w); 
        w.RemoveEntity(e); 
    }; 

    public static void Register(World world) {
        world.AddSystem(ts, tf); 
    }
}
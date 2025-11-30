namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class DrawCitySystem {
    
    private static void DrawLayout(string cityID, int maxScene, Vector2 topleft, World w) {
        switch (cityID) {
            case CityID.Factory: 
                break;
            case CityID.Greenhouse: 
                break;
            case CityID.Mine: 
                break;
            case CityID.Coast: 
                break; 
            case CityID.Reservoir: 
                break;
            case CityID.Collisseum: 
                break; 
            default: 
                int e = EntityFactory.Add(w, scene: maxScene); 
                w.SetComponent<Frame>(e, new Frame(topleft, 100, 100));
                w.SetComponent<Outline>(e, new Outline()); 
                w.SetComponent<TextBox>(e, new TextBox("Test")); 
                break; 
        }
    }

    private static Type[] ts = [typeof(DrawCityMessage)];
    private static Action<World, int> tf = (w, e) => {
        int maxScene = w.GetComponentArray<Scene>().Max(kvp => kvp.Value.Value); 
        Vector2 topleft = Vector2.Zero; 
        KeyValuePair<int, CameraReturn> cr = w.GetComponentArray<CameraReturn>().FirstOrDefault(kvp => {
            return w.GetComponent<Scene>(kvp.Key).Value == maxScene;
        }, new KeyValuePair<int, CameraReturn>(0, null)); 
        if (cr.Value != null) {
            topleft = cr.Value.Position - new Vector2(w.ScreenWidth / 2, w.ScreenHeight / 2); 
        }

        foreach (int cur in w.GetComponentArray<Scene>().Where(kvp => {
            return kvp.Value.Value == maxScene && !w.ComponentContainsEntity<CameraReturn>(kvp.Key);
        }).Select(kvp => kvp.Key)) {
            w.RemoveEntity(cur); 
        }

        DrawLayout(w.GetComponent<DrawCityMessage>(e).GetCity().CityId, maxScene, topleft, w);
        w.RemoveEntity(e); 
    }; 

    public static void Register(World world) {
        world.AddSystem(ts, tf); 
    }
}
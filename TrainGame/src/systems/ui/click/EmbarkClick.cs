namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

public class EmbarkClickSystem() {
    public static void Register(World world) {
        Type[] ts = [typeof(EmbarkButton), typeof(Button), typeof(Frame), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            if (w.GetComponent<Button>(e).Clicked) {
                int popEntity = EntityFactory.Add(w); 
                w.SetComponent<PopSceneMessage>(popEntity, PopSceneMessage.Get()); 
                EmbarkButton eb = w.GetComponent<EmbarkButton>(e); 
                Train t = eb.GetTrain();
                City c = eb.GetDestination(); 
                t.Embark(c, w.Time); 
                int tui = Map.DrawTrain(w, t); 
                //so the entity will survive 2 pops, one to click 
                //out of the embark view, and one to click out of 
                //the trains/machines view
                w.GetComponent<Scene>(tui).Value = 2; 
            }
        }; 

        world.AddSystem(ts, tf); 
    }

}
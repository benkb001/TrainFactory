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
                //TODO: think of a way other than hardcoding scene numbers? 
                w.GetComponent<Scene>(tui).Value = 2; 
                //TODO: TEST
                if (t.HasPlayer) {
                    List<int> es = w.GetMatchingEntities([typeof(Scene)]); 
                    foreach (int e in es) {
                        if (w.GetComponent<Scene>(e).Value == 3) {
                            w.RemoveEntity(e); 
                        }
                    }
                }
            }
        }; 

        world.AddSystem(ts, tf); 
    }
}
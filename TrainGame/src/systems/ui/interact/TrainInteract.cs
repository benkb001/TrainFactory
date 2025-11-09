namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

public class TrainInteractSystem() {
    public static void Register(World world) {
        Type[] ts = [typeof(Train), typeof(Interactable), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            if (w.GetComponent<Interactable>(e).Interacted) {
                int pushEntity = EntityFactory.Add(w); 
                w.SetComponent<PushSceneMessage>(pushEntity, PushSceneMessage.Get()); 
                int drawMapMessage = EntityFactory.Add(w); 
                w.SetComponent<DrawMapMessage>(drawMapMessage, DrawMapMessage.Get()); 
            }
        }; 

        world.AddSystem(ts, tf); 
    }
}
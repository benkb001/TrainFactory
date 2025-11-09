namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

public class ChestInteractSystem() {
    public static void Register(World world) {
        Type[] ts = [typeof(Chest), typeof(Interactable), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            if (w.GetComponent<Interactable>(e).Interacted) {
                int chestMsgEntity = EntityFactory.Add(w); 
                int playerMsgEntity = EntityFactory.Add(w); 
                int scenePushEntity = EntityFactory.Add(w); 
                Chest chest = w.GetComponent<Chest>(e); 
                DrawInventoryMessage chestMessage = chest.ChestInvMessage;
                DrawInventoryMessage playerMessage = chest.PlayerInvMessage;
                
                w.SetComponent<DrawInventoryMessage>(chestMsgEntity, chestMessage);
                w.SetComponent<DrawInventoryMessage>(playerMsgEntity, playerMessage);
                w.SetComponent<PushSceneMessage>(scenePushEntity, PushSceneMessage.Get());
            }
        }; 
        world.AddSystem(ts, tf); 
    }
}
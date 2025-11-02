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
                int chestMsgEntity = w.AddEntity(); 
                int playerMsgEntity = w.AddEntity(); 
                int scenePushEntity = w.AddEntity(); 
                Chest chest = w.GetComponent<Chest>(e); 
                DrawInventoryMessage chestMessage = chest.ChestInvMessage; 
                DrawInventoryMessage playerMessage = chest.PlayerInvMessage; 
                
                w.SetComponent<DrawInventoryMessage>(chestMsgEntity, chestMessage);
                w.SetComponent<DrawInventoryMessage>(playerMsgEntity, playerMessage);
                w.SetComponent<PushSceneMessage>(scenePushEntity, PushSceneMessage.Get());
                //so they aren't set to inactive after pushing 
                w.SetComponent<Scene>(chest.PlayerInvMessage.Entity, new Scene(-1)); 
                w.SetComponent<Scene>(chest.ChestInvMessage.Entity, new Scene(-1)); 
            }
        }; 
        world.AddSystem(ts, tf); 
    }
}
namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Callbacks;

public class ChestInteractSystem() {
    public static void Register(World world) {
        Type[] ts = [typeof(Chest), typeof(Interactable), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            if (w.GetComponent<Interactable>(e).Interacted) {
                Chest chest = w.GetComponent<Chest>(e);

                DrawInventoryCallback.Create(w, chest.ChestInv, chest.ChestInvDrawPosition, chest.ChestInvWidth, 
                    chest.ChestInvHeight, SetMenu: true);
                DrawInventoryCallback.Create(w, chest.PlayerInv, chest.PlayerInvDrawPosition, chest.PlayerInvWidth, 
                    chest.PlayerInvHeight, SetMenu: true);

            }
        }; 
        world.AddSystem(ts, tf); 
    }
}
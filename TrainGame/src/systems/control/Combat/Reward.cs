namespace TrainGame.Systems; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public static class RewardInteractSystem {
    public static void Register<T>(World w, Action<World, int, int> tf) {
        InteractSystem.Register<T>(w, (w, e) => {
            int interactorEntity = w.GetComponent<Interactable>(e).InteractorEntity; 
            tf(w, e, interactorEntity);
            MakeMessage.Add<CombatRewardCollectedMessage>(w, new CombatRewardCollectedMessage()); 
        });
    }

    public static void RegisterRemove(World w) {
        w.AddSystem([typeof(CombatRewardCollectedMessage)], (w, e) => {
            List<int> rewardEnts = w.GetMatchingEntities([typeof(CombatReward)]); 
            rewardEnts.ForEach(ent => {
                w.GetComponent<CombatReward>(ent).Active = false; 
                w.RemoveEntity(ent);
            }); 
            w.RemoveEntity(e); 
        });
    }
}

public static class TempArmorInteractSystem {
    public static void Register(World w) {
        RewardInteractSystem.Register<TempArmor>(w, (w, e, interactorEntity) => {
            TempArmor tArmor = w.GetComponent<TempArmor>(e); 
            Armor playerArmor = w.GetComponent<Armor>(interactorEntity); 
            playerArmor.AddTempDefense(tArmor.Defense); 
        });
    }
}
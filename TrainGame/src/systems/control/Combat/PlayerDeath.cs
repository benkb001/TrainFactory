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

public static class PlayerDeathSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Player), typeof(Health), typeof(RespawnLocation), 
        typeof(Inventory), typeof(Active)], (w, e) => {
            Health h = w.GetComponent<Health>(e);
            if (h.HP <= 0) {
                PlayerWrap.ResetStats(w); 
                Inventory inv = LootWrap.GetDestination(w);
                Dictionary<string, int> taken = new(); 
                string[] toTake = {ItemID.Credit, ItemID.Cobalt, ItemID.Mythril, ItemID.Adamantite};

                foreach (string itemID in toTake) {
                    int lost = inv.ItemCount(itemID) / 2; 
                    if (lost > 0) {
                        inv.Take(itemID, lost); 
                        taken[itemID] = lost;
                    }
                }

                City c = w.GetComponent<RespawnLocation>(e).GetCity(); 
                MakeMessage.Add<DrawCityMessage>(w, new DrawCityMessage(c));
                MakeMessage.Add<DrawToastMessage>(w, new DrawToastMessage($"Lost\n{Util.FormatMap(taken)} on death", 1200));
            }
        });
    }
}
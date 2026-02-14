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

public static class PlayerStats {
    public static void Reset(World w) {
        int e = PlayerWrap.GetEntity(w); 
        Health h = w.GetComponent<Health>(e); 
        h.ResetHP(); 
        Armor armor = w.GetComponent<Armor>(e); 
        armor.ResetTempDefense(); 
        Floor f = w.GetComponent<Floor>(e);
        f.Reset();
    }
}

public static class PlayerDeathSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Player), typeof(Health), typeof(Armor), typeof(RespawnLocation), 
        typeof(Inventory), typeof(Active)], (w, e) => {
            Health h = w.GetComponent<Health>(e);
            if (h.HP <= 0) {
                PlayerStats.Reset(w); 
                Inventory inv = LootWrap.GetDestination(w);
                inv.Take(ItemID.Credit, inv.ItemCount(ItemID.Credit) / 2); 
                City c = w.GetComponent<RespawnLocation>(e).GetCity(); 
                MakeMessage.Add<DrawCityMessage>(w, new DrawCityMessage(c));
            }
        });
    }
}
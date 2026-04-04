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
        w.AddSystem([typeof(Player), typeof(Health), typeof(RespawnLocation), typeof(Active)], (w, e) => {
            Health h = w.GetComponent<Health>(e);
            if (h.HP <= 0) {
                PlayerWrap.ResetStats(w); 
                MakeMessage.Add<DrawCityMessage>(w, new DrawCityMessage(w.GetComponent<RespawnLocation>(e).GetCity()));
            }
        });
    }
}
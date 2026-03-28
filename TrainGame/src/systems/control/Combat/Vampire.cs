namespace TrainGame.Systems; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public static class ApplyVampiredSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Vampiric), typeof(HitMessage), typeof(ShotBy), typeof(Active)], (w, e) => {
            int hitEntity = w.GetComponent<HitMessage>(e).Entity; 
            int shooterEnt = w.GetComponent<ShotBy>(e).Entity;
            int dmg = w.GetComponent<Vampiric>(e).Damage;
            w.SetComponent<Vampired>(hitEntity, new Vampired(shooterEnt, dmg));
        });
    }
}

public static class RemoveVampiredSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Vampired), typeof(Active)], (w, e) => {
            int vampiredByEnt = w.GetComponent<Vampired>(e).VampiredByEntity;
            if (!w.EntityExists(vampiredByEnt)) {
                w.RemoveComponent<Vampired>(e);
                w.SetComponent<EndVampired>(e, new EndVampired());
            }
        });
    }
}

public static class RemoveEndVampiredSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(EndVampired), typeof(Active)], (w, e) => {
            w.RemoveComponent<EndVampired>(e);
        });
    }
}

public static class VampireDamageSystem {
    public static void Register(World w) {
        w.AddSystem((w) => {
            if (w.Time.Ticks == 0) {
                List<int> vampiredEnts = w.GetMatchingEntities([typeof(Vampired), typeof(Active)]);
                vampiredEnts.ForEach(e => {
                    int dmg = w.GetComponent<Vampired>(e).Damage;
                    w.SetComponent<ReceiveDamageMessage>(e, new ReceiveDamageMessage(dmg));
                });
            }
        });
    }
}

namespace TrainGame.Utils; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.ECS;
using TrainGame.Constants; 
using TrainGame.Callbacks; 
using TrainGame.Systems;

public static class PlayerWrap {
    public static int GetEntity(World w) {
        return w.GetMatchingEntities([typeof(Player), typeof(Data)])[0];
    }

    public static int GetRPGEntity(World w) {
        return w.GetMatchingEntities([typeof(Player), typeof(Frame), typeof(Health), typeof(Active)])[0];
    }
}
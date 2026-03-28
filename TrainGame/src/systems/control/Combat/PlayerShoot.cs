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

public static class PlayerShootSystem {
    public static void Register(World w) {
        w.AddSystem((w) => {
            if (VirtualMouse.LeftPressed()) {
                //TODO: Add back in after refactor
            }
        });
    }
}
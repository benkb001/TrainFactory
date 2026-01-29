namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Utils;
using TrainGame.Constants;

public static class ConnectCitiesClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<ConnectCitiesButton>(w, (w, e, b) => {
            b.TryConnect(); 
            DrawCityInterfaceSystem.AddMessage(w, b.GetOrigin());
        });
    }
}
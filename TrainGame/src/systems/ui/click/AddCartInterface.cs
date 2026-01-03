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
using TrainGame.Callbacks; 

public static class AddCartInterfaceClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<AddCartInterfaceButton>(w, (w, e) => {
            int dm = EntityFactory.Add(w, setScene: false); 
            AddCartInterfaceButton btn = w.GetComponent<AddCartInterfaceButton>(e); 
            w.SetComponent<DrawAddCartInterfaceMessage>(dm, 
                new DrawAddCartInterfaceMessage(btn.CartDest, btn.CartSource)); 
        }); 
    }
}
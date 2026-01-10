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

public static class AddCartClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<AddCartButton>(w, (w, e) => {
            AddCartButton btn = w.GetComponent<AddCartButton>(e); 
            Cart cart = btn.CartToAdd; 
            btn.CartDest.AddCart(cart); 
            btn.CartSource.RemoveCart(cart); 
            MakeMessage.Add<DrawTrainInterfaceMessage>(w, new DrawTrainInterfaceMessage(btn.CartDest)); 
        }); 
    }
}
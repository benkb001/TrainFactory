namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants;
using TrainGame.Callbacks; 

public static class AddCartClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<AddCartButton>(w, (w, e) => {
            AddCartButton btn = w.GetComponent<AddCartButton>(e); 
            CartType type = btn.TypeToAdd; 
            btn.CartDest.AddCart(type); 
            btn.CartSource.RemoveCart(type); 
        }); 

        ClickSystem.Register([typeof(AddCartButton), typeof(TextBox)], w, (w, e) => {
            AddCartButton btn = w.GetComponent<AddCartButton>(e); 
            TextBox tb = w.GetComponent<TextBox>(e); 
            CartType type = btn.TypeToAdd;
            tb.Text = Buttons.AddCartLabel(type.ToString(), btn.CartSource.NumCarts(type));
        });
    }
}
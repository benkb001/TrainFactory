namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Constants;

public class Cart {
    public readonly CartType Type; 

    public Cart(CartType type) {
        this.Type = type; 
    }
}
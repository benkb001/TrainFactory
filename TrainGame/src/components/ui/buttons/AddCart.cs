
namespace TrainGame.Components; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 

public class AddCartButton {
    public readonly Train CartDest; 
    public readonly City CartSource; 
    public readonly Cart CartToAdd; 

    public AddCartButton(Train train, City source, Cart cart) {
        CartDest = train; 
        CartSource = source; 
        CartToAdd = cart; 
    }
}
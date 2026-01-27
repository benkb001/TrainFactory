
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
    public readonly CartType TypeToAdd; 

    public AddCartButton(Train train, City source, CartType type) {
        CartDest = train; 
        CartSource = source; 
        TypeToAdd = type; 
    }
}
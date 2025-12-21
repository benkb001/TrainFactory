
namespace TrainGame.Components; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 

public class AddCartInterfaceButton : IClickable {
    public readonly Train CartDest; 
    public readonly City CartSource; 
    private string text; 

    public AddCartInterfaceButton(Train CartDest, City CartSource) {
        this.CartDest = CartDest; 
        this.CartSource = CartSource; 
        text = $"Add a cart to {CartDest.Id}"; 
    }

    public string GetText() {
        return text; 
    }
}
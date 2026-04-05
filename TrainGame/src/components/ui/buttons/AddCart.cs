
namespace TrainGame.Components; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 

//Cartdest and src could be interfaces here but
//this is so low prio like when will this ever need to be generalized 
public class AddCartButton {
    public readonly Train CartDest; 
    public readonly City CartSource; 
    public readonly CartType TypeToAdd; 
    public readonly int TrainEntity;

    public AddCartButton(Train train, City source, CartType type, int trainEnt) {
        CartDest = train; 
        CartSource = source; 
        TypeToAdd = type; 
        TrainEntity = trainEnt;
    }
}
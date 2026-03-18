namespace TrainGame.Systems;

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Utils;
using TrainGame.Constants;


public static class ShooterWrap {
    
    //returns the velocity needed to shoot at targetPos from pos with speed speed
    public static Vector2 Aim(Vector2 pos, Vector2 targetPos, float speed) {
        return (Vector2.Normalize(targetPos - pos)) * speed;
    }
}
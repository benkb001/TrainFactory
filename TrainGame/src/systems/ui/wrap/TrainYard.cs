namespace TrainGame.Systems;

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Constants;

public static class TrainYardWrap {
    public static void Draw(World w, Vector2 pos) {
        int e = EntityFactory.AddUI(w, pos, Constants.TileWidth, Constants.TileWidth, 
            text: "Train Yard", setInteractable: true, setOutline: true, setCollidable: true); 
        w.SetComponent<TrainYard>(e, TrainYard.Get()); 
    }
}
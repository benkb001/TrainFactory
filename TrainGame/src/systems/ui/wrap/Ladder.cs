namespace TrainGame.Systems;

using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TrainGame.Utils;
using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Constants;

public static class LadderWrap {
    public static void AddMessage(World w, Vector2 pos, int FloorDest) {
        MakeMessage.Add<DrawLadderMessage>(w, new DrawLadderMessage(pos, FloorDest));
    }

    public static void Draw(World w, Vector2 pos, int FloorDest) {
        int e = EntityFactory.AddUI(w, pos, Constants.TileWidth, Constants.TileWidth, 
            setInteractable: true, text: FloorDest == 0 ? "Exit" : "Ladder", setOutline: true);
        w.SetComponent<Ladder>(e, new Ladder(FloorDest));
    }
}
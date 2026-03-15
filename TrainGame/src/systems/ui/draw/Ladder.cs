namespace TrainGame.Systems;

using System.Collections.Generic;

using TrainGame.Components;
using TrainGame.ECS;

public static class DrawLadderSystem {
    public static void Register(World w) {
        DrawSystem.Register<DrawLadderMessage>(w, (w, e, dm) => {
            LadderWrap.Draw(w, dm.Pos, dm.FloorDest);
        });
    }
}
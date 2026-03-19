namespace TrainGame.Systems;

using System;
using System.Collections.Generic;

using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Utils;
using TrainGame.Constants;

public static class WarnedWrap {
    public static void RegisterTrait() {
        BulletTraitRegistry.Register<Warned>((w, warned, ent) => {
            w.RemoveComponent<Active>(ent); 
            int warnEnt = EntityFactory.Add(w); 
            (Frame bulletFrame, bool hasFrame) = w.GetComponentSafe<Frame>(ent);
            
            if (!hasFrame) {
                w.RemoveEntity(ent);
                return;
            }

            w.SetComponent<Frame>(warnEnt, new Frame(bulletFrame));
            bulletFrame.SetCoordinates(SceneSystem.OffScreenPosition);
            BulletWarning warn = new BulletWarning(w.Time + warned.WarningDuration, ent);
            w.SetComponent<BulletWarning>(warnEnt, warn); 
            w.SetComponent<Outline>(warnEnt, new Outline(Colors.Warning));
            TextBox tb = new TextBox("!"); 
            tb.TextColor = Colors.Warning;
            w.SetComponent<TextBox>(warnEnt, tb);
        });
    }
}
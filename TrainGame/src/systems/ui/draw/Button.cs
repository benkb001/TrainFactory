namespace TrainGame.Systems; 

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.ECS; 
using TrainGame.Components; 

public static class DrawButtonSystem {
    public static void Register<T>(World w) where T : IClickable {
        w.AddSystem(
            [typeof(DrawButtonMessage<T>)], 
            (w, e) => {
                int btnEntity = EntityFactory.Add(w); 
                DrawButtonMessage<T> dm = w.GetComponent<DrawButtonMessage<T>>(e);
                w.SetComponent<T>(btnEntity, dm.Button); 
                w.SetComponent<Button>(btnEntity, new Button()); 
                w.SetComponent<Frame>(btnEntity, new Frame(dm.Position, dm.Width, dm.Height)); 
                w.SetComponent<Outline>(btnEntity, new Outline()); 
                w.SetComponent<TextBox>(btnEntity, new TextBox(dm.Button.GetText())); 
                w.RemoveEntity(e); 
            }
        );
    }
}
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
    
    public static int Draw<T>(DrawButtonMessage<T> dm, World w) where T : IClickable {
        int btnEntity = EntityFactory.Add(w); 
        w.SetComponent<T>(btnEntity, dm.Button); 
        w.SetComponent<Button>(btnEntity, new Button()); 
        w.SetComponent<Frame>(btnEntity, new Frame(dm.Position, dm.Width, dm.Height)); 
        w.SetComponent<Outline>(btnEntity, new Outline()); 
        w.SetComponent<TextBox>(btnEntity, new TextBox(dm.Button.GetText())); 
        return btnEntity; 
    }

    public static void Register<T>(World w) where T : IClickable {
        w.AddSystem(
            [typeof(DrawButtonMessage<T>)], 
            (w, e) => {
                DrawButtonMessage<T> dm = w.GetComponent<DrawButtonMessage<T>>(e);
                Draw<T>(dm, w); 
                w.RemoveEntity(e); 
            }
        );
    }
}
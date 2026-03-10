namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

public static class ToastSystem {
    //TODO: Make it more dynamic so toast can fade with sprite and such? 
    private static Type[] types = [typeof(TextBox), typeof(Frame), typeof(Outline), typeof(Toast), typeof(Active)]; 
    private static Action<World, int> transformer = (w, e) => {
        Toast t = w.GetComponent<Toast>(e);
        TextBox tb = w.GetComponent<TextBox>(e);
        Outline o = w.GetComponent<Outline>(e); 
        float prev_opacity = t.RemainingDuration; 
        
        tb.TextColor *= (1f /prev_opacity);
        o.SetColor(o.GetColor() * (1f/prev_opacity));  
        t.DecrementDuration(); 
        float cur_opacity = t.RemainingDuration; 
        tb.TextColor *= cur_opacity; 
        o.SetColor(o.GetColor() * cur_opacity); 

        if (cur_opacity <= 0f) {
            w.RemoveEntity(e); 
        }
    }; 

    public static void Register(World world) {
        world.AddSystem(types, transformer); 
    }

    public static void Draw(World w, string message) {
        float width = w.ScreenWidth / 4f; 
        float height = w.ScreenHeight / 4f;

        int e = EntityFactory.AddUI(w, new Vector2((w.ScreenWidth / 2f) - (width / 2f), w.ScreenHeight / 3f), width, height, 
            setOutline: true, screenAnchor: true, text: message, setToast: true);
    }

    public static void RegisterDraw(World w) {
        w.AddSystem([typeof(DrawToastMessage)], (w, e) => {
            Draw(w, w.GetComponent<DrawToastMessage>(e).Text);
            w.RemoveEntity(e); 
        });
    }
}

public class DrawToastMessage {
    public readonly string Text; 

    public DrawToastMessage(string Text) {
        this.Text = Text; 
    }
}
namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

public class ToastSystem() {
    //TODO: Make it more dynamic so toast can fade with sprite and such? 
    private static Type[] types = [typeof(TextBox), typeof(Frame), typeof(Outline), typeof(Toast)]; 
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
}
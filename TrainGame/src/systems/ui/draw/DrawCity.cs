namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Callbacks; 
using TrainGame.Systems;

public static class DrawHPSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Health), typeof(TextBox), typeof(Active)], (w, e) => {
            Health h = w.GetComponent<Health>(e); 
            TextBox tb = w.GetComponent<TextBox>(e); 
            
            tb.Text = $"HP: {h.HP}";
        });
    }
}

public static class DrawCitySystem {
    
    private static Type[] ts = [typeof(DrawCityMessage)];
    private static Action<World, int> tf = (w, e) => {
        foreach (int ent in w.GetMatchingEntities([typeof(City), typeof(Data)])) {
            w.GetComponent<City>(ent).HasPlayer = false; 
        }

        City c = w.GetComponent<DrawCityMessage>(e).GetCity();
        c.HasPlayer = true; 

        w.LockCamera(); 

        Layout.Draw(w, Layout.Cities[c.Id]);
        w.RemoveEntity(e); 
    }; 

    public static void Register(World world) {
        world.AddSystem(ts, tf); 
    }
}
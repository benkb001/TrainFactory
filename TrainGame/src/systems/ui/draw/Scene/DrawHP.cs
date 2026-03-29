namespace TrainGame.Systems;

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using TrainGame.Components;
using TrainGame.ECS;

public static class DrawHPSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Health), typeof(Parrier), typeof(TextBox), typeof(Active)], (w, e) => {
            Health h = w.GetComponent<Health>(e); 
            TextBox tb = w.GetComponent<TextBox>(e); 
            Parrier p = w.GetComponent<Parrier>(e);
            
            tb.Text = $"HP: {h.HP}\nShield: {p.HP}";
        });
    }
}
namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class AmmoHUD {}

public static class DrawAmmoHUDSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(AmmoHUD), typeof(TextBox), typeof(Active)], (w, e) => {
            Shooter shooter = PlayerWrap.GetShooter(w); 
            if (shooter != null) {
                w.GetComponent<TextBox>(e).Text = $"Ammo: {shooter.Ammo} / {shooter.MaxAmmo}";
            }
        });
    }
}
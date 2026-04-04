namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public class Shooter {
    public WorldTime TimeBetweenShots;
    public WorldTime ReloadTime; 
    public WorldTime CanShoot; 

    public int Ammo; 
    public int MaxAmmo;
    public readonly int BaseMaxAmmo;

    public Shooter(int ammo = 6, int ticksPerShot = 30, int reloadTicks = 60) {
        this.Ammo = ammo; 
        this.MaxAmmo = ammo;
        this.BaseMaxAmmo = ammo;
        this.TimeBetweenShots = new WorldTime(ticks: ticksPerShot); 
        this.ReloadTime = new WorldTime(ticks: reloadTicks);
        this.CanShoot = new WorldTime();
    }

    public void Update(WorldTime now, int shot = 1) {
        Ammo-= shot; 

        if (Ammo <= 0) {
            Ammo = MaxAmmo; 
            CanShoot = now + ReloadTime;
        } else {
            CanShoot = now + TimeBetweenShots;
        }
    }

    public Shooter Clone() {
        return new Shooter( MaxAmmo, TimeBetweenShots.InTicks(), ReloadTime.InTicks());
    }
}
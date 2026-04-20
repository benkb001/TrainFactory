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
    public WorldTime LastShot; 

    public int Ammo; 
    public int MaxAmmo;
    public readonly int BaseMaxAmmo;
    public bool Reloading = false;

    public Shooter(int ammo = 6, int ticksPerShot = 30, int reloadTicks = 60) {
        this.Ammo = ammo; 
        this.MaxAmmo = ammo;
        this.BaseMaxAmmo = ammo;
        this.TimeBetweenShots = new WorldTime(ticks: ticksPerShot); 
        this.ReloadTime = new WorldTime(ticks: reloadTicks);
        this.CanShoot = new WorldTime();
    }

    public void Update(WorldTime now) {
        Ammo--;
        LastShot = now.Clone();

        if (Ammo <= 0) {
            Reloading = true;
            Ammo = 0;
            CanShoot = now + ReloadTime;
        } else {
            Reloading = false; 
            CanShoot = now + TimeBetweenShots;
        }
    }

    public Shooter Clone() {
        return new Shooter( MaxAmmo, TimeBetweenShots.InTicks(), ReloadTime.InTicks());
    }

    public float GetReloadCompletion(WorldTime now) {
        return (CanShoot - now) / ReloadTime;
    }
}
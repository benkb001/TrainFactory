namespace TrainGame.Systems; 

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
    private WorldTime canShoot; 
    private int ticksPerShot; 
    private int ammo; 
    private int maxAmmo; 
    private int reloadTicks; 
    private IShootPattern shootPattern;

    public int Ammo => ammo; 
    public int MaxAmmo => maxAmmo; 
    public IShootPattern ShootPattern => shootPattern;

    public Shooter(IShootPattern shootPattern, int ammo = 10, int ticksPerShot = 10, int reloadTicks = 20) {
        this.shootPattern = shootPattern;
        this.ticksPerShot = ticksPerShot; 
        this.ammo = ammo; 
        this.maxAmmo = ammo; 
        canShoot = new WorldTime(); 
        this.reloadTicks = reloadTicks;
    }

    public IEnumerable<BulletContainer> Shoot(WorldTime now, Vector2 position, Vector2 targetPosition) {
        if (CanShoot(now)) {
            ammo -= shootPattern.GetBulletsShot(); 

            if (ammo <= 0) {
                ammo = maxAmmo; 
                canShoot = now + new WorldTime(ticks: reloadTicks);
            } else {
                canShoot = now + new WorldTime(ticks: ticksPerShot);
            }

            return shootPattern.Shoot(position, targetPosition);
        }

        return new List<BulletContainer>();
    }

    public bool CanShoot(WorldTime t) {
        return t.IsAfterOrAt(canShoot); 
    }
}
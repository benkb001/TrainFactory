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

class Shooter {
    private float bulletSpeed; 
    private int bulletDamage; 
    private WorldTime canShoot; 
    private int ticksPerShot; 
    private int skill; 
    private int ammo; 
    private int maxAmmo; 

    public Shooter(int bulletDamage = 1, int ticksPerShot = 30, float bulletSpeed = Constants.BulletSpeed, 
        int ammo = 10, int skill = 1) {
        this.bulletDamage = bulletDamage; 
        this.ticksPerShot = ticksPerShot; 
        this.bulletSpeed = bulletSpeed; 
        this.skill = 1; 
        this.ammo = ammo; 
        this.maxAmmo = ammo; 
        canShoot = new WorldTime(); 
    }

    public Bullet Shoot(WorldTime now) {
        ammo--; 
        if (ammo <= 0) {
            ammo = maxAmmo; 
            canShoot = now + new WorldTime(ticks: ticksPerShot * 5);
        } else {
            canShoot = now + new WorldTime(ticks: ticksPerShot);
        }

        return new Bullet(now, bulletDamage); 
    }

    public float GetBulletSpeed() => bulletSpeed; 
    public int GetBulletDamage() => bulletDamage; 
    public bool CanShoot(WorldTime t) {
        return t.IsAfterOrAt(canShoot); 
    }
}
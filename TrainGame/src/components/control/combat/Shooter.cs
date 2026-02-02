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

public enum ShootPattern {
    Default,
    Multi
}

public enum BulletType {
    Default, 
    Homing
}

public enum OnExpireEffect {
    Default, 
    Explosion
}

public class Shooter {
    private float bulletSpeed; 
    private int bulletDamage; 
    private WorldTime canShoot; 
    private int ticksPerShot; 
    private int skill; 
    private int ammo; 
    private int maxAmmo; 
    private int bulletsPerShot; 
    private int bulletSize; 
    private float spreadDegrees = 10f;
    private ShootPattern shootPattern;
    private BulletType bulletType; 
    private OnExpireEffect onExpireEffect;

    public ShootPattern GetShootPattern() => shootPattern; 
    public BulletType GetBulletType() => bulletType; 
    public OnExpireEffect GetOnExpireEffect() => onExpireEffect;
    public int BulletsPerShot => bulletsPerShot; 
    public int BulletSize => bulletSize; 
    public float SpreadDegrees => spreadDegrees;

    public Shooter(int bulletDamage = 1, int ticksPerShot = 30, float bulletSpeed = 3, 
        int ammo = 10, int skill = 1, int bulletsPerShot = 1, ShootPattern shootPattern = ShootPattern.Default, 
        BulletType bulletType = BulletType.Default, OnExpireEffect onExpireEffect = OnExpireEffect.Default, 
        int bulletSize = Constants.BulletSize) {
        this.bulletDamage = bulletDamage; 
        this.ticksPerShot = ticksPerShot; 
        this.bulletSpeed = bulletSpeed; 
        this.skill = 1; 
        this.ammo = ammo; 
        this.maxAmmo = ammo; 
        canShoot = new WorldTime(); 
        this.shootPattern = shootPattern; 
        this.bulletType = bulletType; 
        this.onExpireEffect = onExpireEffect; 
        this.bulletsPerShot = bulletsPerShot; 
        this.bulletSize = bulletSize; 
    }

    public Bullet Shoot(WorldTime now) {
        ammo--; 
        if (ammo <= 0) {
            ammo = maxAmmo; 
            canShoot = now + new WorldTime(ticks: ticksPerShot * 5);
        } else {
            canShoot = now + new WorldTime(ticks: ticksPerShot);
        }

        return new Bullet(bulletDamage); 
    }

    public float GetBulletSpeed() => bulletSpeed; 
    public int GetBulletDamage() => bulletDamage; 
    public int Inaccuracy => 100 - skill; 
    public bool CanShoot(WorldTime t) {
        return t.IsAfterOrAt(canShoot); 
    }
}
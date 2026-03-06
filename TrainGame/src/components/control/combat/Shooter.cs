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

public enum ShooterType {
    Enemy,
    Player
}

public enum ShootPattern {
    Circle,
    Default,
    HorizontalLine,
    Multi,
    Square,
    VerticalLine
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
    private float spreadDegrees;
    private int patternIndex = 0; 
    private int patternLength;
    private float patternSize;
    private int reloadTicks; 
    private int bulletLifetimeTicks;

    private ShootPattern shootPattern;
    private BulletType bulletType; 
    private OnExpireEffect onExpireEffect;

    public ShootPattern GetShootPattern() => shootPattern; 
    public BulletType GetBulletType() => bulletType; 
    public OnExpireEffect GetOnExpireEffect() => onExpireEffect;
    public int BulletsPerShot => bulletsPerShot; 
    public int BulletSize => bulletSize; 
    public float SpreadDegrees => spreadDegrees;
    public float PatternSize => patternSize; 
    public int Ammo => ammo; 
    public int MaxAmmo => maxAmmo; 

    public Shooter(int bulletDamage = 1, int ticksPerShot = 30, float bulletSpeed = 3f, 
        int ammo = 10, int skill = 50, int bulletsPerShot = 1, ShootPattern shootPattern = ShootPattern.Default, 
        BulletType bulletType = BulletType.Default, OnExpireEffect onExpireEffect = OnExpireEffect.Default, 
        int bulletSize = Constants.BulletSize, int patternLength = 1, float patternSize = 10f, int reloadTicks = 150,
        int bulletLifetimeTicks = 120, float spreadDegrees = 10f) {
        this.bulletDamage = bulletDamage; 
        this.ticksPerShot = ticksPerShot; 
        this.bulletSpeed = bulletSpeed; 
        this.skill = skill; 
        this.ammo = ammo; 
        this.maxAmmo = ammo; 
        canShoot = new WorldTime(); 
        this.shootPattern = shootPattern; 
        this.bulletType = bulletType; 
        this.onExpireEffect = onExpireEffect; 
        this.bulletsPerShot = bulletsPerShot; 
        this.bulletSize = bulletSize; 
        this.patternLength = patternLength; 
        this.patternSize = patternSize;
        this.reloadTicks = reloadTicks;
        this.bulletLifetimeTicks = bulletLifetimeTicks;
        this.spreadDegrees = spreadDegrees;
    }

    public Bullet Shoot(WorldTime now) {
        ammo--; 
        if (ammo <= 0) {
            ammo = maxAmmo; 
            canShoot = now + new WorldTime(ticks: reloadTicks);
        } else {
            canShoot = now + new WorldTime(ticks: ticksPerShot);
        }

        return new Bullet(bulletDamage, maxFramesActive: bulletLifetimeTicks); 
    }

    public float GetBulletSpeed() => bulletSpeed; 
    public int GetBulletDamage() => bulletDamage; 
    public int Inaccuracy => 100 - skill; 

    public bool CanShoot(WorldTime t) {
        return t.IsAfterOrAt(canShoot); 
    }

    public void IncreaseDamage(int dmg) {
        bulletDamage += dmg; 
    }
}

public static class ShooterWrap {
    
    private static float[] ds = {-1f, 1f};

    private static void shootCardinal(World w, int axis, Frame f, Shooter shooter, Func<int> addBulletEnt) {
        V2 pos = new V2(f.Position);
        V2 size = new V2(new Vector2(f.GetWidth(), f.GetHeight()));
        int bullets = shooter.BulletsPerShot;

        float minBulletPos = pos[axis + 1] + (size[axis + 1] / 2) - (shooter.PatternSize / 2f);
        float center = pos[axis] + (size[axis] / 2f);
        float spacePerBullet = shooter.PatternSize / (float)(bullets - 1);

        foreach (int d in ds) {
            for (int i = 0; i < bullets; i++) {
                int bulletEnt = addBulletEnt(); 
                V2 vel = new V2(0f, 0f);
                vel[axis] = d * shooter.GetBulletSpeed();
                w.SetComponent<Velocity>(bulletEnt, new Velocity(vel));
                Frame bulletFrame = w.GetComponent<Frame>(bulletEnt);
                V2 bulletPos = new V2(0f, 0f); 
                bulletPos[axis] = center + (0.5f * size[axis] * d); 
                bulletPos[axis + 1] = minBulletPos + (spacePerBullet * i);
                bulletFrame.SetCoordinates(bulletPos);
            }
        }
    }

    public static void TryShoot(World w, Shooter shooter, Frame f, Vector2 dest, ShooterType type) {
        if (shooter.CanShoot(w.Time)) {
            Vector2 pos = f.Position; 
            
            List<int> bulletEnts = new();
            float speed = shooter.GetBulletSpeed(); 
            int inaccuracy = shooter.Inaccuracy; 
            int bullets = shooter.BulletsPerShot; 
            Velocity bulletVelocity = new Velocity(Vector2.Zero);
            
            float offset;
            int bulletEnt; 

            int addBulletEnt() {
                int bEnt = EntityFactory.AddUI(w, pos, shooter.BulletSize, 
                    shooter.BulletSize, setOutline: true);
                bulletEnts.Add(bEnt);
                return bEnt;
            }

            switch (shooter.GetShootPattern()) {
                case ShootPattern.Default: 
                    bulletEnt = addBulletEnt();

                    offset = (float)(inaccuracy * w.NextDouble()); 
                    dest += new Vector2(offset, offset); 

                    bulletVelocity = new Velocity(Vector2.Normalize(dest - pos) * speed);
                    w.SetComponent<Velocity>(bulletEnt, bulletVelocity); 
                    break;
                case ShootPattern.Multi: 
                    offset = (float)(inaccuracy * w.NextDouble()); 
                    dest += new Vector2(offset, offset); 
                    float spread = shooter.SpreadDegrees;
                    float degreesPerShot = spread / (float)(bullets - (bullets % 2));
                    float startDegree = -1 * degreesPerShot * (bullets / 2);

                    for (int i = 0; i < bullets; i++) {
                        bulletEnt = addBulletEnt();

                        Vector2 v = Vector2.Normalize(dest - pos) * speed; 
                        v = Util.Rotate(v, startDegree + (degreesPerShot * i));
                        w.SetComponent<Velocity>(bulletEnt, new Velocity(v)); 
                    }

                    break;
                case ShootPattern.HorizontalLine: 
                    shootCardinal(w, 0, f, shooter, addBulletEnt);
                    break;
                case ShootPattern.VerticalLine: 
                    shootCardinal(w, 1, f, shooter, addBulletEnt);
                    break;
                default: 
                    throw new InvalidOperationException("Undefined shoot pattern type");
            }

            
            bulletEnts.ForEach(ent => {
                switch (type) {
                    case ShooterType.Enemy: 
                        w.SetComponent<Enemy>(ent, new Enemy()); 
                        break;
                    case ShooterType.Player: 
                        w.SetComponent<Player>(ent, new Player());
                        break;
                    default: 
                        throw new InvalidOperationException("Undefined shooter type");  
                }

                w.SetComponent<Bullet>(ent, shooter.Shoot(w.Time)); 

                switch (shooter.GetBulletType()) {
                    case BulletType.Default: 
                        break; 
                    case BulletType.Homing: 
                        int targetEnt = type switch {
                            ShooterType.Enemy => PlayerWrap.GetRPGEntity(w), 
                            ShooterType.Player => EnemyWrap.GetFirst(w),
                            _ => -1
                        };
                        w.SetComponent<Homing>(ent, new Homing(targetEnt));
                        break;
                    default: 
                        throw new InvalidOperationException("Undefined bullet type");
                }
            });
        }
    }
}
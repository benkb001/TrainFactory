namespace TrainGame.Systems;

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Utils;
using TrainGame.Constants;

public class ShotBy {
    public int Entity; 
    public ShotBy(int e) {
        this.Entity = e; 
    }
}

public class BulletCreatedFlag {}

public static class ShooterWrap {
    
    //returns the velocity needed to shoot at targetPos from pos with speed speed
    public static Vector2 Aim(Vector2 pos, Vector2 targetPos, float speed) {
        return targetPos == pos ? Vector2.Zero : (Vector2.Normalize(targetPos - pos)) * speed;
    }

    public static int Add<U>(World w, Vector2 pos, Vector2 targetPos, BulletContainer bc, int shooterEnt) 
    where U : IFlag<U> {
        float width = bc.Width;
        float height = bc.Height;
        
        int e = EntityFactory.AddUI(w, pos, width, height, setOutline: true);
        w.SetComponent<Velocity>(e, new Velocity(Aim(pos, targetPos, bc.Speed)));
        w.SetComponent<Bullet>(e, bc.GetBullet());
        w.SetComponent<U>(e, U.Get());
        
        if (w.ComponentContainsEntity<ShotBy>(shooterEnt)) {
            w.SetComponent<ShotBy>(e, w.GetComponent<ShotBy>(shooterEnt));
        } else {
            w.SetComponent<ShotBy>(e, new ShotBy(shooterEnt));
        }

        w.SetComponent<BulletCreatedFlag>(e, new BulletCreatedFlag());
        w.SetComponent<Outline>(e, new Outline(Colors.GetBulletColor<U>()));

        foreach (IBulletTrait bt in bc.GetBulletTraits()) {
            BulletTraitRegistry.Add(w, bt, e);
        }

        return e; 
    }

    public static void UpgradeDamage(IShootPattern p, int dmg = 1) {
        foreach (BulletContainer bc in p.GetBulletContainers()) {
            bc.AddDamage(dmg); 
        }
    }

    public static void UpgradeBulletSize(IShootPattern sp) {
        foreach (BulletContainer bc in sp.GetBulletContainers()) {
            bc.Width += Constants.BulletSizeIncrease;
            bc.Height += Constants.BulletSizeIncrease;
        }
    }

    public static void UpgradeBulletSpeed(IShootPattern sp) {
        foreach (BulletContainer bc in sp.GetBulletContainers()) {
            bc.Speed += Constants.BulletSpeedIncrease;
        }
    }

    public static void AddExplosion(IShootPattern sp) {
        foreach (BulletContainer bc in sp.GetBulletContainers()) {
            bool hasExplosion = false; 
            foreach (IBulletTrait bt in bc.GetTraits()) {
                if (bt is Split split && split.Pattern is MeleeShootPattern msp) {
                    foreach (BulletContainer innerBC in msp.GetBulletContainers()) {
                        innerBC.AddDamage(1);
                    }
                    hasExplosion = true;
                    break;
                }
            }
            
            if (!hasExplosion) {
                bc.AddTrait(
                    new Split(
                        new MeleeShootPattern(
                            new BulletContainer(
                                new Bullet(1, maxFramesActive: 10),
                                new Frame(Constants.TileWidth * 2, Constants.TileWidth * 2)
                            )
                        )
                    )
                );
            }
        }
    }

    public static void AddHoming(IShootPattern sp) {
        foreach (BulletContainer bc in sp.GetBulletContainers()) {
            if (!bc.GetTraits().Any(t => t.GetType() == typeof(Homing))) {
                bc.AddTrait(new Homing(Speed: bc.Speed));
            }
        }
    }

    public static void AddKnockback(IShootPattern sp) {

        foreach (BulletContainer bc in sp.GetBulletContainers()) {
            bool hasKnockback = false;
            foreach (IBulletTrait t in bc.GetTraits()) {
                if (t is AppliesKnockback applies) {
                    applies.Multiplier += 1f; 
                    hasKnockback = true;
                }
            }
            if (!hasKnockback) {
                bc.AddTrait(new AppliesKnockback());
            }
        }
    }

    public static void UpgradeUnloadSpeed(Shooter s) {
        s.TimeBetweenShots -= Constants.TicksBetweenShotDecrement;
        if (s.TimeBetweenShots.InTicks() < 1) {
            s.TimeBetweenShots = new WorldTime(ticks: 1);
        }
    }

    public static void UpgradeReloadSpeed(Shooter s) {
        s.ReloadTime -= Constants.ReloadTicksDecrement;
        if (s.ReloadTime.InTicks() < 1) {
            s.ReloadTime = new WorldTime(ticks: 1);
        }
    }

    public static void UpgradeMaxAmmo(Shooter s) {
        s.MaxAmmo++;
    }
}
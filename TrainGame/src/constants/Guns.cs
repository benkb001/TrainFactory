namespace TrainGame.Constants;

using System;
using System.Collections.Generic;
using TrainGame.Components;

public class PlayerGun {

    private IShootPattern shootPattern;
    private Shooter shooter;
    public IShootPattern GetShootPattern() => shootPattern;
    public Shooter GetShooter() => shooter; 

    public PlayerGun(Shooter shooter, IShootPattern shootPattern) {
        this.shooter = shooter; 
        this.shootPattern = shootPattern;
    }
}

public static class Weapons {
    public static Dictionary<string, PlayerGun> PlayerGunMap = new() {
        [ItemID.Gun] = new PlayerGun(
            new Shooter(
                ammo: 8, 
                ticksPerShot: 15, 
                reloadTicks: 120
            ), 
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(1),
                    BulletSpeed: 4f,
                    traits: new List<IBulletTrait>(){
                        new RemoveOnCollision()
                    }
                )
            )
        ),
        [ItemID.Gun2] = new PlayerGun(
            new Shooter(
                ammo: 16, 
                ticksPerShot: 15, 
                reloadTicks: 30
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(1),
                    BulletSpeed: 6f,
                    traits: new List<IBulletTrait>(){
                        new RemoveOnCollision()
                    }
                )
            )
        ),
        [ItemID.Gun3] = new PlayerGun(
            new Shooter(
                ammo: 32, 
                ticksPerShot: 8, 
                reloadTicks: 16
            ),
            new ShotgunShootPattern(
                new BulletContainer(
                    new Bullet(1),
                    traits: new List<IBulletTrait>(){
                        new RemoveOnCollision()
                    },
                    BulletSpeed: 8f
                ),
                2,
                Math.PI / 20
            )
        )
    };
}
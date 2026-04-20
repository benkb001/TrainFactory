namespace TrainGame.Constants;

using System;
using System.Collections.Generic;
using TrainGame.Components;
using TrainGame.Utils;

public static class EquipmentID {
    public static List<string> Armor = new() {
        ItemID.Armor1, ItemID.Armor2, ItemID.Armor3
    };

    public static void InitMaps() {
        EquipmentSlot<PlayerGun>.EquipmentMap = new() {
            [""] = new PlayerGun(
                new Shooter(
                    ammo: 0
                ),
                new DefaultShootPattern(
                    new BulletContainer(
                        new Bullet(0)
                    )
                )
            ),
            [ItemID.Pistol] = new PlayerGun(
                new Shooter(
                    ammo: 8, 
                    ticksPerShot: 15, 
                    reloadTicks: 80
                ), 
                new DefaultShootPattern(
                    new BulletContainer(
                        new Bullet(2, maxFramesActive: 45),
                        BulletSpeed: 6f,
                        traits: new List<IBulletTrait>(){
                            new RemoveOnCollision()
                        }
                    ),
                    Inaccuracy: 45f
                )
            ),
            [ItemID.Ring] = new PlayerGun(
                new Shooter(
                    ammo: 5,
                    ticksPerShot: 20,
                    reloadTicks: 60
                ),
                new RadialShootPattern(
                    BulletsPerShot: 16,
                    new BulletContainer(
                        new Bullet(1, maxFramesActive: 100),
                        BulletSpeed: 4f,
                        traits: new List<IBulletTrait>(){
                            new RemoveOnCollision()
                        }
                    )
                )
            ),
            [ItemID.Shotgun] = new PlayerGun(
                new Shooter(
                    ammo: 2,
                    ticksPerShot: 60,
                    reloadTicks: 180
                ),
                new RandomShotgunShootPattern(
                    new BulletContainer(
                        new Bullet(1, maxFramesActive: 120),
                        new Frame(Constants.TileWidth, Constants.TileWidth),
                        BulletSpeed: 2f,
                        traits: new List<IBulletTrait>(){
                            new RemoveOnCollision(),
                            new Split(
                                new MeleeShootPattern(
                                    new BulletContainer(
                                        new Bullet(1, maxFramesActive: 10),
                                        new Frame(Constants.TileWidth * 2, Constants.TileWidth * 2)
                                    )
                                )
                            )
                        }
                    ),
                    new BulletContainer(
                        new Bullet(1, maxFramesActive: 60),
                        BulletSpeed: 5f,
                        traits: new List<IBulletTrait>(){
                            new RemoveOnCollision()
                        }
                    ),
                    Math.PI / 5d,
                    1,
                    5
                )
            )
        };
    }
}
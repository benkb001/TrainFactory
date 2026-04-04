namespace TrainGame.Constants;

using System;
using System.Collections.Generic;
using TrainGame.Components;

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
                    reloadTicks: 120
                ), 
                new DefaultShootPattern(
                    new BulletContainer(
                        new Bullet(2, maxFramesActive: 30),
                        BulletSpeed: 6f,
                        traits: new List<IBulletTrait>(){
                            new RemoveOnCollision()
                        }
                    ),
                    Inaccuracy: 45f
                )
            )
        };
    }
}
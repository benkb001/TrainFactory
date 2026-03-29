namespace TrainGame.Constants;

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TrainGame.Components;
using TrainGame.Utils;

public static class EnemyID {
    public static readonly Dictionary<EnemyType, EnemyConst> Enemies = new() {
        [EnemyType.Artillery] = new EnemyConst(
            new Shooter(
                ammo: 4, 
                ticksPerShot: 100, 
                reloadTicks: 300
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(15, maxFramesActive: 600),
                    traits: new List<IBulletTrait>(){
                        new Homing()
                    }
                )
            ), 
            new DefaultMovePattern(
                ticksToMove: 60, 
                ticksToWait: 60,
                speed: 0.5f
            ),
            Type: EnemyType.Artillery, 
            HP: 15, 
            Size: Constants.TileWidth * 2, 
            Difficulty: 2
        ),
        [EnemyType.Barbarian] = new EnemyConst(
            new Shooter(
                ticksPerShot: 200, 
                reloadTicks: 200, 
                ammo: 1
            ),
            new MeleeShootPattern(
                new BulletContainer(
                    new Bullet(1, maxFramesActive: 10),
                    width: Constants.TileWidth * 3,
                    traits: new List<IBulletTrait>(){
                        new Warned(new WorldTime(ticks: 45)),
                        new RemoveOnCollision()
                    }
                )
            ), 
            new DefaultMovePattern(
                ticksToMove: 60,
                ticksToWait: 200,
                speed: Constants.PlayerSpeed / 3f
            ),
            Type: EnemyType.Barbarian, 
            HP: 20, 
            Size: Constants.TileWidth,
            Difficulty: 2
        ),
        [EnemyType.Default] = new EnemyConst(
            new Shooter(),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(5),
                    traits: new List<IBulletTrait>(){
                        new RemoveOnCollision()
                    }
                )
            ),
            new DefaultMovePattern()
        ),
        [EnemyType.MachineGun] = new EnemyConst(
            new Shooter(
                ammo: 36, 
                reloadTicks: 120
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(15),
                    traits: new List<IBulletTrait>(){
                        new RemoveOnCollision()
                    }
                ),
                Inaccuracy: 10
            ),
            new ChaseMovePattern(
                Constants.PlayerSpeed / 2f
            ),
            Type: EnemyType.MachineGun,
            HP: 12
        ),
        [EnemyType.Ninja] = new EnemyConst(
            new Shooter(
                ticksPerShot: 10, 
                ammo: 2, 
                reloadTicks: 120
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(5),
                    traits: new List<IBulletTrait>(){
                        new RemoveOnCollision()
                    }
                ),
                Inaccuracy: 10
            ),
            new ChaseMovePattern(
                Constants.PlayerSpeed / 1.5f
            ),
            Type: EnemyType.Ninja, 
            HP: 6
        ),
        [EnemyType.Robot] = new EnemyConst(
            new Shooter(
                ammo: 32, 
                ticksPerShot: 4, 
                reloadTicks: 60
            ),
            new RadialShootPattern(
                2,
                new BulletContainer(
                    new Bullet(10),
                    traits: new List<IBulletTrait>(){
                        new RemoveOnCollision()
                    }
                ), 
                Math.PI / 2
            ),
            new CyclicalMovePattern(
                new List<Vector2>() { new Vector2(1, 0), new Vector2(-1, 0) },
                new List<WorldTime>() { new WorldTime(ticks: 60 ) },
                new WorldTime(ticks: 360),
                1f
            ),
            Type: EnemyType.Robot, 
            HP: 8
        ),
        [EnemyType.Shotgun] = new EnemyConst(
            new Shooter(
                ammo: 12, 
                ticksPerShot: 120, 
                reloadTicks: 240
            ),
            new ShotgunShootPattern(
                new BulletContainer(
                    new Bullet(10, maxFramesActive: 180),
                    traits: new List<IBulletTrait>(){
                        new RemoveOnCollision()
                    }
                ),
                4,
                Math.PI / 5
            ),
            new DefaultMovePattern(),
            Type: EnemyType.Shotgun, 
            HP: 8
        ),
        [EnemyType.Sniper] = new EnemyConst(
            new Shooter(
                ammo: 1, 
                reloadTicks: 300
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(40, maxFramesActive: 240),
                    BulletSpeed: 10f,
                    traits: new List<IBulletTrait>(){
                        new Warned(new WorldTime(ticks: 20)),
                        new RemoveOnCollision()
                    }
                )
            ),
            new DefaultMovePattern(),
            Type: EnemyType.Sniper, 
            HP: 25
        ),
        [EnemyType.Volley] = new EnemyConst(
            new Shooter(
                ammo: 24, 
                reloadTicks: 200, 
                ticksPerShot: 60
            ),
            new ShotgunShootPattern(
                new BulletContainer(
                    new Bullet(40, maxFramesActive: 60),
                    BulletSpeed: 3f,
                    traits: new List<IBulletTrait>(){
                        new RemoveOnCollision()
                    }
                ),
                12,
                Math.PI / 1.5
            ),
            new DefaultMovePattern(),
            Type: EnemyType.Volley, 
            HP: 25
        ),
        [EnemyType.Vampire] = new EnemyConst(
            new Shooter(
                ammo: 1,
                reloadTicks: 600
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(10, maxFramesActive: 600),
                    width: Constants.TileWidth / 2f,
                    traits: new List<IBulletTrait>(){
                        new RemoveOnCollision(),
                        new Vampiric(5),
                        new Homing(Speed: Constants.PlayerSpeed / 1.2f)
                    }
                )
            ),
            new CyclicalMovePattern(
                new List<Vector2>(){
                    new Vector2(0.5f, -0.86f),
                    new Vector2(0.5f, 0.86f),
                    new Vector2(-1f, 0f)
                }, 
                new List<WorldTime>(){
                    new WorldTime(ticks: 10), 
                    new WorldTime(ticks: 60),
                    new WorldTime(ticks: 10)
                },
                new WorldTime(ticks: 10),
                Speed: 8f
            ),
            Type: EnemyType.Vampire,
            HP: 100,
            Size: Constants.TileWidth * 1.5f
        ),
        [EnemyType.Warrior] = new EnemyConst(
            new Shooter(
                ammo: 20, 
                reloadTicks: 480
            ),
            new ShotgunShootPattern(
                new BulletContainer(
                    new Bullet(60, maxFramesActive: 240),
                    BulletSpeed: 4f,
                    traits: new List<IBulletTrait>(){
                        new Warned(new WorldTime(ticks: 30)),
                        new RemoveOnCollision()
                    }
                ),
                20, 
                Math.PI
            ),
            new DefaultMovePattern(),
            Type: EnemyType.Warrior, 
            HP: 40,
            Size: Constants.TileWidth * 2
        ),
        [EnemyType.Wizard] = new EnemyConst(
            new Shooter(
                ammo: 10,
                ticksPerShot: 12,
                reloadTicks: 120
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(60, maxFramesActive: 3600),
                    traits: new List<IBulletTrait>(){
                        new ParametricCurve(
                            //spiral
                            (t) => {
                                t = t + 1; 
                                float r = 10 + (t * 0.3f);
                                float theta = MathF.Log(1 + t*5) * 5.0f;
                                return (float)(r * MathF.Cos(theta));
                            },
                            (t) => {
                                t = t + 1; 
                                float r = 10 + (t * 0.3f);
                                float theta =  MathF.Log(1 + t*5) * 5.0f;
                                return (float)(r * MathF.Sin(theta));
                            }
                        ),
                        new RemoveOnCollision()
                    }
                )
            ),
            new DefaultMovePattern(0),
            Type: EnemyType.Wizard,
            HP: 40,
            Size: Constants.TileWidth
        )
    };
}
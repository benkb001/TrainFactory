namespace TrainGame.Constants;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TrainGame.Components;
using TrainGame.Utils;

public enum EnemyType {
    Artillery, //Big, Shoots vertically, homing bullets
    Barbarian, //Melee attacks around it
    Default,
    ExplodeOnDeath,
    Fractal, //Fractal random splitting
    MachineGun, //Shoots a lot of bullets
    Ninja, //Dashes around, shoots occasionally
    Robot, //moves left to right and shoots up/down in bursts
    Shotgun, //Shoots in a small spread
    Skeleton, //Spawns a moving grid 
    Sniper, //bullets are warned, travel far and fast and hit hard 
    Splitter, //bullets split when they collide
    Square, //shoots a bunch of bullets that form a square 
    Tripples, //1->3->9
    Vampire, //player gets slowly damaged until vampire is killed
    Volley, //Shoots in a large spread
    Warrior, //Shoots in a very wide spread, has high hp and damage, high reload time
    Witch, //Shoots in a little circle that lasts a while
    Wizard //Shoots in a spiral pattern 
}

public static class EnemyID {
    private static Frame bFrame() => new Frame(Constants.DefaultBulletSize, Constants.DefaultBulletSize);

    public static readonly Dictionary<EnemyType, EnemyConst> Enemies = new() {
        [EnemyType.Artillery] = new EnemyConst(
            new Shooter(
                ammo: 1, 
                ticksPerShot: 300, 
                reloadTicks: 300
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(10, maxFramesActive: 600),
                    bFrame(),
                    traits: new List<IBulletTrait>(){
                        new Homing(Speed: Constants.PlayerSpeed / 2f),
                        new RemoveOnCollision()
                    }
                )
            ), 
            new ChaseMovePattern(Speed: 2f, SecondsToChase: 3),
            Type: EnemyType.Artillery, 
            HP: 16, 
            Size: Constants.TileWidth * 2,
            Difficulty: 3
        ),
        [EnemyType.Barbarian] = new EnemyConst(
            new Shooter(
                ticksPerShot: 120, 
                reloadTicks: 120, 
                ammo: 1
            ),
            new MeleeShootPattern(
                new BulletContainer(
                    new Bullet(10, maxFramesActive: 10),
                    new Frame(Constants.TileWidth * 4, Constants.TileWidth * 4),
                    traits: new List<IBulletTrait>(){
                        new Warned(new WorldTime(ticks: 45)),
                        new RemoveOnCollision()
                    }
                )
            ), 
            new ChaseMovePattern(
                Constants.PlayerSpeed / 1.25f,
                SecondsToChase: 8
            ),
            Type: EnemyType.Barbarian, 
            HP: 15, 
            Size: Constants.TileWidth,
            Difficulty: 2
        ),
        [EnemyType.Default] = new EnemyConst(
            new Shooter(),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(5),
                    bFrame(),
                    traits: new List<IBulletTrait>(){
                        new RemoveOnCollision()
                    }
                )
            ),
            new ChaseMovePattern(
                Constants.PlayerSpeed / 2f
            ),
            Type: EnemyType.Default,
            Difficulty: 1
        ),
        [EnemyType.ExplodeOnDeath] = new EnemyConst(
            new Shooter(
                ammo: 3,
                reloadTicks: 240,
                ticksPerShot: 40
            ),
            new RandomShotgunShootPattern(
                new BulletContainer(
                    new Bullet(20, maxFramesActive: 180),
                    new Frame(Constants.TileWidth, Constants.TileWidth),
                    BulletSpeed: 1f,
                    traits: new List<IBulletTrait>(){
                        new Split(
                            new MeleeShootPattern(
                                new BulletContainer(
                                    new Bullet(20),
                                    new Frame(Constants.TileWidth * 2, Constants.TileWidth * 2),
                                    traits: new List<IBulletTrait>(){
                                        new Warned(new WorldTime(ticks: 15))
                                    }
                                )
                            )
                        )
                    }
                ),
                new BulletContainer(
                    new Bullet(20, maxFramesActive: 140),
                    BulletSpeed: 4f
                ),
                Math.PI / 8,
                1,
                7
            ),
            new ChaseMovePattern(
                Constants.PlayerSpeed / 2f
            ),
            Type: EnemyType.ExplodeOnDeath,
            HP: 25,
            traits: new List<IEnemyTrait>(){
                new Split(
                    new MeleeShootPattern(
                        new BulletContainer(
                            new Bullet(20),
                            new Frame(Constants.TileWidth * 2, Constants.TileWidth * 2),
                            traits: new List<IBulletTrait>(){
                                new Warned(new WorldTime(ticks: 30))
                            }
                        )
                    )
                )
            },
            Difficulty: 6
        ),
        [EnemyType.Fractal] = new EnemyConst(
            new Shooter(
                ammo: 4,
                reloadTicks: 60,
                ticksPerShot: 10
            ),
            new RadialShootPattern(
                2,
                new BulletContainer(
                    new Bullet(35, maxFramesActive: 15),
                    bFrame(),
                    BulletSpeed: Constants.PlayerSpeed / 1.25f,
                    traits: new List<IBulletTrait>(){
                        new Fractal(0.01f, 0.1f),
                        new RemoveOnCollision(),
                        new Split(
                            new ShotgunShootPattern(
                                new BulletContainer(
                                    new Bullet(35, maxFramesActive: 15),
                                    bFrame(),
                                    BulletSpeed: Constants.PlayerSpeed / 1.25f,
                                    traits: new List<IBulletTrait>(){
                                        new RemoveOnCollision(),
                                    }
                                ),
                                2,
                                Math.PI / 5
                            )
                        )
                    }
                ),
                Math.PI / 2
            ),
            new ChaseMovePattern(Constants.PlayerSpeed / 2f),
            Type: EnemyType.Fractal,
            HP: 35,
            Difficulty: 8,
            Size: Constants.TileWidth * 2f
        ),
        [EnemyType.MachineGun] = new EnemyConst(
            new Shooter(
                ammo: 36, 
                reloadTicks: 120,
                ticksPerShot: 10
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(15),
                    bFrame(),
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
            HP: 20,
            Difficulty: 4
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
                    bFrame(),
                    traits: new List<IBulletTrait>(){
                        new RemoveOnCollision()
                    }
                ),
                Inaccuracy: 10
            ),
            new ChaseMovePattern(
                Constants.PlayerSpeed / 2.5f,
                SecondsToChase: 4
            ),
            Type: EnemyType.Ninja, 
            HP: 12,
            Difficulty: 2
        ),
        [EnemyType.Robot] = new EnemyConst(
            new Shooter(
                ammo: 8, 
                ticksPerShot: 8, 
                reloadTicks: 60
            ),
            new RadialShootPattern(
                2,
                new BulletContainer(
                    new Bullet(10, maxFramesActive: 60),
                    bFrame(),
                    BulletSpeed: 8f,
                    traits: new List<IBulletTrait>(){
                        new RemoveOnCollision()
                    }
                ), 
                Math.PI / 2
            ),
            new CyclicalMovePattern(
                new List<Vector2>() { new Vector2(1, 0), new Vector2(-1, 0) },
                new List<WorldTime>() { new WorldTime(ticks: 60 ) },
                new WorldTime(ticks: 300),
                1f
            ),
            Type: EnemyType.Robot, 
            HP: 16,
            Difficulty: 3
        ),
        /*
        [EnemyType.Skeleton] = new EnemyConst(
            new Shooter(
                ammo: 1,
                ticksPerShot: 240,
                reloadTicks: 720
            ),
            new GridShootPattern(
                new BulletContainer(
                    new Bullet(50, maxFramesActive: 10),
                    new Frame(Constants.TileWidth / 4f, Constants.TileWidth * 25),
                    BulletSpeed: 0f,
                    traits: new List<IBulletTrait>(){
                        new Warned(new WorldTime(ticks: 120))
                    }
                ),
                new BulletContainer(
                    new Bullet(50, maxFramesActive: 10),
                    new Frame(Constants.TileWidth * 25, Constants.TileWidth / 4f),
                    BulletSpeed: 0f,
                    traits: new List<IBulletTrait>(){
                        new Warned(new WorldTime(ticks: 120))
                    }
                ),
                Constants.TileWidth * 2.5f,
                Constants.TileWidth * 2.5f,
                10,
                5,
                new Vector2(Constants.TileWidth / 2, 0),
                3
            ),
            new DefaultMovePattern(speed: 0f),
            Type: EnemyType.Skeleton,
            HP: 50,
            Difficulty: 11
        ),
        */
        [EnemyType.Shotgun] = new EnemyConst(
            new Shooter(
                ammo: 3, 
                ticksPerShot: 120, 
                reloadTicks: 240
            ),
            new ShotgunShootPattern(
                new BulletContainer(
                    new Bullet(10, maxFramesActive: 90),
                    bFrame(),
                    traits: new List<IBulletTrait>(){
                        new RemoveOnCollision()
                    }
                ),
                4,
                Math.PI / 5
            ),
            new ChaseMovePattern(
                Constants.PlayerSpeed / 2f
            ),
            Type: EnemyType.Shotgun, 
            HP: 12,
            Difficulty: 2
        ),
        [EnemyType.Sniper] = new EnemyConst(
            new Shooter(
                ammo: 1, 
                reloadTicks: 300
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(40, maxFramesActive: 240),
                    bFrame(),
                    BulletSpeed: 10f,
                    traits: new List<IBulletTrait>(){
                        new Warned(new WorldTime(ticks: 20)),
                        new RemoveOnCollision()
                    }
                )
            ),
            new ChaseMovePattern(
                Constants.PlayerSpeed / 2f
            ),
            Type: EnemyType.Sniper, 
            HP: 40,
            Difficulty: 9
        ),
        [EnemyType.Splitter] = new EnemyConst(
            new Shooter(
                ammo: 2,
                reloadTicks: 600,
                ticksPerShot: 60
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(40, maxFramesActive: 60),
                    new Frame(Constants.TileWidth / 1.25f, Constants.TileWidth / 1.25f),
                    BulletSpeed: Constants.TileWidth / 5f,
                    traits: new List<IBulletTrait>(){
                        new Warned(new WorldTime(ticks: 45)),
                        new RemoveOnCollision(),
                        new Split(
                            new RadialShootPattern(
                                20,
                                new BulletContainer(
                                    new Bullet(40, maxFramesActive: 60),
                                    new Frame(Constants.TileWidth / 8f, Constants.TileWidth / 8f),
                                    BulletSpeed: Constants.TileWidth / 5f,
                                    traits: new List<IBulletTrait>(){
                                        new RemoveOnCollision()
                                    }
                                )
                            )
                        )
                    }
                )
            ),
            new ChaseMovePattern(
                Constants.PlayerSpeed / 2f
            ),
            Type: EnemyType.Splitter,
            HP: 40,
            Difficulty: 10
        ),
        [EnemyType.Square] = new EnemyConst(
            new Shooter(
                ammo: 4,
                reloadTicks: 180,
                ticksPerShot: 45
            ),
            new ShapeShootPattern(
                new BulletContainer(
                    new Bullet(20),
                    bFrame(),
                    BulletSpeed: Constants.PlayerSpeed / 1.25f,
                    traits: new List<IBulletTrait>(){
                        new RemoveOnCollision(), new Warned(new WorldTime(ticks: 30))
                    }
                ),
                new ParametricCurve(
                    (t) => {
                        float w = Constants.PlayerWidth * 2.5f;
                        if (t < 25) {
                            return ((float)t / 25f) * w;
                        } else if (t < 50) {
                            return w;
                        } else if (t < 75) {
                            return w - (((float)(t - 50) / 25f) * w); 
                        } else {
                            return 0f;
                        }
                    },
                    (t) => {
                        float w = Constants.PlayerWidth * 2.5f;
                        if (t < 25) {
                            return 0f; 
                        } else if (t < 50) {
                            return ((t - 25) / 25f) * w; 
                        } else if (t < 75) {
                            return w; 
                        } else {
                            return w - (((t - 75) / 25f) * w);
                        }
                    },
                    100
                ),
                20
            ),
            new ChaseMovePattern(
                Constants.PlayerSpeed / 2f
            ),
            Type: EnemyType.Square,
            HP: 25,
            Difficulty: 5
        ),
        [EnemyType.Tripples] = new EnemyConst(
            new Shooter(
                ammo: 3,
                ticksPerShot: 120,
                reloadTicks: 240
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(30, maxFramesActive: 40),
                    new Frame(Constants.TileWidth / 2f, Constants.TileWidth / 2f),
                    BulletSpeed: Constants.PlayerSpeed / 1.1f,
                    traits: new List<IBulletTrait>(){
                        new Split(
                            new ShotgunShootPattern(
                                new BulletContainer(
                                    new Bullet(30, maxFramesActive: 40),
                                    new Frame(Constants.TileWidth / 4f, Constants.TileWidth / 4f),
                                    BulletSpeed: Constants.PlayerSpeed / 1.1f,
                                    traits: new List<IBulletTrait>(){
                                        new Split(
                                            new ShotgunShootPattern(
                                                new BulletContainer(
                                                    new Bullet(30, maxFramesActive: 40),
                                                    bFrame(),
                                                    BulletSpeed: Constants.PlayerSpeed / 1.1f,
                                                    traits: new List<IBulletTrait>(){
                                                        new RemoveOnCollision()
                                                    }
                                                ),
                                                3,
                                                Math.PI / 4
                                            )
                                        )
                                    }
                                ),
                                3,
                                Math.PI / 2
                            )
                        )
                    }
                )
            ),
            new ChaseMovePattern(
                Constants.PlayerSpeed / 2f
            ),
            Type: EnemyType.Tripples,
            HP: 30,
            Difficulty: 7
        ),
        [EnemyType.Volley] = new EnemyConst(
            new Shooter(
                ammo: 24, 
                reloadTicks: 200, 
                ticksPerShot: 60
            ),
            new ShotgunShootPattern(
                new BulletContainer(
                    new Bullet(30, maxFramesActive: 60),
                    bFrame(),
                    BulletSpeed: 3f,
                    traits: new List<IBulletTrait>(){
                        new RemoveOnCollision()
                    }
                ),
                12,
                Math.PI / 1.5
            ),
            new ChaseMovePattern(
                Constants.PlayerSpeed / 2f
            ),
            Type: EnemyType.Volley, 
            HP: 30,
            Difficulty: 7
        ),
        [EnemyType.Vampire] = new EnemyConst(
            new Shooter(
                ammo: 1,
                reloadTicks: 600
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(60, maxFramesActive: 600),
                    new Frame(Constants.TileWidth / 2f),
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
            HP: 60,
            Size: Constants.TileWidth * 1.5f,
            Difficulty: 12
        ),
        [EnemyType.Warrior] = new EnemyConst(
            new Shooter(
                ammo: 3, 
                reloadTicks: 240,
                ticksPerShot: 20
            ),
            new ShotgunShootPattern(
                new BulletContainer(
                    new Bullet(40, maxFramesActive: 60),
                    bFrame(),
                    BulletSpeed: Constants.PlayerSpeed / 1.5f,
                    traits: new List<IBulletTrait>(){
                        new Warned(new WorldTime(ticks: 45)),
                        new RemoveOnCollision()
                    }
                ),
                20, 
                Math.PI
            ),
            new ChaseMovePattern(
                Constants.PlayerSpeed / 2f
            ),
            Type: EnemyType.Warrior, 
            HP: 60,
            Size: Constants.TileWidth * 2,
            Difficulty: 11
        ),
        [EnemyType.Witch] = new EnemyConst(
            new Shooter(
                ammo: 1,
                ticksPerShot: 600,
                reloadTicks: 600
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(20, maxFramesActive: 600),
                    bFrame(),
                    traits: new List<IBulletTrait>(){
                        new RemoveOnCollision(),
                        new ParametricCurve(
                            (t) => {
                                float radius = Constants.TileWidth / 2f; 
                                float theta = (float)t / 10f;
                                return ((float)(radius * MathF.Cos(theta)), (float)(radius * MathF.Sin(theta)));
                            }
                        )
                    }
                )
            ),
            new ChaseMovePattern(Constants.PlayerSpeed / 2f),
            Type: EnemyType.Witch,
            HP: 30,
            Difficulty: 6
        ),
        [EnemyType.Wizard] = new EnemyConst(
            new Shooter(
                ammo: 10,
                ticksPerShot: 12,
                reloadTicks: 120
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(35, maxFramesActive: 600),
                    bFrame(),
                    traits: new List<IBulletTrait>(){
                        new ParametricCurve(
                            //spiral
                            (t) => {
                                t = t + 1; 
                                float r = 20 + (t * 0.6f);
                                float theta = MathF.Log(1 + t*5) * 5.0f;
                                return (float)(r * MathF.Cos(theta));
                            },
                            (t) => {
                                t = t + 1; 
                                float r = 20 + (t * 0.6f);
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
            HP: 35,
            Size: Constants.TileWidth,
            Difficulty: 8
        )
    };

    private static Dictionary<int, List<EnemyType>> getWithDifficulty() {
        Dictionary<int, List<EnemyType>> difficulties = new(); 
        foreach (EnemyConst e in Enemies.Values.ToList()) {
            if (!difficulties.ContainsKey(e.Difficulty)) {
                difficulties[e.Difficulty] = new List<EnemyType>(); 
            }
            difficulties[e.Difficulty].Add(e.Type); 
        }
        return difficulties;
    }

    private static Dictionary<int, List<EnemyType>> withDifficulty = getWithDifficulty();

    public static EnemyType GetRandomWithDifficulty(int d) {
        if (!withDifficulty.ContainsKey(d)) {
            Console.WriteLine($"difficulty {d} not specified, defaulting to 12");
            d = 12; 
        }

        List<EnemyType> withD = withDifficulty[d]; 
        return withD[Util.NextInt(withD.Count)];
    }
}
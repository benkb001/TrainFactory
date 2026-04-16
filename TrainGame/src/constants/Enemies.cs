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
    MachineGun, //Shoots a lot of bullets
    Ninja, //Dashes around, shoots occasionally
    Robot, //moves left to right and shoots up/down in bursts
    Shotgun, //Shoots in a small spread
    Skeleton, //Spawns a moving grid 
    Sniper, //bullets are warned, travel far and fast and hit hard 
    Splitter, //bullets split when they collide
    Vampire, //player gets slowly damaged until vampire is killed
    Volley, //Shoots in a large spread
    Warrior, //Shoots in a very wide spread, has high hp and damage, high reload time
    Wizard //Shoots in a parametric curve 
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
                    new Bullet(15, maxFramesActive: 600),
                    bFrame(),
                    traits: new List<IBulletTrait>(){
                        new Homing(Speed: Constants.PlayerSpeed / 4f),
                        new RemoveOnCollision()
                    }
                )
            ), 
            new ChaseMovePattern(Speed: 0.5f, SecondsToChase: 3),
            Type: EnemyType.Artillery, 
            HP: 12, 
            Size: Constants.TileWidth * 2,
            Difficulty: 2,
            Dist: new LootDistribution(
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 4,
                    [ItemID.Cobalt] = 60
                },
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 10,
                    [ItemID.Cobalt] = 5
                }
            )
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
                    new Frame(Constants.TileWidth * 2, Constants.TileWidth * 2),
                    traits: new List<IBulletTrait>(){
                        new Warned(new WorldTime(ticks: 45)),
                        new RemoveOnCollision()
                    }
                )
            ), 
            new ChaseMovePattern(
                Constants.PlayerSpeed / 2f
            ),
            Type: EnemyType.Barbarian, 
            HP: 15, 
            Size: Constants.TileWidth,
            Difficulty: 2,
            Dist: new LootDistribution(
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 1
                },
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 15
                }
            )
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
            Difficulty: 1,
            Dist: new LootDistribution(
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 90,
                    [ItemID.Cobalt] = 10
                },
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 5,
                    [ItemID.Cobalt] = 5
                }
            )
        ),
        [EnemyType.ExplodeOnDeath] = new EnemyConst(
            new Shooter(
                ammo: 24,
                reloadTicks: 240,
                ticksPerShot: 40
            ),
            new RandomShotgunShootPattern(
                new BulletContainer(
                    new Bullet(50, maxFramesActive: 180),
                    new Frame(Constants.TileWidth, Constants.TileWidth),
                    BulletSpeed: 1f,
                    traits: new List<IBulletTrait>(){
                        new Split(
                            new MeleeShootPattern(
                                new BulletContainer(
                                    new Bullet(50),
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
            HP: 15,
            traits: new List<IEnemyTrait>(){
                new Split(
                    new MeleeShootPattern(
                        new BulletContainer(
                            new Bullet(50),
                            new Frame(Constants.TileWidth * 2, Constants.TileWidth * 2),
                            traits: new List<IBulletTrait>(){
                                new Warned(new WorldTime(ticks: 30))
                            }
                        )
                    )
                )
            },
            Difficulty: 5,
            Dist: new LootDistribution(
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 30,
                    [ItemID.Cobalt] = 30,
                    [ItemID.Mythril] = 40
                },
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 80,
                    [ItemID.Cobalt] = 40,
                    [ItemID.Mythril] = 20
                }
            )
        ),
        [EnemyType.MachineGun] = new EnemyConst(
            new Shooter(
                ammo: 36, 
                reloadTicks: 120
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
            HP: 12,
            Difficulty: 3,
            Dist: new LootDistribution(
                new Dictionary<string, int>(){
                    [ItemID.Cobalt] = 1
                },
                new Dictionary<string, int>(){
                    [ItemID.Cobalt] = 10
                }
            )
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
            HP: 6,
            Difficulty: 1,
            Dist: new LootDistribution(
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 80,
                    [ItemID.Cobalt] = 20
                },
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 15,
                    [ItemID.Cobalt] = 5
                }
            )
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
            HP: 8,
            Difficulty: 2,
            Dist: new LootDistribution(
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 70,
                    [ItemID.Cobalt] = 30
                },
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 15,
                    [ItemID.Cobalt] = 10
                }
            )
        ),
        [EnemyType.Skeleton] = new EnemyConst(
            new Shooter(
                ammo: 45,
                ticksPerShot: 240,
                reloadTicks: 720
            ),
            new GridShootPattern(
                new BulletContainer(
                    new Bullet(10, maxFramesActive: 10),
                    new Frame(Constants.TileWidth / 4f, Constants.TileWidth * 25),
                    BulletSpeed: 0f,
                    traits: new List<IBulletTrait>(){
                        new Warned(new WorldTime(ticks: 120))
                    }
                ),
                new BulletContainer(
                    new Bullet(10, maxFramesActive: 10),
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
            HP: 20,
            Difficulty: 5,
            Dist: new LootDistribution(
                new Dictionary<string, int>(){
                    [ItemID.Cobalt] = 30,
                    [ItemID.Mythril] = 70
                },
                new Dictionary<string, int>(){
                    [ItemID.Cobalt] = 50,
                    [ItemID.Mythril] = 30
                }
            )
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
            HP: 8,
            Difficulty: 1,
            Dist: new LootDistribution(
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 85,
                    [ItemID.Cobalt] = 15
                },
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 5,
                    [ItemID.Cobalt] = 5
                }
            )
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
            HP: 15,
            Difficulty: 3,
            Dist: new LootDistribution(
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 50,
                    [ItemID.Cobalt] = 50
                },
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 15,
                    [ItemID.Cobalt] = 10
                }
            )
        ),
        [EnemyType.Splitter] = new EnemyConst(
            new Shooter(
                ammo: 1,
                reloadTicks: 700,
                ticksPerShot: 700
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(50, maxFramesActive: 120),
                    new Frame(Constants.TileWidth / 1.25f, Constants.TileWidth / 1.25f),
                    BulletSpeed: Constants.TileWidth / 10f,
                    traits: new List<IBulletTrait>(){
                        new Warned(new WorldTime(ticks: 45)),
                        new RemoveOnCollision(),
                        new Split(
                            new RadialShootPattern(
                                20,
                                new BulletContainer(
                                    new Bullet(25, maxFramesActive: 300),
                                    new Frame(Constants.TileWidth / 8f, Constants.TileWidth / 8f),
                                    BulletSpeed: Constants.TileWidth / 10f,
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
            HP: 20,
            Difficulty: 4,
            Dist: new LootDistribution(
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 50,
                    [ItemID.Cobalt] = 50
                },
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 30,
                    [ItemID.Cobalt] = 15
                }
            )
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
            HP: 15,
            Difficulty: 4,
            Dist: new LootDistribution(
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 70,
                    [ItemID.Cobalt] = 30               
                },
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 50,
                    [ItemID.Cobalt] = 30
                }
            )
        ),
        [EnemyType.Vampire] = new EnemyConst(
            new Shooter(
                ammo: 1,
                reloadTicks: 600
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(10, maxFramesActive: 600),
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
            HP: 20,
            Size: Constants.TileWidth * 1.5f,
            Difficulty: 5,
            Dist: new LootDistribution(
                new Dictionary<string, int>(){
                    [ItemID.Mythril] = 1
                },
                new Dictionary<string, int>(){
                    [ItemID.Mythril] = 40
                }
            )
        ),
        [EnemyType.Warrior] = new EnemyConst(
            new Shooter(
                ammo: 20, 
                reloadTicks: 480
            ),
            new ShotgunShootPattern(
                new BulletContainer(
                    new Bullet(60, maxFramesActive: 240),
                    bFrame(),
                    BulletSpeed: 4f,
                    traits: new List<IBulletTrait>(){
                        new Warned(new WorldTime(ticks: 30)),
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
            HP: 25,
            Size: Constants.TileWidth * 2,
            Difficulty: 3,
            Dist: new LootDistribution(
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 50,
                    [ItemID.Cobalt] = 50
                },
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 60,
                    [ItemID.Cobalt] = 30
                }
            )
        ),
        [EnemyType.Wizard] = new EnemyConst(
            new Shooter(
                ammo: 10,
                ticksPerShot: 12,
                reloadTicks: 120
            ),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(60, maxFramesActive: 600),
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
            HP: 25,
            Size: Constants.TileWidth,
            Difficulty: 4,
            Dist: new LootDistribution(
                new Dictionary<string, int>(){
                    [ItemID.Cobalt] = 50,
                    [ItemID.Credit] = 50
                },
                new Dictionary<string, int>(){
                    [ItemID.Cobalt] = 40,
                    [ItemID.Credit] = 20
                }
            )
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
            Console.WriteLine($"difficulty {d} not specified, defaulting to max");
            d = 5; 
        }

        List<EnemyType> withD = withDifficulty[d]; 
        return withD[Util.NextInt(withD.Count)];
    }
}
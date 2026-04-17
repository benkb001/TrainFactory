namespace TrainGame.Components;
using System;
using System.Collections.Generic;
using TrainGame.Constants;
using TrainGame.Utils;

public static class CombatRewardDistribution {
    //dictionary<difficulty, dictionary<rarity, Distribution>>
    //1 is most common, 4 is most rare
    private static Dictionary<int, Dictionary<int, Distribution<string, int>>> rewards = new(){
        [1] = new Dictionary<int, Distribution<string, int>>() {
            [1] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 100
                }, 
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 50
                }
            ), 
            [2] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.Cobalt] = 100
                }, 
                new Dictionary<string, int>(){
                    [ItemID.Cobalt] = 50
                }
            ),
            [3] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.TimeCrystal] = 100
                },
                new Dictionary<string, int>(){
                    [ItemID.TimeCrystal] = 10
                }
            ),
            [4] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.TimeSeed] = 100
                },
                new Dictionary<string, int>(){
                    [ItemID.TimeSeed] = 1
                } 
            )
        },
        [2] = new(){
            [1] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 90,
                    [ItemID.Cobalt] = 10
                },
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 60,
                    [ItemID.Cobalt] = 60
                }
            ),
            [2] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.Cobalt] = 100
                },
                new Dictionary<string, int>(){
                    [ItemID.Cobalt] = 20
                }
            ),
            [3] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.TimeCrystal] = 100
                },
                new Dictionary<string, int>(){
                    [ItemID.TimeCrystal] = 15
                }
            ),
            [4] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.TimeSeed] = 100
                },
                new Dictionary<string, int>(){
                    [ItemID.TimeSeed] = 1
                }
            )
        },
        [3] = new(){
            [1] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 80,
                    [ItemID.Cobalt] = 20
                },
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 100,
                    [ItemID.Cobalt] = 100
                }
            ),
            [2] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.Cobalt] = 100
                },
                new Dictionary<string, int>(){
                    [ItemID.Cobalt] = 150
                }
            ),
            [3] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.TimeCrystal] = 100
                },
                new Dictionary<string, int>(){
                    [ItemID.TimeCrystal] = 20
                }
            ),
            [4] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.TimeSeed] = 100
                },
                new Dictionary<string, int>(){
                    [ItemID.TimeSeed] = 1
                }
            )
        },
        [4] = new(){
            [1] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 70,
                    [ItemID.Cobalt] = 30
                },
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 150,
                    [ItemID.Cobalt] = 150
                }
            ),
            [2] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.Cobalt] = 100
                },
                new Dictionary<string, int>(){
                    [ItemID.Cobalt] = 40
                }
            ),
            [3] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.TimeCrystal] = 100
                },
                new Dictionary<string, int>(){
                    [ItemID.TimeCrystal] = 20
                }
            ),
            [4] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.TimeSeed] = 100
                },
                new Dictionary<string, int>(){
                    [ItemID.TimeSeed] = 1
                }
            )
        },
        [5] = new(){
            [1] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 60,
                    [ItemID.Cobalt] = 40
                },
                new Dictionary<string, int>(){
                    [ItemID.Credit] = 200,
                    [ItemID.Cobalt] = 200
                }
            ),
            [2] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.Cobalt] = 70,
                    [ItemID.Mythril] = 30
                },
                new Dictionary<string, int>(){
                    [ItemID.Cobalt] = 300,
                    [ItemID.Mythril] = 150
                }
            ),
            [3] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.TimeCrystal] = 90,
                    [ItemID.Mythril] = 10
                },
                new Dictionary<string, int>(){
                    [ItemID.TimeCrystal] = 30,
                    [ItemID.Mythril] = 300
                }
            ),
            [4] = new Distribution<string, int>(
                new Dictionary<string, int>(){
                    [ItemID.TimeSeed] = 100
                },
                new Dictionary<string, int>(){
                    [ItemID.TimeSeed] = 1
                }
            )
        }
    };

    public static (string, int) GetRandom(int difficulty, int rarity) {
        return rewards[difficulty][rarity].GetRandom();
    }
}

public class CombatRewardSpawner {
    public float LootChance = 0.1f;
    public int LootMultiplier = 1;
    public int ShieldHealAmount = 5;
    public int ShieldHealAmountLevel = 1;
    public int XP = 0; 
    public int XPToNextLevel = 10;
    public int ExtraXPPerKill = 0;

    public Dictionary<int, int> BaseRarityChances = new() {
        [1] = 97,
        [2] = 1,
        [3] = 1,
        [4] = 1
    };

    public Distribution<int, int> RarityDistribution = new Distribution<int, int>(
        new (){
            [1] = 1,
            [2] = 1,
            [3] = 1,
            [4] = 1
        },
        new (){
            [1] = 1,
            [2] = 2,
            [3] = 3, 
            [4] = 4
        }
    );

    public CombatRewardSpawner() {
        Reset();
    }

    public void Reset() {
        XP = 0; 
        ResetRarityChances();
    }

    public void ResetRarityChances() {
        for (int i = 1; i < 4; i++) {
            RarityDistribution.SetChance(i, BaseRarityChances[i]);
        }
    }

    public int GetLootRarity() {
        (int rarity, int _) = RarityDistribution.GetRandom();
        if (rarity == 4) {
            ResetRarityChances();
        } else {
            //Basically if you get something not 4, take one ball and move it up towards 4. 
            //If you get 4, reset
            //IDK if this is good because it is kinda like going on a streak, we won't move 
            //a lot of balls towards 4 until there are a lot of balls at 3, but if this ends 
            //up feeling too op we will change it 
            int prevLowRarityChance = RarityDistribution.GetChance(rarity);
            int prevHighRarityChance = RarityDistribution.GetChance(rarity + 1);
            RarityDistribution.SetChance(rarity, prevLowRarityChance - 1);
            RarityDistribution.SetChance(rarity + 1, prevHighRarityChance + 1);
        }
        return rarity;
    }

    public void UpgradeShieldHealAmount() {
        if (ShieldHealAmountLevel < Constants.MaxShieldHealAmountLevel) {
            ShieldHealAmountLevel++; 
            ShieldHealAmount += Constants.ShieldHealPerLevel;
        }
    }
}
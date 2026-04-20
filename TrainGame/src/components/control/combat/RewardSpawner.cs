namespace TrainGame.Components;
using System;
using System.Collections.Generic;
using TrainGame.Constants;
using TrainGame.Utils;

public static class CombatRewardDistribution {
    //dictionary<difficulty, dictionary<rarity, Distribution>>
    //1 is most common, 4 is most rare
    private static Dictionary<int, Dictionary<int, Distribution<(string, int)>>> rewards = new(){
        [1] = new Dictionary<int, Distribution<(string, int)>>() {
            [1] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Credit, 50)] = 1
                }
            ), 
            [2] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Cobalt, 100)] = 1
                }
            ),
            [3] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeCrystal, 10)] = 1
                }
            ),
            [4] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeSeed, 1)] = 1
                }
            )
        },
        [2] = new(){
            [1] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Credit, 60)] = 90,
                    [(ItemID.Cobalt, 60)] = 10
                }
            ),
            [2] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Cobalt, 120)] = 1
                }
            ),
            [3] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeCrystal, 15)] = 1
                }
            ),
            [4] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeSeed, 1)] = 1
                }
            )
        },
        [3] = new(){
            [1] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Credit, 100)] = 80,
                    [(ItemID.Cobalt, 100)] = 20
                }
            ),
            [2] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Cobalt, 150)] = 100
                }
            ),
            [3] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeCrystal, 20)] = 100
                }
            ),
            [4] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeSeed, 1)] = 1
                }
            )
        },
        [4] = new(){
            [1] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Credit, 150)] = 70,
                    [(ItemID.Cobalt, 150)] = 30
                }
            ),
            [2] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Cobalt, 200)] = 1
                }
            ),
            [3] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeCrystal, 20)] = 1
                }
            ),
            [4] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeSeed, 1)] = 1
                }
            )
        },
        [5] = new(){
            [1] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Credit, 200)] = 60,
                    [(ItemID.Cobalt, 200)] = 40
                }
            ),
            [2] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Cobalt, 300)] = 70,
                    [(ItemID.Mythril, 150)] = 30
                }
            ),
            [3] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeCrystal, 30)] = 90,
                    [(ItemID.Mythril, 300)] = 10
                }
            ),
            [4] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeSeed, 1)] = 1
                }
            )
        },
        [6] = new(){
            [1] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Credit, 200)] = 50,
                    [(ItemID.Cobalt, 200)] = 40,
                    [(ItemID.Mythril, 150)] = 10
                }
            ),
            [2] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Cobalt, 350)] = 60,
                    [(ItemID.Mythril, 200)] = 40
                }
            ),
            [3] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeCrystal, 35)] = 90,
                    [(ItemID.Mythril, 350)] = 10
                }
            ),
            [4] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeSeed, 1)] = 1
                }
            )
        },
        [7] = new(){
            [1] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Credit, 250)] = 30,
                    [(ItemID.Cobalt, 250)] = 50,
                    [(ItemID.Mythril, 200)] = 20
                }
            ),
            [2] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Cobalt, 400)] = 50,
                    [(ItemID.Mythril, 300)] = 50
                }
            ),
            [3] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeCrystal, 40)] = 90,
                    [(ItemID.Mythril, 400)] = 10
                }
            ),
            [4] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeSeed, 1)] = 1
                }
            )
        },
        [8] = new(){
            [1] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Credit, 300)] = 30,
                    [(ItemID.Cobalt, 300)] = 40,
                    [(ItemID.Mythril, 250)] = 30
                }
            ),
            [2] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Cobalt, 450)] = 30,
                    [(ItemID.Mythril, 350)] = 70
                }
            ),
            [3] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeCrystal, 45)] = 90,
                    [(ItemID.Mythril, 500)] = 10
                }
            ),
            [4] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeSeed, 1)] = 1
                }
            )
        },
        [9] = new(){
            [1] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Adamantite, 50)] = 1
                }
            ),
            [2] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Adamantite, 100)] = 1
                }
            ),
            [3] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Adamantite, 150)] = 1
                }
            ),
            [4] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeSeed, 1)] = 1
                }
            )
        },
        [10] = new(){
            [1] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Adamantite, 100)] = 1
                }
            ),
            [2] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Adamantite, 150)] = 1
                }
            ),
            [3] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Adamantite, 200)] = 1
                }
            ),
            [4] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeSeed, 1)] = 1
                }
            )
        },
        [11] = new(){
            [1] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Adamantite, 300)] = 1
                }
            ),
            [2] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Adamantite, 500)] = 1
                }
            ),
            [3] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Adamantite, 700)] = 1
                }
            ),
            [4] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeSeed, 1)] = 1
                }
            )
        },
        [12] = new(){
            [1] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Adamantite, 1000)] = 1
                }
            ),
            [2] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Adamantite, 2000)] = 1
                }
            ),
            [3] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.Adamantite, 3000)] = 1
                }
            ),
            [4] = new Distribution<(string, int)>(
                new Dictionary<(string, int), int>(){
                    [(ItemID.TimeSeed, 1)] = 1
                }
            )
        }
    };

    public static (string, int) GetRandom(int difficulty, int rarity) {
        return rewards[difficulty][rarity].GetRandom();
    }
}

public class CombatRewardSpawner : IExperienceTracker {
    private int maxRarity = 4; 

    public float LootChance = 0.1f;
    public int LootMultiplier = 1;
    public int ShieldHealAmount = 5;
    public int ShieldHealAmountLevel = 1;
    public int XP = 0; 
    public int XPToNextLevel = 10;
    public int ExtraXPPerKill = 0;

    public int GetXP() => XP; 
    public int GetXPToNextLevel() => XPToNextLevel; 

    private Distribution<int> rarityDistribution = new Distribution<int>(
        new (){
            [1] = 1000,
            [2] = 100,
            [3] = 10,
            [4] = 1
        }
    );

    private Distribution<int> xpMultiplierDistribution = new Distribution<int>(
        new Dictionary<int, int>() {
            [0] = 35,
            [1] = 25,
            [2] = 20,
            [3] = 15,
            [4] = 5
        }
    );

    private Distribution<int> upgradeRarityDistribution = new Distribution<int>(
        new Dictionary<int, int>(){
            [1] = 60,
            [2] = 30,
            [3] = 8,
            [4] = 2
        }
    );

    private int getRarity(Distribution<int> dist) {
        int rarity = dist.GetRandom(); 
        if (rarity == maxRarity) {
            dist.Reset();
        } else {
            dist.MoveChance(rarity, rarity + 1, 1);
        }
        return rarity; 
    }

    public CombatRewardSpawner() {
        Reset();
    }

    public void Reset() {
        XP = 0; 
        rarityDistribution.Reset();
        xpMultiplierDistribution.Reset();
    }

    public int GetLootRarity() {
        return getRarity(rarityDistribution);
    }

    public int GetXPMultiplier() {
        return getRarity(xpMultiplierDistribution);
    }

    public int GetUpgradeRarity() {
        return getRarity(upgradeRarityDistribution);
    }

    public void UpgradeShieldHealAmount() {
        if (ShieldHealAmountLevel < Constants.MaxShieldHealAmountLevel) {
            ShieldHealAmountLevel++; 
            ShieldHealAmount += Constants.ShieldHealPerLevel;
        }
    }
}
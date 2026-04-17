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
        [1] = 1000,
        [2] = 100,
        [3] = 10,
        [4] = 1
    };

    public Distribution<int> RarityDistribution = new Distribution<int>(
        new (){
            [1] = 1,
            [2] = 1,
            [3] = 1,
            [4] = 1
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
        int rarity = RarityDistribution.GetRandom();
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
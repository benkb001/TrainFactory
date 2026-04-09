namespace TrainGame.Components;
using TrainGame.Constants;
public class CombatRewardSpawner {
    public int LootMultiplier = 1;
    public float RewardChance = Constants.RewardChance;
    public int ShieldHealAmount = 5;
    public int ShieldHealAmountLevel = 1;
    public CombatRewardSpawner() {}

    public void UpgradeShieldHealAmount() {
        if (ShieldHealAmountLevel < Constants.MaxShieldHealAmountLevel) {
            ShieldHealAmountLevel++; 
            ShieldHealAmount += Constants.ShieldHealPerLevel;
        }
    }
}
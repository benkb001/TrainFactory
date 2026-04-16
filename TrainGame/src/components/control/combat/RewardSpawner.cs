namespace TrainGame.Components;
using TrainGame.Constants;
public class CombatRewardSpawner {
    public int LootMultiplier = 1;
    public int ShieldHealAmount = 5;
    public int ShieldHealAmountLevel = 1;
    public int XP = 0; 
    public int XPToNextLevel = 10;
    public int ExtraXPPerKill = 0;

    public CombatRewardSpawner() {}

    public void Reset() {
        XP = 0; 
    }

    public void UpgradeShieldHealAmount() {
        if (ShieldHealAmountLevel < Constants.MaxShieldHealAmountLevel) {
            ShieldHealAmountLevel++; 
            ShieldHealAmount += Constants.ShieldHealPerLevel;
        }
    }
}
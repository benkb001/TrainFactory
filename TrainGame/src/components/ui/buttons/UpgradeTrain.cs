
namespace TrainGame.Components; 

public class UpgradeTrainButton : IClickable {
    public readonly Train UpgradingTrain; 
    public readonly Inventory PlayerInv; 

    public UpgradeTrainButton(Train t, Inventory inv) {
        UpgradingTrain = t; 
        PlayerInv = inv; 
    }

    public string GetText() {
        return $"Upgrade {UpgradingTrain.Id}'s inventory?";
    }
}
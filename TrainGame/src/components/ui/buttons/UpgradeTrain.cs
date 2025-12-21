
namespace TrainGame.Components; 

public class UpgradeTrainButton : IClickable {
    public readonly Train UpgradingTrain; 

    public UpgradeTrainButton(Train t) {
        UpgradingTrain = t; 
    }

    public string GetText() {
        return $"Upgrade {UpgradingTrain.Id}'s inventory?";
    }
}
namespace TrainGame.Components;

using TrainGame.Utils;
using TrainGame.Constants;

public class CombatReward {
    public WorldTime Expire;
    public CombatReward(WorldTime now) {
        this.Expire = now + new WorldTime(minutes: Constants.RewardLifetimeSeconds); 
    }
}
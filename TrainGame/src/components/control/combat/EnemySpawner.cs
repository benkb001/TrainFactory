namespace TrainGame.Components;

using System.Collections.Generic;
using System.Linq;
using TrainGame.Utils;
using TrainGame.Systems;
using TrainGame.Constants;

public class CombatRewardCollectedMessage {}

public class EnemySpawner {
    public WorldTime NextSpawn = new WorldTime(minutes: 2);
    public int Difficulty; 
    public int NumActive;
    public int TargetNumActive;
    public int MaxDifficulty = Constants.MaxDifficultyPerUpgrade; 
    public int MaxDifficultyLevel = 1;

    public EnemySpawner() {
        Reset();
    }

    public void Reset() {
        Difficulty = 1; 
        NumActive = 0; 
        TargetNumActive = 1; 
    }

    public void UpgradeMaxDifficulty() {
        if (MaxDifficultyLevel < Constants.MaxMaxDifficultyLevel) {
            MaxDifficultyLevel++; 
            MaxDifficulty += Constants.MaxDifficultyPerUpgrade;
        }
    }
}
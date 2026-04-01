namespace TrainGame.Components;

using System.Collections.Generic;
using System.Linq;
using TrainGame.Utils;
using TrainGame.Systems;

public class CombatRewardCollectedMessage {}

public class EnemySpawner {
    public WorldTime NextSpawn = new WorldTime(minutes: 2);
    public int Difficulty;

    public EnemySpawner(int difficulty = 1) {
        this.Difficulty = difficulty; 
    }
}
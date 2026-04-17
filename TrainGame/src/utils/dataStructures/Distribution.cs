namespace TrainGame.Utils;

using System.Collections.Generic;
using System;
using System.Linq;

public class Distribution<T> {
    private int chanceTotal; 
    private Dictionary<T, int> chances; 

    private void setChanceTotal() {
        this.chanceTotal = chances.Aggregate(0, (acc, cur) => acc + cur.Value);
    }

    public Distribution(Dictionary<T, int> chances) {
        this.chances = chances;
        setChanceTotal();
    }

    public T GetRandom() {
        int r = Util.NextInt(chanceTotal); 
        int total = 0; 

        foreach (KeyValuePair<T, int> chance in chances) {
            T key = chance.Key;
            int c = chance.Value; 

            total += c; 

            
            if (total > r) {
                return key; 
            }
        }

        throw new InvalidOperationException("Error in LootDistribution.GetRandom");
    }

    public int GetChance(T eventKey) {
        return chances[eventKey];
    }

    public void SetChance(T eventKey, int chance) {
        chances[eventKey] = chance;
        setChanceTotal();
    }
}
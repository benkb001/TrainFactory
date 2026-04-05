namespace TrainGame.Utils;

using System.Collections.Generic;
using System;
using System.Linq;

public class Distribution<T, U> {
    private int chanceTotal; 
    private Dictionary<T, int> chances; 
    private Dictionary<T, U> events; 

    public Distribution(Dictionary<T, int> chances, Dictionary<T, U> events) {
        if (chances.Count != events.Count) {
            throw new InvalidOperationException($"A  distribution must have the same number of chance and event");
        }

        foreach (T key in chances.Keys) {
            if (!events.ContainsKey(key)) {
                throw new InvalidOperationException("A distribution must have all keys match in chances and events");
            }
        }

        this.chances = chances;
        this.events = events;
        this.chanceTotal = chances.Aggregate(0, (acc, cur) => acc + cur.Value);
    }

    public (T, U) GetRandom() {
        int r = Util.NextInt(chanceTotal); 
        int total = 0; 

        foreach (KeyValuePair<T, int> chance in chances) {
            T key = chance.Key;
            int c = chance.Value; 

            total += c; 

            
            if (total > r) {
                U eve = events[key]; 
                return (key, eve); 
            }
        }

        throw new InvalidOperationException("Error in LootDistribution.GetRandom");
    }
}
namespace TrainGame.Components;

using System;
using System.Linq;
using System.Collections.Generic; 

public class ReceiveDamageMessage {
    private List<int> damageSources = new();

    public int DMG => Math.Max(0, damageSources.Aggregate(0, (acc, cur) => acc + cur)); 
    public int FirstSourceDMG => damageSources[0]; 

    public ReceiveDamageMessage(int dmg) {
        damageSources.Add(dmg);
    }

    public void AddDamage(int dmg) {
        damageSources.Add(Math.Max(0, dmg));
    }

    public void ReduceDamage(int dmg) {
        damageSources.Add(dmg < 0 ? dmg : -dmg);
    }

    public void SetDamage(int dmg) {
        damageSources.Clear();
        AddDamage(dmg);
    }
}

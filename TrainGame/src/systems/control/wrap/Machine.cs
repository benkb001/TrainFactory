namespace TrainGame.Systems;

using System;
using System.Collections.Generic;
using System.Linq;

using TrainGame.Components;
using TrainGame.ECS;

public class MachineWrap {
    public static Machine GetByID(World w, string id) {
        Machine res = w.GetMatchingEntities([typeof(Machine), typeof(Data)])
        .Select(e => w.GetComponent<Machine>(e))
        .Where(m => m.Id == id)
        .FirstOrDefault();

        if (res == null || res.Equals(default(Machine))) {
            throw new InvalidOperationException($"No machine with {id} exists");
        }

        return res;
    }

    public static Machine GetTest() {
        Inventory inv = new Inventory("Default", 1, 1); 
        return new Machine(inv, new Dictionary<string, int>(), "", 0, minTicks: 1); 
    }
}
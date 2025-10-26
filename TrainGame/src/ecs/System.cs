namespace TrainGame.ECS; 

using System.Collections.Generic;
using System; 

public class _System {
    private HashSet<int> es {get;}

    private bool[] signature;

    private Action<World, int> transformer;
    private Action<World> update; 

    public _System(bool[] s, Action<World, int> t) {
        es = new HashSet<int>(); 
        transformer = t; 
        signature = s; 
    }

    public _System(bool[] s, Action<World> u) {
        es = new HashSet<int>(); 
        update = u; 
        signature = s; 
    }

    public void AddEntity(int entity) {
        es.Add(entity); 
    }

    public void RemoveEntity(int entity) {
        es.Remove(entity); 
    }

    public bool ContainsEntity(int entity) {
        return es.Contains(entity); 
    }

    public bool ActsOnComponentType(int componentTypeIndex) {
        return signature[componentTypeIndex]; 
    }

    public void Update(World w) {
        if (update != null) {
            update(w); 
        } else {
            foreach (int e in es) {
                transformer(w, e); 
            }
        }
    }

    public int EntityCount() {
        return es.Count; 
    }
}
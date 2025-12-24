namespace TrainGame.ECS; 

using System.Collections.Generic;
using System; 
using System.Linq; 

public class _System {
    private HashSet<int> es {get;}

    private bool[] signature;

    private Action<World, int> transformer;
    private Action<World> update; 
    private Func<int, int> orderer; 

    public _System(bool[] s, Action<World, int> t, Func<int, int> orderer = null) {
        es = new HashSet<int>(); 
        transformer = t; 
        signature = s; 
        this.orderer = orderer; 
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

            IEnumerable<int> entsEnum = es; 
            if (orderer != null) {
                entsEnum = es.OrderBy(orderer); 
            }

            foreach (int e in entsEnum) {
                transformer(w, e); 
            }
        }
    }

    public int EntityCount() {
        return es.Count; 
    }
}
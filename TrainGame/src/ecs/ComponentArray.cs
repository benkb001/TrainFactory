namespace TrainGame.ECS; 

using System.Collections.Generic;
using System; 

public interface IComponentArray {
    public int Index {get; set;}

    void RemoveEntity(int e);
}

class ComponentArray<T> : IComponentArray {
    private Dictionary<int, T> entities; 

    public int Index {get; set;}

    public ComponentArray(int i) {
        Index = i; 
        entities = new Dictionary<int, T>();
    }

    public void AddEntity(int e, T strct) {
        entities[e] = strct;
    }

    public void RemoveEntity(int e) {
        entities.Remove(e); 
    }

    public T GetComponent(int e) {
        EnsureEntityExists(e); 
        return entities[e]; 
    }

    public bool ContainsEntity(int e) {
        return entities.ContainsKey(e); 
    }

    public int EntityCount() {
        return entities.Count; 
    }

    private void EnsureEntityExists(int e) {
        if (!ContainsEntity(e)) {
            throw new InvalidOperationException(
                $"Entity num {e} not registered by ComponentArray {typeof(T).Name}"
            );
        }
    }

    //I don't like it but hey
    public Dictionary<int, T> GetEntities() {
        return entities; 
    }
}
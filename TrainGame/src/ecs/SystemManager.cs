namespace TrainGame.ECS; 

using System.Collections.Generic;
using System; 

using TrainGame.Constants; 

class SystemManager {
    private Dictionary<int, bool[]> entitySignatures; 
    private HashSet<_System> systems;

    public SystemManager() {
        entitySignatures = new(); 
        systems = new(); 
    }

    public _System Register(bool[] signature, Action<World, int> transformer, Func<int, int> orderer = null) {
        _System s = new _System(signature, transformer, orderer); 
        systems.Add(s); 
        return s; 
    }

    public _System Register(bool[] signature, Action<World> update) {
        _System s = new _System(signature, update); 
        systems.Add(s); 
        return s; 
    }

    public void SetEntitySignature(int entity, bool[] signature) {
        entitySignatures[entity] = signature; 

        foreach (_System s in systems) {
            bool removed = false; 
            int i = 0; 

            while (!removed && i < Constants.MaxComponents) {
                if (s.ActsOnComponentType(i) && !signature[i]) {
                    s.RemoveEntity(entity); 
                    removed = true; 
                }
                i++; 
            }

            if (!removed) {
                s.AddEntity(entity); 
            }
        }
    }

    public void RemoveEntity(int entity) {
        foreach (_System s in systems) {
            s.RemoveEntity(entity); 
        }
    }

    public void Update(World w) {
        foreach(_System s in systems) {
            s.Update(w);
        }
    }

    public int SystemCount() {
        return systems.Count; 
    }
}
namespace TrainGame.ECS; 

using System.Collections.Generic;
using System; 

using TrainGame.Constants; 

class SystemManager {
    private HashSet<_System> systems;

    public SystemManager() {
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

        foreach (_System s in systems) {
            bool removed = false; 
            IEnumerable<int> typesActedOn = s.TypesActedOn; 
            
            foreach (int i in typesActedOn) {
                if (!signature[i]) {
                    s.RemoveEntity(entity); 
                    removed = true;
                    break;
                }
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
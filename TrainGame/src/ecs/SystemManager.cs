namespace TrainGame.ECS; 

using System.Collections.Generic;
using System; 

using TrainGame.Constants; 

public interface IWorld {}

class SystemManager<T> where T : IWorld {
    private HashSet<_System<T>> systems;
    public HashSet<_System<T>> Systems => systems;

    public SystemManager() {
        systems = new(); 
    }

    public _System<T> Register(bool[] signature, Action<T, int> transformer, Func<int, int> orderer = null) {
        _System<T> s = new _System<T>(signature, transformer, orderer); 
        systems.Add(s); 
        return s; 
    }

    public _System<T> Register(bool[] signature, Action<T> update) {
        _System<T> s = new _System<T>(signature, update); 
        systems.Add(s); 
        return s; 
    }

    public void SetEntitySignature(int entity, bool[] signature) {

        foreach (_System<T> s in systems) {
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
        foreach (_System<T> s in systems) {
            s.RemoveEntity(entity); 
        }
    }

    public int SystemCount() {
        return systems.Count; 
    }
}
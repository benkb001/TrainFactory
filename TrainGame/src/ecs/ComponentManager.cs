namespace TrainGame.ECS; 

using System.Collections.Generic;
using System; 

using TrainGame.Constants;

class ComponentManager {
    private int nextIndex = 0; 

    private Dictionary<Type, IComponentArray> components; 

    public ComponentManager() {
        components = new Dictionary<Type, IComponentArray>(); 
    }

    public void Register<T>() {
        if (nextIndex >= Constants.maxComponents) {
            throw new InvalidOperationException(
                "ComponentManager has registered max components"); 
        }

        if (components.ContainsKey(typeof(T))) {
            throw new InvalidOperationException(
                $"ComponnentManager has already registered {typeof(T).Name}"
            );
        }

        ComponentArray<T> c = new ComponentArray<T>(nextIndex); 
        nextIndex++; 
        components[typeof(T)] = c; 
    }

    public ComponentArray<T> GetComponentArray<T>() {
        return (ComponentArray<T>)components[typeof(T)]; 
    }

    public int GetComponentIndex<T>() {
        EnsureTypeRegistered<T>();
        return components[typeof(T)].Index; 
    }

    public void RemoveEntity(int entity) {
        foreach (IComponentArray c in components.Values) {
            c.RemoveEntity(entity); 
        }
    }

    public bool[] GetSignature(Type[] ts) {
        //bool[] default false
        bool[] sig = new bool[Constants.maxComponents]; 

        foreach (Type t in ts) {
            if (!components.ContainsKey(t)) {
                throw new InvalidOperationException(
                    $"{t.Name} not registered by component manager"
                );
            }
            sig[components[t].Index] = true;
        }

        return sig; 
    }

    public bool[] AddComponent<T>(int entity, bool[] signature, T c) {
        EnsureTypeRegistered<T>(); 
        signature[components[typeof(T)].Index] = true; 

        ((ComponentArray<T>)GetComponentArray<T>()).AddEntity(entity, c); 

        return signature; 
    }

    public bool[] RemoveComponent<T>(int entity, bool[] signature) {
        EnsureTypeRegistered<T>(); 
        signature[components[typeof(T)].Index] = false; 

        components[typeof(T)].RemoveEntity(entity); 
        return signature; 
    }

    public bool ComponentTypeIsRegistered<T>() {
        return components.ContainsKey(typeof(T)); 
    }

    public T GetComponent<T>(int e) {
        EnsureTypeRegistered<T>(); 
        return ((ComponentArray<T>)GetComponentArray<T>()).GetComponent(e); 
    }

    public bool ComponentContainsEntity<T>(int e) {
        EnsureTypeRegistered<T>(); 
        return ((ComponentArray<T>)GetComponentArray<T>()).ContainsEntity(e); 
    }

    public int EntityCount<T>() {
        EnsureTypeRegistered<T>(); 
        return ((ComponentArray<T>)GetComponentArray<T>()).EntityCount();  
    }

    public int GetComponentTypeCount() {
        return components.Count; 
    }

    private void EnsureTypeRegistered<T>() {
        if (!ComponentTypeIsRegistered<T>()) {
            Type t = typeof(T); 
            throw new InvalidOperationException(
                $"{t.Name} not registered by component manager"
            ); 
        }
    }
}
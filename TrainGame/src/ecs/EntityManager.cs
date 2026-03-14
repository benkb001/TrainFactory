namespace TrainGame.ECS; 

using System.Collections.Generic;
using System; 
using System.Linq; 

using TrainGame.Constants;

class EntityManager {
    private Queue<int> availableEntities; 
    private Dictionary<int, bool[]> eSignatures; 

    private const int maxEntities = 10000; 

    public EntityManager() {
        availableEntities = new Queue<int>(); 
        eSignatures = new Dictionary<int, bool[]>(); 

        for (int i = 0; i < maxEntities; i++) {
            availableEntities.Enqueue(i); 
        }
    }

    public int Add() {
        if (availableEntities.Count < 1) {
            throw new InvalidOperationException("EntityManager has exceeded" + 
            " max Entity limit"); 
        }
            
        int entity = availableEntities.Dequeue(); 
        eSignatures[entity] = new bool[Constants.MaxComponents]; 
        return entity; 
    }

    public void RemoveEntity(int entity) {
        if (eSignatures.Remove(entity)) {
            availableEntities.Enqueue(entity); 
        }
    }

    public void SetEntitySignature(int entity, bool[] signature) {
        EnsureEntityExists(entity); 
        eSignatures[entity] = signature; 
    }

    public bool[] GetSignature(int entity) {
        EnsureEntityExists(entity); 
        return eSignatures[entity]; 
    }

    public int EntityCount() {
        return eSignatures.Count; 
    }

    public bool EntityExists(int entity) {
        return eSignatures.ContainsKey(entity); 
    }

    private void EnsureEntityExists(int e) {
        if (!EntityExists(e)) {
            throw new InvalidOperationException(
                $"Entity num {e} not registered by Entity Manager"
            );
        }
    }

    public List<int> GetEntities() {
        return eSignatures.Keys.ToList(); 
    }

    private List<int> getSetIndices(bool[] signature) {
        List<int> setIndices = new(); 

        for (int i = 0; i < signature.Length; i++) {
            if (signature[i]) {
                setIndices.Add(i); 
            }
        }

        return setIndices;
    }

    public List<int> GetMatchingEntities(bool[] signature) {
        List<int> res = new(); 
        
        List<int> setIndices = getSetIndices(signature);

        foreach (KeyValuePair<int, bool[]> eSig in eSignatures) {
            
            bool mismatch = false; 

            foreach (int i in setIndices) {
                if (!eSig.Value[i]) {
                    mismatch = true; 
                    break;
                }
            }

            if (!mismatch) {
                res.Add(eSig.Key); 
            }
        }

        return res; 
    }

    public int GetFirstMatchingEntity(bool[] signature) {
        List<int> setIndices = getSetIndices(signature);
        
        foreach (KeyValuePair<int, bool[]> eSig in eSignatures) {

            bool mismatch = false; 

            foreach (int i in setIndices) {
                if (!eSig.Value[i]) {
                    mismatch = true; 
                    break;
                }
            }

            if (!mismatch) {
                return eSig.Key;
            }
        }
        
        return -1; 
    }
}
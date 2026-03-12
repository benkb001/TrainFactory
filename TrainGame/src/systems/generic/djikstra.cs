
namespace TrainGame.Utils; 

using System; 
using System.Collections.Generic; 
using System.Linq; 

using TrainGame.Components;

public static partial class Util {
    public static List<T> ShortestPathUnweighted<T>(List<T> nodes, T start, T dest) where T : INode<T> {

        List<T> unvisited = nodes.ToList();
        Dictionary<T, List<T>> paths = unvisited
        .Select(n => new KeyValuePair<T, List<T>>(n, null))
        .ToDictionary(); 

        paths[start] = new List<T>();

        while (unvisited.Count > 0) {
            T minDistNode = unvisited
            .OrderBy(n => paths[n] == null ? Int32.MaxValue : paths[n].Count)
            .FirstOrDefault();

            List<T> path = paths[minDistNode];
            int distThroughCur = path == null ? Int32.MaxValue : path.Count + 1;

            foreach (T node in minDistNode.GetNeighbors().Where(n => unvisited.Contains(n))) {
                
                int prevDist = paths[node] == null ? Int32.MaxValue : paths[node].Count;

                if (prevDist > distThroughCur) {
                    List<T> newPath = new List<T>(path);
                    newPath.Add(node); 
                    paths[node] = newPath; 
                }
            }

            bool removed = unvisited.Remove(minDistNode);
            if (!removed) {
                break;
            }
        }

        return paths[dest];
    }
}
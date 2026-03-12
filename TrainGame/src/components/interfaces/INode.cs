namespace TrainGame.Components;

using System; 
using System.Collections.Generic;

public interface INode<T> {
    List<T> GetNeighbors();
}
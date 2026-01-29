namespace TrainGame.Utils; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class QuadTree<T> {
    private (QuadTree<T>, QuadTree<T>, QuadTree<T>, QuadTree<T>) children;
    private T data;
    private bool hasChildren = false;

    public (QuadTree<T>, QuadTree<T>, QuadTree<T>, QuadTree<T>) Children => children;
    public T Data => data; 

    public QuadTree(T data) {
        this.data = data; 
    }

    public void SetChildren(T d1, T d2, T d3, T d4) {
        this.children = (new QuadTree<T>(d1), new QuadTree<T>(d2), new QuadTree<T>(d3), new QuadTree<T>(d4)); 
        hasChildren = true;
    }

    public List<T> GetLeaves() {
        if (!hasChildren) {
            return new List<T>() { data };
        }

        (QuadTree<T> c1, QuadTree<T> c2, QuadTree<T> c3, QuadTree<T> c4) = children; 
        return c1.GetLeaves().Concat(c2.GetLeaves()).Concat(c3.GetLeaves()).Concat(c4.GetLeaves()).ToList(); 
    }

    public List<T> GetAll() {
        List<T> single = new List<T>() { data };
        if (!hasChildren) {
            return single;
        }
        (QuadTree<T> c1, QuadTree<T> c2, QuadTree<T> c3, QuadTree<T> c4) = children; 
        return single.Concat(c1.GetLeaves()).Concat(c2.GetLeaves()).Concat(c3.GetLeaves()).Concat(c4.GetLeaves()).ToList(); 
    }
}
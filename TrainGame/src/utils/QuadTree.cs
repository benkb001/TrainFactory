namespace TrainGame.Utils; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class QuadTree<T> {
    public QuadTree<T> C1; 
    public QuadTree<T> C2; 
    public QuadTree<T> C3; 
    public QuadTree<T> C4; 

    private T data;
    private bool hasChildren = false;
    public bool HasChildren => hasChildren;

    public (QuadTree<T>, QuadTree<T>, QuadTree<T>, QuadTree<T>) Children => (C1, C2, C3, C4);
    public T Data => data; 

    public QuadTree(T data) {
        this.data = data; 
    }

    public void SetChildren(T d1, T d2, T d3, T d4) {
        this.C1 = new QuadTree<T>(d1);
        this.C2 = new QuadTree<T>(d2);
        this.C3 = new QuadTree<T>(d3); 
        this.C4 = new QuadTree<T>(d4);

        hasChildren = true;
    }

    public void RemoveChildren() {
        hasChildren = false; 
        this.C1 = null; 
        this.C2 = null; 
        this.C3 = null; 
        this.C4 = null;
    }

    public List<T> GetLeaves() {
        List<T> ls = new(); 
        List<QuadTree<T>> toProcess = new(); 

        toProcess.Add(this);

        while (toProcess.Count > 0) {
            QuadTree<T> cur = toProcess[0]; 

            if (cur.hasChildren) {
                toProcess.Add(cur.C1);
                toProcess.Add(cur.C2);
                toProcess.Add(cur.C3);
                toProcess.Add(cur.C4);
            } else {
                ls.Add(cur.data);
            }

            toProcess.RemoveAt(0);
        }
        
        return ls;
    }

    public List<T> GetAll() {
        List<T> single = new List<T>() { data };
        if (!hasChildren) {
            return single;
        }

        return single.Concat(C1.GetLeaves()).Concat(C2.GetLeaves()).Concat(C3.GetLeaves()).Concat(C4.GetLeaves()).ToList(); 
    }
}
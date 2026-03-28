namespace TrainGame.Utils;

using System;
using System.Collections.Generic;

public class CircularList<T> {

    private List<T> list; 

    public CircularList(List<T> ls) {
        this.list = ls; 
    }

    public T this[int i] {
        get { return list[i % list.Count]; }
        set { list[i % list.Count] = value; }
    }
}
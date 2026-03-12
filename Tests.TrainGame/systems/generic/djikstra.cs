using System;
using System.Linq;
using System.Collections.Generic;

using TrainGame.Systems;
using TrainGame.Components;
using TrainGame.Utils;

public class TestNode : INode<TestNode> {
    private static int nextID = 0;

    public readonly int ID;

    public List<TestNode> Neighbors = new(); 

    public TestNode() {
        this.ID = nextID; 
        nextID++; 
    }

    public void AddNeighbor(TestNode n) {
        Neighbors.Add(n);
        n.Neighbors.Add(this);
    }
    
    public List<TestNode> GetNeighbors() {
        return Neighbors;
    }
}

public class DjikstraTest {
    [Fact]
    public void Djikstra_ShortestPathUnweightedShouldReturnTheShortestPathFromStartToDestination() {
        TestNode n0 = new TestNode();
        TestNode n1 = new TestNode(); 
        TestNode n2 = new TestNode(); 
        TestNode n3 = new TestNode(); 
        TestNode n4 = new TestNode(); 
        TestNode n5 = new TestNode(); 
        
        List<TestNode> ns = new() { n0, n1, n2, n3, n4, n5 };

        n0.AddNeighbor(n1); 
        n0.AddNeighbor(n2);
        n1.AddNeighbor(n3);
        n1.AddNeighbor(n2);
        n2.AddNeighbor(n4);
        n1.AddNeighbor(n5);
        n4.AddNeighbor(n5);

        List<TestNode> shortestFrom0To1 = Util.ShortestPathUnweighted(ns, n0, n1);
        Assert.Single(shortestFrom0To1);
        Assert.Equal(n1, shortestFrom0To1[0]);

        List<TestNode> from0To5 = Util.ShortestPathUnweighted(ns, n0, n5); 
        Assert.Equal(2, from0To5.Count); 
        Assert.Equal(n1, from0To5[0]); 
        Assert.Equal(n5, from0To5[1]);
    }
}
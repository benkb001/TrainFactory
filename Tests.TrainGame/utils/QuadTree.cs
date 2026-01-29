using TrainGame.Utils; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class QuadTreeTest {
    [Fact]
    public void QuadTree_GetLeavesShouldReturnAListWithInstanceForEachLeaf() {
        QuadTree<Test> qT = new QuadTree<Test>(new Test()); 
        Assert.Single(qT.GetLeaves()); 
        qT.SetChildren(new Test(), new Test(), new Test(), new Test());
        Assert.Equal(4, qT.GetLeaves().Count); 
        (QuadTree<Test> t1, QuadTree<Test> t2, QuadTree<Test> t3, QuadTree<Test> t4) = qT.Children; 
        t1.SetChildren(new Test(), new Test(), new Test(), new Test()); 
        Assert.Equal(4, t1.GetLeaves().Count); 
        Assert.Equal(7, qT.GetLeaves().Count);
    }
}
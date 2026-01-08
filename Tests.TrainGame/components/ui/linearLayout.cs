using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class LinearLayoutComponentTest {

    [Fact]
    public void LinearLayout_ShouldRespectConstructors() {
        LinearLayout ll = new LinearLayout("horizontal", "alignLow"); 
        Assert.True(ll.IsHorizontal()); 
        Assert.True(ll.IsAlignLow()); 
        Assert.False(ll.IsVertical()); 
        Assert.False(ll.IsAlignHigh()); 
        Assert.False(ll.IsSpaceEven()); 
        Assert.Equal("horizontal", ll.GetDirection()); 
        Assert.Equal("alignlow", ll.GetSpacing()); 
    }

    [Fact] 
    public void LinearLayout_ShouldRespectSetters() {
        LinearLayout ll = new LinearLayout("horizontal", "alignLow"); 
        ll.SetDirection("vertical"); 
        Assert.True(ll.IsVertical()); 
        Assert.False(ll.IsHorizontal()); 
        ll.SetSpacing("spaceEven"); 
        Assert.True(ll.IsSpaceEven()); 
        Assert.False(ll.IsAlignLow()); 
        Assert.Equal("spaceeven", ll.GetSpacing()); 
        Assert.Equal("vertical", ll.GetDirection()); 
    }

    [Fact]
    public void LinearLayout_ShouldStoreAddedChildren() {
        LinearLayout ll = new LinearLayout("horizontal", "alignlow"); 
        Assert.DoesNotContain(0, ll.GetChildren()); 
        Assert.Empty(ll.GetChildren()); 
        ll.AddChild(0); 
        ll.AddChild(1); 
        Assert.Contains(0, ll.GetChildren()); 
        Assert.Contains(1, ll.GetChildren()); 
        Assert.Equal(2, ll.GetChildren().Count); 
    }

    [Fact]
    public void LinearLayout_ShouldntStoreRemovedChildren() {
        LinearLayout ll = new LinearLayout("horizontal", "alignlow"); 
        ll.AddChild(0); 
        ll.AddChild(1);
        ll.AddChild(2);
        ll.RemoveChild(0); 
        Assert.Contains(1, ll.GetChildren()); 
        Assert.DoesNotContain(0, ll.GetChildren()); 
        Assert.Equal(2, ll.GetChildren().Count); 
    }

    [Fact] 
    public void LinearLayout_ShouldntStoreDuplicateChildren() {
        LinearLayout ll = new LinearLayout("horizontal", "alignlow"); 
        ll.AddChild(0); 
        ll.AddChild(1); 
        bool added = ll.AddChild(0);
        Assert.False(added); 
        Assert.Equal(2, ll.GetChildren().Count);
        Assert.Contains(0, ll.GetChildren()); 
    }

    [Fact]
    public void LinearLayout_ShouldThrowOnBadArgument() {
        Assert.Throws<InvalidOperationException>(() => {
            LinearLayout ll = new LinearLayout("horzontal", "alignlow"); 
        });

        Assert.Throws<InvalidOperationException>(() => {
            LinearLayout ll = new LinearLayout("horizontal", "alignlw"); 
        }); 

        Assert.Throws<InvalidOperationException>(() => {
            LinearLayout ll = new LinearLayout("horizontal", "alignlow"); 
            ll.SetDirection("vertcal"); 
        }); 

        Assert.Throws<InvalidOperationException>(() => {
            LinearLayout ll = new LinearLayout("horizontal", "alignlow"); 
            ll.SetDirection("alignhgh"); 
        }); 
    }

    [Fact]
    public void LinearLayout_ShouldNotReturnMoreChildrenThanPageCount() {
        LinearLayout ll = new LinearLayout(usePaging: true, childrenPerPage: 1);
        ll.AddChild(0); 
        ll.AddChild(1); 
        Assert.Single(ll.GetChildren()); 
    }

    [Fact]
    public void LinearLayout_PageShouldSetChildrenToThoseInRange() {
        LinearLayout ll = new LinearLayout(usePaging: true, childrenPerPage: 2); 
        for (int i = 0; i < 10; i++) {
            ll.AddChild(i);
        }

        ll.Page(2); 
        Assert.Equal(2, ll.GetChildren()[0]); 
        Assert.Equal(3, ll.GetChildren()[1]); 

        ll.Page(-2); 
        Assert.Equal(0, ll.GetChildren()[0]); 
        Assert.Equal(1, ll.GetChildren()[1]); 
    }
}
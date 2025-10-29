using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 

public class DraggableTest {
    [Fact] 
    public void Draggable_ShouldDefaultToZeroRelativePositionAndZeroSnap() {
        Draggable d = new Draggable(); 
        Assert.Equal(Vector2.Zero, d.RelativeClickPosition); 
        Assert.Equal(Vector2.Zero, d.SnapPosition); 
    }

    [Fact] 
    public void Draggable_ShouldAcceptSnapPositionArgument() {
        Draggable d = new Draggable(new Vector2(1f, 1f)); 
        Assert.Equal(1f, d.SnapPosition.X); 
        Assert.Equal(1f, d.SnapPosition.Y); 
    }

    [Fact] 
    public void Draggable_ShouldDefaultToReleased() {
        Draggable d = new Draggable(); 
        Assert.True(d.IsReleased()); 
    }

    [Fact] 
    public void Draggable_ShouldNotBeDroppedIfNotFirstHeld() {
        Draggable d = new Draggable(); 
        d.Drop(); 
        Assert.True(d.IsReleased()); 
        Assert.False(d.IsBeingDropped()); 
        Assert.False(d.IsHeld()); 
    }

    [Fact] 
    public void Draggable_ShouldRespectStateChanges() {
        Draggable d = new Draggable(); 
        
        d.PickUp(); 
        Assert.True(d.IsHeld()); 
        Assert.False(d.IsReleased()); 
        Assert.False(d.IsBeingDropped()); 

        d.Drop(); 
        Assert.True(d.IsBeingDropped()); 
        Assert.False(d.IsReleased()); 
        Assert.False(d.IsHeld()); 

        d.Release(); 
        Assert.True(d.IsReleased()); 
        Assert.False(d.IsBeingDropped()); 
        Assert.False(d.IsHeld()); 
    }
}
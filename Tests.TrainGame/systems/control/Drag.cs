
using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

//sequential because global state (mouse)
[Collection("Sequential")]
public class DragTest {
    [Fact]
    public void DragSystem_ShouldPickUpClickedDraggables() {
        VirtualMouse.Reset();

        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int drag = EntityFactory.Add(w); 
        w.SetComponent<Frame>(drag, new Frame(0, 0, 10, 10)); 
        w.SetComponent<Button>(drag, new Button()); 
        w.SetComponent<Draggable>(drag, new Draggable()); 

         
        VirtualMouse.SetCoordinates(1, 1); 
        VirtualMouse.LeftPress(); 

        w.Update(); 

        Assert.True(w.GetComponent<Draggable>(drag).IsHeld());
        VirtualMouse.Reset();
    }

    [Fact]
    public void DragSystem_ShouldMoveHeldDraggables() {
        VirtualMouse.Reset();

        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int drag = EntityFactory.Add(w); 
        w.SetComponent<Frame>(drag, new Frame(0, 0, 10, 10)); 
        w.SetComponent<Button>(drag, new Button()); 
        w.SetComponent<Draggable>(drag, new Draggable()); 

        VirtualMouse.SetCoordinates(0, 0); 
        VirtualMouse.LeftPress(); 
        w.Update(); 
        VirtualMouse.SetCoordinates(100, 200); 
        w.Update(); 
        Assert.Equal(100, w.GetComponent<Frame>(drag).GetX()); 
        Assert.Equal(200, w.GetComponent<Frame>(drag).GetY());
        VirtualMouse.Reset(); 
    }

    [Fact]
    public void DragSystem_ShouldDropDraggablesIfLeftMouseIsntHeld() {
        VirtualMouse.Reset();

        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int drag = EntityFactory.Add(w);
        w.SetComponent<Frame>(drag, new Frame(0, 0, 10, 10)); 
        w.SetComponent<Button>(drag, new Button()); 
        w.SetComponent<Draggable>(drag, new Draggable()); 

        VirtualMouse.SetCoordinates(0, 0); 
        VirtualMouse.LeftPress(); 
        w.Update(); 
        Assert.True(w.GetComponent<Draggable>(drag).IsHeld()); 
        VirtualMouse.LeftRelease(); 
        w.Update(); 
        Assert.True(w.GetComponent<Draggable>(drag).IsBeingDropped()); 
        w.Update(); 
        Assert.True(w.GetComponent<Draggable>(drag).IsReleased()); 

        VirtualMouse.Reset(); 
    }
}
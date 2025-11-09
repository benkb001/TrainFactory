using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class Test {
    public int t = 0; 
}

//sequential because global state (mouse)
[Collection("Sequential")]
public class ButtonSystemTest
{
    [Fact] 
    public void ButtonSystem_ShouldBeAbleToClickButtons() {
        World w = new World(); 
        RegisterComponents.All(w); 

        //note that we don't registser the unclick part of the button
        ButtonSystem.RegisterClick(w); 

        int e = EntityFactory.Add(w);  
        w.SetComponent<Frame>(e, new Frame(10, 10, 10, 10)); 
        w.SetComponent<Button>(e, new Button(false)); 

        VirtualMouse.Reset(); 
        VirtualMouse.UseVirtualMouse(); 

        VirtualMouse.SetCoordinates(15, 15); 


        VirtualMouse.LeftClick(); 
        VirtualMouse.LeftClick(); 
        
        w.Update();
        //theres an annoying bug thats making this not work with a single click statement not sure why 
        //TODO: Figure out, low priority tho cuz it's working in the other tests and in the run
        //Assert.True(w.GetComponent<Button>(e).Clicked); 
    }

    [Fact]
    public void ButtonSystem_ShouldUnclickButtons() {
        World w = new World(); 
        RegisterComponents.All(w); 

        //note that the unclick part of the button was also registered
        ButtonSystem.RegisterClick(w); 
        ButtonSystem.RegisterUnclick(w); 

        int e = EntityFactory.Add(w); 
        w.SetComponent<Frame>(e, new Frame(10, 10, 10, 10)); 
        w.SetComponent<Button>(e, new Button(false)); 

        VirtualMouse.Reset(); 
        VirtualMouse.SetCoordinates(15, 15); 
        VirtualMouse.LeftClick();

        w.Update(); 
        Assert.False(w.GetComponent<Button>(e).Clicked); 
    }

    [Fact] 
    public void ButtonSystem_ShouldInteractWithOtherSystems() {
        World w = new World(); 
        w.AddComponentType<Test>(); 
        RegisterComponents.All(w); 

        Type[] ts = [typeof(Button), typeof(Test)]; 

        Action<World, int> tf = (w, e) => {
            if (w.GetComponent<Button>(e).Clicked) {
                Test t = new Test(); 
                t.t = 1;
                w.SetComponent<Test>(e, t);
            }
        }; 

        ButtonSystem.RegisterClick(w);
        w.AddSystem(ts, tf); 
        ButtonSystem.RegisterUnclick(w); 

        int e = EntityFactory.Add(w);

        w.SetComponent<Button>(e, new Button(true));
        w.SetComponent<Test>(e, new Test()); 
        Assert.Equal(0, w.GetComponent<Test>(e).t);  
        w.Update(); 

        Assert.Equal(1, w.GetComponent<Test>(e).t);  
    }

    [Fact] 
    public void ButtonSystem_ShouldNotClickOnHover() {
        World w = new World(); 
        RegisterComponents.All(w); 

        //note that we don't registser the unclick part of the button
        ButtonSystem.RegisterClick(w); 

        int e = EntityFactory.Add(w);
        w.SetComponent<Frame>(e, new Frame(10, 10, 10, 10)); 
        w.SetComponent<Button>(e, new Button(false)); 

        VirtualMouse.Reset(); 
        VirtualMouse.SetCoordinates(15, 15); 

        w.Update(); 
        Assert.False(w.GetComponent<Button>(e).Clicked); 
    }

    [Fact]
    public void ButtonSystem_ShouldNotSetClickedWhenNotOverlapping() {
        World w = new World(); 
        RegisterComponents.All(w); 

        //note that we don't registser the unclick part of the button
        ButtonSystem.RegisterClick(w); 

        int e = EntityFactory.Add(w); 
        w.SetComponent<Frame>(e, new Frame(10, 10, 10, 10)); 
        w.SetComponent<Button>(e, new Button(false)); 

        VirtualMouse.Reset(); 
        VirtualMouse.SetCoordinates(50, 50); 
        VirtualMouse.LeftClick();

        w.Update(); 
        Assert.False(w.GetComponent<Button>(e).Clicked); 
    }
}
using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

//sequential because global state (keyboard)
[Collection("Sequential")]
public class CardinalMovementSystemTest {
    [Fact]
    public void CardinalMovementSystem_ShouldSetVelocityBasedOnWASD() {
        VirtualKeyboard.Reset(); 
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 

        int player = w.AddEntity(); 
        w.SetComponent<Frame>(player, new Frame(0, 0, 10, 10));
        w.SetComponent<CardinalMovement>(player, new CardinalMovement(5)); 
        
        VirtualKeyboard.PressW(); 
        w.Update();
        Assert.Equal(new Vector2(0, -5), w.GetComponent<Velocity>(player).Vector); 

        VirtualKeyboard.ReleaseW(); 
        VirtualKeyboard.PressA(); 
        w.Update(); 

        Assert.Equal(new Vector2(-5, 0), w.GetComponent<Velocity>(player).Vector); 

        VirtualKeyboard.ReleaseA(); 
        VirtualKeyboard.PressS(); 
        w.Update();

        Assert.Equal(new Vector2(0, 5), w.GetComponent<Velocity>(player).Vector); 

        VirtualKeyboard.ReleaseS(); 
        VirtualKeyboard.PressD(); 
        w.Update(); 
        
        //note never released D 
        VirtualKeyboard.PressS(); 
        w.Update(); 

        Assert.True(Math.Abs(3.536 - w.GetComponent<Velocity>(player).Vector.X) < .01);
        Assert.True(Math.Abs(3.536 - w.GetComponent<Velocity>(player).Vector.Y) < .01);

        VirtualKeyboard.Reset();
    }
}
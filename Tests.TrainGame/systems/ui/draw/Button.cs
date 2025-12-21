using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Systems; 

public class TestButton : IClickable {
    public bool Clicked = false; 

    public string GetText() {
        return "test"; 
    }
}

public class DrawButtonSystemTest {
    [Fact]
    public void DrawButtonSystem_ShouldDrawButton() {
        World w = WorldFactory.Build(); 
        w.AddComponentType<TestButton>(); 
        w.AddComponentType<DrawButtonMessage<TestButton>>(); 
        DrawButtonSystem.Register<TestButton>(w); 

        TestButton btn = new TestButton(); 

        DrawButtonMessage<TestButton> dm = new DrawButtonMessage<TestButton>(
            Position: Vector2.Zero, 
            Width: 10f, 
            Height: 10f, 
            Button: btn
        ); 

        int e = w.AddEntity(); 
        w.SetComponent<DrawButtonMessage<TestButton>>(e, dm);

        w.Update(); 

        Assert.Single(w.GetMatchingEntities([typeof(TestButton), typeof(Frame)])); 
    }

    [Fact]
    public void DrawButtonSystem_ShouldRemoveMessageEntity() {
        World w = WorldFactory.Build(); 
        w.AddComponentType<TestButton>(); 
        w.AddComponentType<DrawButtonMessage<TestButton>>(); 
        DrawButtonSystem.Register<TestButton>(w); 

        TestButton btn = new TestButton(); 

        DrawButtonMessage<TestButton> dm = new DrawButtonMessage<TestButton>(
            Position: Vector2.Zero, 
            Width: 10f, 
            Height: 10f, 
            Button: btn
        ); 

        int e = w.AddEntity(); 
        w.SetComponent<DrawButtonMessage<TestButton>>(e, dm);

        w.Update(); 

        Assert.False(w.EntityExists(e)); 
    }
}
using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 

public class MovementTest {
    [Fact]
    public void MovementSystem_ShouldAllowEntitiesToMove() {
        World w = new World(); 
        RegisterComponents.All(w); 
        MovementSystem.Register(w); 

        int x = 10; 
        int y = 10; 
        int width = 5; 
        int height = 5; 

        Frame f1 = new Frame(x, y, width, height); 
        Frame f2 = new Frame(x + 10, y, width, height);

        int dx = 0; 
        int dy = 10; 

        Velocity v1 = new Velocity(dx, dy); 

        int e1 = w.AddEntity(); 
        int e2 = w.AddEntity(); 

        w.SetComponent<Velocity>(e1, v1); 

        w.SetComponent<Frame>(e1, f1); 
        w.SetComponent<Frame>(e2, f2);

        Collidable c = Collidable.Get(); 

        w.SetComponent<Collidable>(e1, c); 
        w.SetComponent<Collidable>(e2, c); 


        w.Update(); 

        Assert.Equal(x + dx, w.GetComponent<Frame>(e1).GetX());
        Assert.Equal(y + dy, w.GetComponent<Frame>(e1).GetY()); 
    }

    [Fact]
    public void MovementSystem_ShouldAllowEntitiesToMoveWithZeroPixelGaps() {
        World w = new World(); 
        RegisterComponents.All(w); 
        MovementSystem.Register(w); 

        int x = 10; 
        int y = 10; 
        int width = 5; 
        int height = 5; 

        Frame f1 = new Frame(x + width, y, width, height); 
        Frame f2 = new Frame(x, y, width, height);

        int dx = 0; 
        int dy = 1; 

        Velocity v1 = new Velocity(dx, dy); 

        int e1 = w.AddEntity(); 
        int e2 = w.AddEntity(); 

        w.SetComponent<Velocity>(e1, v1); 

        w.SetComponent<Frame>(e1, f1); 
        w.SetComponent<Frame>(e2, f2);

        Collidable c = Collidable.Get(); 

        w.SetComponent<Collidable>(e1, c); 
        w.SetComponent<Collidable>(e2, c); 

        w.Update(); 

        Assert.Equal(x + width + dx, w.GetComponent<Frame>(e1).GetX());
        Assert.Equal(y + dy, w.GetComponent<Frame>(e1).GetY()); 
    }

    [Fact]
    public void MovementSystem_ShouldNotAllowEntitiesToCollide() {
        World w = new World(); 
        RegisterComponents.All(w); 
        MovementSystem.Register(w); 

        int x = 10; 
        int y = 10; 
        int width = 5; 
        int height = 5; 

        Frame f1 = new Frame(0, y, width, height); 
        Frame f2 = new Frame(x, y, width, height);

        int dx = 6; 
        int dy = 0; 

        //f1 is 5 pixels to the left of f2 with a velocity of 6 pixels to the right 
        //the system should push it up against f2 without overlap
        Velocity v1 = new Velocity(dx, dy); 

        int e1 = w.AddEntity(); 
        int e2 = w.AddEntity(); 

        w.SetComponent<Velocity>(e1, v1); 

        w.SetComponent<Frame>(e1, f1); 
        w.SetComponent<Frame>(e2, f2);

        Collidable c = Collidable.Get(); 

        w.SetComponent<Collidable>(e1, c); 
        w.SetComponent<Collidable>(e2, c); 

        w.Update(); 

        Assert.Equal(5, w.GetComponent<Frame>(e1).GetX());
    }

    [Fact]
    public void MovementSystem_ShouldHandleCollisionsWithDiagonalVelocity() {
        World w = new World(); 
        RegisterComponents.All(w); 
        MovementSystem.Register(w); 

        int x = 10; 
        int y = 10; 
        int width = 5; 
        int height = 5; 

        Frame f1 = new Frame(0, 0, width, height); 
        Frame f2 = new Frame(x, y, width, height);

        int dx = 6; 
        int dy = 6; 

        //f1 is 5 pixels to the left of f2 with a velocity of 6 pixels to the right 
        //and 5 pixels above f2 with a velocity of 6 pixels down

        //the system should first push it the full 6 pixels to the right, 
        //then push it only 5 pixels down
        Velocity v1 = new Velocity(dx, dy); 

        int e1 = w.AddEntity(); 
        int e2 = w.AddEntity(); 

        w.SetComponent<Velocity>(e1, v1); 

        w.SetComponent<Frame>(e1, f1); 
        w.SetComponent<Frame>(e2, f2);

        Collidable c = Collidable.Get(); 

        w.SetComponent<Collidable>(e1, c); 
        w.SetComponent<Collidable>(e2, c); 

        w.Update(); 

        Assert.Equal(6, w.GetComponent<Frame>(e1).GetX());
        Assert.Equal(5, w.GetComponent<Frame>(e1).GetY()); 
    }

    [Fact]
    public void MovementSystem_ShouldHandleCollisionsWitBothEntitiesMoving() {
        World w = new World(); 
        RegisterComponents.All(w); 
        MovementSystem.Register(w); 

        int width = 5; 
        int height = 5; 

        //f1 is at 10 moving 10 to the right 
        //f2 is at 30 moving 10 to the left 
        //since f1 is added first, it should move to 20
        //so to avoid collision f2 should only move to 20 + width(f1) = 25
        Frame f1 = new Frame(10, 10, width, height); 
        Frame f2 = new Frame(30, 10, width, height);

        int dx1 = 10; 
        int dy1 = 0; 

        int dx2 = -10; 
        int dy2 = 0; 

        Velocity v1 = new Velocity(dx1, dy1); 
        Velocity v2 = new Velocity(dx2, dy2);

        int e1 = w.AddEntity(); 
        int e2 = w.AddEntity(); 

        w.SetComponent<Velocity>(e1, v1); 
        w.SetComponent<Velocity>(e2, v2); 
        w.SetComponent<Frame>(e1, f1);      
        w.SetComponent<Frame>(e2, f2);

        Collidable c = Collidable.Get(); 

        w.SetComponent<Collidable>(e1, c); 
        w.SetComponent<Collidable>(e2, c); 

        w.Update(); 

        Assert.Equal(25, w.GetComponent<Frame>(e2).GetX()); 
        Assert.Equal(20, w.GetComponent<Frame>(e1).GetX()); 
    }

    [Fact]
    public void MovementSystem_ShouldHandleCollisionsInAllFourCardinalDirections() {
        World w = new World(); 
        RegisterComponents.All(w); 
        MovementSystem.Register(w); 

        int width = 5; 
        int height = 5; 

        Frame f1 = new Frame(30, 30, width, height); 
        Frame f_above = new Frame(30, 20, width, height); 
        Frame f_below = new Frame(30, 40, width, height); 
        Frame f_left = new Frame(20, 30, width, height); 
        Frame f_right = new Frame(40, 30, width, height); 

        Velocity v_above = new Velocity(0, 10); 
        Velocity v_below = new Velocity(0, -10); 
        Velocity v_left = new Velocity(10, 0); 
        Velocity v_right = new Velocity(-10, 0); 

        Collidable c = Collidable.Get(); 

        int e1 = w.AddEntity(); 
        int e_above = w.AddEntity(); 
        int e_below = w.AddEntity(); 
        int e_left = w.AddEntity(); 
        int e_right = w.AddEntity(); 

        w.SetComponent<Collidable>(e1, c);
        w.SetComponent<Collidable>(e_above, c);
        w.SetComponent<Collidable>(e_below, c);
        w.SetComponent<Collidable>(e_left, c);
        w.SetComponent<Collidable>(e_right, c);

        w.SetComponent<Frame>(e1, f1); 
        w.SetComponent<Frame>(e_below, f_below); 
        w.SetComponent<Frame>(e_above, f_above); 
        w.SetComponent<Frame>(e_left, f_left); 
        w.SetComponent<Frame>(e_right, f_right);

        w.SetComponent<Velocity>(e_above, v_above);
        w.SetComponent<Velocity>(e_below, v_below); 
        w.SetComponent<Velocity>(e_left, v_left); 
        w.SetComponent<Velocity>(e_right, v_right); 
        
        w.Update(); 

        Assert.Equal(25, w.GetComponent<Frame>(e_above).GetY()); 
        Assert.Equal(35, w.GetComponent<Frame>(e_below).GetY());
        Assert.Equal(25, w.GetComponent<Frame>(e_left).GetX());
        Assert.Equal(35, w.GetComponent<Frame>(e_right).GetX());
    }

    [Fact]
    public void MovementSystem_ShouldStopMovingEntitiesOnCollisionsByDirection() {
        //should stop an object that collides horizontally from moving horizontally, 
        //but should not stop it from moving vertically, and vice-versa

        World w = new World(); 
        RegisterComponents.All(w); 
        MovementSystem.Register(w); 

        int width = 5; 
        int height = 5; 

        Frame f1 = new Frame(0, 0, width, height); 
        Frame f2 = new Frame(10, -500, 1000, 1000);

        int dx = 20; 
        int dy = 10;

        Velocity v1 = new Velocity(dx, dy); 

        int e1 = w.AddEntity(); 
        int e2 = w.AddEntity(); 

        w.SetComponent<Velocity>(e1, v1); 

        w.SetComponent<Frame>(e1, f1); 
        w.SetComponent<Frame>(e2, f2);

        Collidable c = Collidable.Get(); 

        w.SetComponent<Collidable>(e1, c); 
        w.SetComponent<Collidable>(e2, c); 

        w.Update(); 
        
        Assert.Equal(5, w.GetComponent<Frame>(e1).GetX());
        Assert.Equal(10, w.GetComponent<Frame>(e1).GetY());
        Velocity v_test = w.GetComponent<Velocity>(e1); 
        Assert.Equal(0f, v_test.Vector.X); 
        Assert.Equal(10f, v_test.Vector.Y); 
    }
}
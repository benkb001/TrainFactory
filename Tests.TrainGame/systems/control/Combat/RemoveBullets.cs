using TrainGame.Systems; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public class RemoveBulletSystemTest {
    [Fact]
    public void RemoveBulletSystem_ShouldRemoveBulletsAfterEnoughFramesHavePassed() {
        World w = WorldFactory.Build(); 

        int bulletEnt = EntityFactory.Add(w); 
        Bullet b = new Bullet(); 

        w.SetComponent<Bullet>(bulletEnt, b); 

        for (int i = 0; i < b.MaxFramesActive; i++) {
            w.Update(); 
        }

        w.Update(); 

        Assert.False(w.EntityExists(bulletEnt)); 
    }

    [Fact]
    public void RemoveBulletSystem_ShouldRemoveBulletsThatCollideWithWalls() {
        World w = WorldFactory.Build(); 

        int wallEnt = EntityFactory.AddUI(w, Vector2.Zero, 100, 100); 
        int bulletEnt = EntityFactory.AddUI(w, new Vector2(1, 1), 10, 10); 
        w.SetComponent<Bullet>(bulletEnt, new Bullet()); 
        w.SetComponent<Collidable>(wallEnt, new Collidable()); 

        w.Update(); 

        Assert.False(w.EntityExists(bulletEnt)); 
    }
}
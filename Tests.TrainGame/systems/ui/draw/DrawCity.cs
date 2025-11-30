
using System.Collections.Generic;
using System; 
using System.Drawing; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class DrawCitySystemTest {
    [Fact]
    public void DrawCitySystem_ShouldRemoveAllNonCameraReturnEntitiesFromMaxScene() {
        World w = WorldFactory.Build(); 
        int e = EntityFactory.Add(w); 
        int crEntity = EntityFactory.Add(w); 
        w.SetComponent<CameraReturn>(crEntity, new CameraReturn(Vector2.Zero, 0f)); 
        
        int pushEntity = EntityFactory.Add(w); 
        w.SetComponent<PushSceneMessage>(pushEntity, PushSceneMessage.Get()); 
        w.Update();

        City c = new City("Test", new Inventory("Test", 1, 1), 100f, 150f);

        int dm = EntityFactory.Add(w); 
        w.SetComponent<DrawCityMessage>(dm, new DrawCityMessage(c));
        w.Update(); 

        Assert.False(w.EntityExists(e));
        Assert.True(w.EntityExists(crEntity)); 
    }

    [Fact]
    public void DrawCitySystem_ShouldDrawRelativeToCameraReturn() {
        World w = WorldFactory.Build(); 
        int crEntity = EntityFactory.Add(w); 
        w.SetComponent<CameraReturn>(crEntity, new CameraReturn(new Vector2(2000, 2000), 0f)); 
        
        int pushEntity = EntityFactory.Add(w); 
        w.SetComponent<PushSceneMessage>(pushEntity, PushSceneMessage.Get()); 
        w.Update();

        City c = new City("Test", new Inventory("Test", 1, 1), 100f, 150f);
        int dm = EntityFactory.Add(w); 
        w.SetComponent<DrawCityMessage>(dm, new DrawCityMessage(c));
        w.Update(); 

        Frame f = w.GetComponentArray<Frame>().First().Value; 
        //because cameraReturn is the center of the screen, and x/y drawn relative to top-left, 
        //so 1500 instead of 2000
        Assert.True(f.GetX() >= 1500f);
        Assert.True(f.GetY() >= 1500f); 
    }

    [Fact]
    public void DrawCitySystem_ShouldDrawOnTheMaxScene() {
        World w = WorldFactory.Build(); 
        int crEntity = EntityFactory.Add(w); 
        w.SetComponent<CameraReturn>(crEntity, new CameraReturn(new Vector2(2000, 2000), 0f)); 
        
        int pushEntity = EntityFactory.Add(w); 
        w.SetComponent<PushSceneMessage>(pushEntity, PushSceneMessage.Get()); 
        w.Update();

        City c = new City("Test", new Inventory("Test", 1, 1), 100f, 150f);
        int dm = EntityFactory.Add(w); 
        w.SetComponent<DrawCityMessage>(dm, new DrawCityMessage(c));
        w.Update(); 

        Assert.Contains(w.GetComponentArray<Scene>(), kvp => {
            return kvp.Value.Value == 1 && w.ComponentContainsEntity<Frame>(kvp.Key); 
        }); 
    }
}
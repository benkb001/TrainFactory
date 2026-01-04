
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
public class EmbarkClickSystemTest {
    [Fact]
    public void EmbarkClickSystem_ShouldEmbarkClickedTrain() {
        VirtualMouse.Reset(); 
        World w = WorldFactory.Build(); 
        Inventory inv = new Inventory("Test", 1, 1); 
        City cStart = new City("start", inv, 100f, 100f);
        City cEnd = new City("end", inv, 100f, 100f);
        Train t = new Train(inv, cStart);
        Assert.Equal(cStart, t.GoingTo);
        Assert.False(t.IsTraveling()); 

        int embarkEntity = EntityFactory.Add(w); 
        w.SetComponent<Frame>(embarkEntity, new Frame(0, 0, 100, 100)); 
        w.SetComponent<Button>(embarkEntity, new Button()); 
        w.SetComponent<EmbarkButton>(embarkEntity, new EmbarkButton(cEnd, t));

        VirtualMouse.LeftClick(new Vector2(1, 1));
        w.Update(); 
        Assert.Equal(cEnd, t.GoingTo); 
        Assert.True(t.IsTraveling()); 
        VirtualMouse.Reset(); 
    }
}
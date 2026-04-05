
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

public class TrainClickSystemTest {
    [Fact]
    public void TrainClickSystem_ShouldDrawCityAndTrainInventories() {
        World w = WorldFactory.Build(); 
        
        Inventory trainInv = new Inventory("TrainInv", 1, 1); 
        Inventory cityInv = new Inventory("CityInv", 1, 1); 
        City c = new City("CTest", cityInv); 
        Train t = TrainWrap.GetTest();
        int trainEnt = TrainWrap.RegisterExisting(w, t, c);
        
        int tUIEntity = EntityFactory.Add(w); 
        w.SetComponent<Frame>(tUIEntity, new Frame(0, 0, 100, 100)); 
        w.SetComponent<Button>(tUIEntity, new Button(true)); 
        w.SetComponent<TrainUI>(tUIEntity, new TrainUI(t, trainEnt)); 
        w.SetComponent<ComingFromCity>(trainEnt, new ComingFromCity(c));
        
        w.Update(); 
        Assert.Equal(SceneType.TrainInterface, SceneSystem.CurrentScene);
    }
}

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

//sequential because global state (keyboard)
[Collection("Sequential")]
public class TrainClickSystemTest {
    [Fact]
    public void TrainClickSystem_ShouldDrawCityAndTrainInventoriesAndEmbarkView() {
        VirtualMouse.Reset(); 

        World w = WorldFactory.Build(); 
        
        Inventory trainInv = new Inventory("TrainInv", 1, 1); 
        Inventory cityInv = new Inventory("CityInv", 1, 1); 
        City c = new City("CTest", cityInv); 
        Train t = new Train(trainInv, c); 
        
        int tUIEntity = EntityFactory.Add(w); 
        w.SetComponent<Frame>(tUIEntity, new Frame(0, 0, 100, 100)); 
        w.SetComponent<Button>(tUIEntity, new Button()); 
        w.SetComponent<TrainUI>(tUIEntity, new TrainUI(t)); 
        
        VirtualMouse.LeftClick(new Vector2(1, 1)); 
        w.Update(); 

        List<int> es = w.GetMatchingEntities([typeof(Inventory), typeof(LinearLayout)]); 
        
        Assert.Equal(2, es.Count);
        Assert.Single(es, e => w.GetComponent<Inventory>(e) == trainInv);
        Assert.Single(es, e => w.GetComponent<Inventory>(e)  == cityInv); 

        VirtualMouse.Reset(); 
    }
}
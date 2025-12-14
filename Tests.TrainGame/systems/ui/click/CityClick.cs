
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
public class CityClickSystemTest {
    [Fact]
    public void CityClickSystem_ShouldMakeMessageToDrawInventoryTrainsAndMachines() {
        World w = new World(); 
        RegisterComponents.All(w); 
        CityClickSystem.Register(w); 

        int e = EntityFactory.Add(w); 

        City c = new City("Test", new Inventory("Test", 1, 1)); 
        CityUI cUI = new CityUI(c); 

        w.SetComponent<CityUI>(e, cUI); 
        w.SetComponent<Frame>(e, new Frame(0, 0, 10, 10)); 
        w.SetComponent<Button>(e, new Button(true));
        
        w.Update(); 
        
        //TODO: a way to specify between drawcallback types, 
        //or pick a different way to test
        List<int> invEs = w.GetMatchingEntities([typeof(DrawCallback)]); 
        Assert.Single(invEs);
        int invE = invEs[0]; 

        List<int> tvEs = w.GetMatchingEntities([typeof(DrawTrainsViewMessage)]); 
        Assert.Single(tvEs);
        int tvE = tvEs[0]; 

        List<int> mvEs = w.GetMatchingEntities([typeof(DrawMachinesViewMessage)]); 
        Assert.Single(mvEs);
        int mvE = mvEs[0]; 

        List<int> es = [invE, tvE, mvE]; 
        Assert.Distinct(es); 
    }
}
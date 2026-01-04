
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

public class CityClickSystemTest {
    [Fact]
    public void CityClickSystem_ShouldMakeDrawCityInterfaceMessage() {
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

        Assert.Single(w.GetComponentArray<DrawCityInterfaceMessage>()); 
    }
}
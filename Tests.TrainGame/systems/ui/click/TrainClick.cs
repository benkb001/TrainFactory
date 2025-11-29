
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

        World w = new World(); 
        RegisterComponents.All(w); 
        ButtonSystem.RegisterClick(w); 
        TrainClickSystem.Register(w); 
        
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

        DrawEmbarkMessage embarkMsg = w.GetComponent<DrawEmbarkMessage>(
            w.GetMatchingEntities([typeof(DrawEmbarkMessage)])[0]
        );

        List<DrawInventoryMessage> invMsgs = w.GetMatchingEntities([typeof(DrawInventoryMessage)]).Select(
            e => w.GetComponent<DrawInventoryMessage>(e)).ToList(); 
        
        Assert.Equal(t, embarkMsg.GetTrain());
        Assert.Single(invMsgs, msg => trainInv == msg.Inv);
        Assert.Single(invMsgs, msg => cityInv == msg.Inv); 

        VirtualMouse.Reset(); 
    }
}
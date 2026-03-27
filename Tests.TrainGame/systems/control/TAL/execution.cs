using TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 
using TrainGame.Utils; 

public class TALExecutionTest {
    
    private (World, TALBody<Train, City>, Train, City, City, City, int) init(string program) {
        World w = WorldFactory.Build(); 
        Inventory trainInv = new Inventory("Train", 2, 2); 
        Inventory cInv1 = new Inventory("City1", 2, 2); 
        Inventory cInv2 = new Inventory("City2", 2, 2); 
        Inventory cInv3 = new Inventory("City3", 2, 2); 
        
        City factory = new City(CityID.Factory, cInv1); 
        City mine = new City(CityID.Mine, cInv2); 
        City coast = new City(CityID.Coast, cInv3); 

        City[] cs = [factory, mine, coast]; 
        foreach (City c in cs) {
            int e = EntityFactory.Add(w, setData: true); 
            w.SetComponent<City>(e, c); 
        }

        factory.AddConnection(mine);

        Train t = new Train(trainInv, factory.RealPosition, new Dictionary<CartType, Inventory>(), "TestTrain"); 
        int trainEnt = EntityFactory.AddData<Train>(w, t);
        w.SetComponent<ComingFromCity>(trainEnt, new ComingFromCity(factory));
        TALBody<Train, City> ast = TAL.SetTrainProgram(program, t, trainEnt, w); 

        return (w, ast, t, factory, mine, coast, trainEnt);
    }
    

    [Fact]
    public void TALExecution_ExecutingGoShouldMakeTrainEmbarkTowardsDestination() {
        (World w, TALBody<Train, City> ast, Train t, City factory, City mine, City coast, int trainEnt) = init(@"
            GO TO Mine;
        ");

        ast.Execute(new TrainWorld(w)); 
        Assert.Equal(w.GetComponent<GoingToCity>(trainEnt), mine);
        Assert.True(t.IsTraveling()); 
    }

    [Fact]
    public void TALExecution_ExecutingGoShouldDoNothingIfCitiesAreNotConnected() {
        (World w, TALBody<Train, City> ast, Train t, City factory, City mine, City coas, int trainEnt) = init(@"
            GO TO Coast;
        ");

        ast.Execute(new TrainWorld(w)); 
        Assert.False(t.IsTraveling()); 
    }

    [Fact]
    public void TALExecution_LoadShouldTakeItemsFromCityInventory() {
        (World w, TALBody<Train, City> ast, Train t, City factory, City mine, City coast, int trainEnt) = init(@"
            LOAD 3 Iron;
        ");

        factory.Inv.Add(ItemID.Iron, 4);
        ast.Execute(new TrainWorld(w)); 

        Assert.Equal(3, InventoryWrap.ItemCount(t.GetInventories(), ItemID.Iron)); 
    }

    [Fact]
    public void TALExecution_UnloadShouldPlaceItemsAtCityInventory() {
        (World w, TALBody<Train, City> ast, Train t, City factory, City mine, City coast, int trainEnt) = init(@"
            UNLOAD 3 Iron;
        ");

        t.Inv.Add(ItemID.Iron, 4);
        ast.Execute(new TrainWorld(w)); 
        
        Assert.Equal(3, factory.Inv.ItemCount(ItemID.Iron)); 
    }   

    [Fact]
    public void TALExecution_WhileShouldRunUntilConditionIsFalse() {
        (World w, TALBody<Train, City> ast, Train t, City factory, City mine, City coast, int trainEnt) = init(@"
            WHILE Factory.Iron >= 1 {
                WAIT;
            }
            GO TO Mine;
        ");

        factory.Inv.Add(ItemID.Iron, 1);
        ast.Execute(new TrainWorld(w)); 
        
        Assert.False(t.IsTraveling()); 
        ast.Execute(new TrainWorld(w)); 
        Assert.False(t.IsTraveling()); 

        factory.Inv.Take(ItemID.Iron, 1); 
        ast.Execute(new TrainWorld(w)); 

        Assert.True(t.IsTraveling()); 
    }
}
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

    
    private (World, TALBody, Train, City, City, City) init(string program) {
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

        Train t = new Train(trainInv, factory); 
        TALBody ast = TAL.SetTrainProgram(program, t, w); 

        return (w, ast, t, factory, mine, coast);
    }
    

    [Fact]
    public void TALExecution_ExecutingGoShouldMakeTrainEmbarkTowardsDestination() {
        (World w, TALBody ast, Train t, City factory, City mine, City coast) = init(@"
            GO TO Mine;
        ");

        ast.Execute(w); 
        Assert.Equal(t.GoingTo, mine);
        Assert.True(t.IsTraveling()); 
    }

    [Fact]
    public void TALExecution_ExecutingGoShouldDoNothingIfCitiesAreNotConnected() {
        (World w, TALBody ast, Train t, City factory, City mine, City coast) = init(@"
            GO TO Coast;
        ");

        ast.Execute(w); 
        Assert.False(t.IsTraveling()); 
    }

    [Fact]
    public void TALExecution_LoadShouldTakeItemsFromCityInventory() {
        (World w, TALBody ast, Train t, City factory, City mine, City coast) = init(@"
            LOAD 3 Iron;
        ");

        factory.Inv.Add(ItemID.Iron, 4);
        ast.Execute(w); 

        Assert.Equal(3, InventoryWrap.ItemCount(t.GetInventories(), ItemID.Iron)); 
    }

    [Fact]
    public void TALExecution_UnloadShouldPlaceItemsAtCityInventory() {
        (World w, TALBody ast, Train t, City factory, City mine, City coast) = init(@"
            UNLOAD 3 Iron;
        ");

        t.Inv.Add(ItemID.Iron, 4);
        ast.Execute(w); 
        
        Assert.Equal(3, factory.Inv.ItemCount(ItemID.Iron)); 
    }   

    [Fact]
    public void TALExecution_WhileShouldRunUntilConditionIsFalse() {
        (World w, TALBody ast, Train t, City factory, City mine, City coast) = init(@"
            WHILE Factory.Iron >= 1 {
                WAIT;
            }
            GO TO Mine;
        ");

        factory.Inv.Add(ItemID.Iron, 1);
        ast.Execute(w); 
        
        Assert.False(t.IsTraveling()); 
        ast.Execute(w); 
        Assert.False(t.IsTraveling()); 

        factory.Inv.Take(ItemID.Iron, 1); 
        ast.Execute(w); 

        Assert.True(t.IsTraveling()); 
    }
}
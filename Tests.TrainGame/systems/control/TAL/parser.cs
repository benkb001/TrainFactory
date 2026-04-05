using System.IO; 

using Microsoft.Xna.Framework;

using TrainGame.Systems; 
using TrainGame.Constants; 
using TrainGame.ECS; 
using TrainGame.Components;

public class TALParserTest {

    private (City, int, Train, int, World) init() {
        World w = WorldFactory.Build(); 
        Inventory inv = new Inventory("Test", 2, 2); 
        City c = new City(CityID.Factory, inv); 
        Train t = new Train(inv, Vector2.Zero, new Dictionary<CartType, Inventory>(), "TestTrain");
        City mine = new City(CityID.Mine, inv); 
        
        
        int trainEnt = EntityFactory.Add(w, setData: true); 
        w.SetComponent<Train>(trainEnt, t); 
        int cityEnt = EntityFactory.Add(w, setData: true); 
        w.SetComponent<City>(cityEnt, c); 
        int mineEnt = EntityFactory.Add(w, setData: true); 
        w.SetComponent<City>(mineEnt, mine); 

        return (c, cityEnt, t, trainEnt, w); 
    }

    [Fact]
    public void TALParser_ParseIntExpressionShouldReturnAnIntWithTheCorrectValue() {
        (City c, int cEnt, Train t, int tEnt, World w) = init();

        List<TALToken> tokens = TALLexer.Tokenize("10"); 
        ITALExpression e = TALParser.ParseIntExp<Train, City>(tokens, new TrainWorld(w), t); 
        TALIntExpression intExp = Assert.IsType<TALIntExpression>(e);
        Assert.Equal(10, (int)intExp.Evaluate());
        
        tokens = TALLexer.Tokenize("10 + 20"); 
        e = TALParser.ParseIntExp<Train, City>(tokens, new TrainWorld(w), t); 
        TALAddExpression addExp = Assert.IsType<TALAddExpression>(e);
        Assert.Equal(30, (int)addExp.Evaluate());

        int ironCount = 10;
        t.Inv.Add(ItemID.Iron, ironCount);
        tokens = TALLexer.Tokenize($"{t.Id}.Iron"); 
        e = TALParser.ParseIntExp<Train, City>(tokens, new TrainWorld(w), t); 
        TALAccessTrainExpression<Train> accTrainExp = Assert.IsType<TALAccessTrainExpression<Train>>(e);
        Assert.Equal(ironCount, (int)accTrainExp.Evaluate());
        
        int woodCount = 20; 
        c.Inv.Add(ItemID.Wood, woodCount);
        tokens = TALLexer.Tokenize($"{CityID.Factory}.{ItemID.Wood}"); 
        e = TALParser.ParseIntExp<Train, City>(tokens, new TrainWorld(w), t); 
        TALAccessCityExpression<City> accCityExp = Assert.IsType<TALAccessCityExpression<City>>(e);
        Assert.Equal(woodCount, c.Inv.ItemCount(ItemID.Wood));
        Assert.Equal(woodCount, (int)accCityExp.Evaluate());
    }

    [Fact]
    public void TALParser_ParseIntExpressionShouldHandleNestedOperations() {
        (City c, int cEnt, Train t, int tEnt, World w) = init();
        List<TALToken> tokens = TALLexer.Tokenize($"(10 + 20) * (10 - (100 / 2))"); 
        ITALExpression e = TALParser.ParseIntExp<Train, City>(tokens, new TrainWorld(w), t); 
        TALMultiplyExpression multExp = Assert.IsType<TALMultiplyExpression>(e);
        Assert.Equal(-1200, (int)multExp.Evaluate());
    }

    [Fact]
    public void TALParser_ParseBooleanShouldReturnBooleanExpressionType() {
        (City c, int cEnt, Train t, int tEnt, World w) = init();
        List<TALToken> tokens = TALLexer.Tokenize($"true"); 
        ITALExpression e = TALParser.ParseBooleanExp<Train, City>(tokens, new TrainWorld(w), t); 
        TALBoolExpression boolExp = Assert.IsType<TALBoolExpression>(e);
        Assert.True((bool)boolExp.Evaluate());

        tokens = TALLexer.Tokenize($"10 > 20"); 
        e = TALParser.ParseBooleanExp<Train, City>(tokens, new TrainWorld(w), t); 
        TALGreaterExpression greaterExp = Assert.IsType<TALGreaterExpression>(e);
        Assert.False((bool)greaterExp.Evaluate());
    }

    [Fact]
    public void TALParser_ParseConditionalShouldReturnConditionalExpressionType() {
        (City c, int cEnt, Train t, int tEnt, World w) = init();
        List<TALToken> tokens = TALLexer.Tokenize($"true AND false"); 

        ITALExpression e = TALParser.ParseConditionalExp<Train, City>(tokens, new TrainWorld(w), t); 
        TALAndExpression andExp = Assert.IsType<TALAndExpression>(e);
        Assert.False((bool)andExp.Evaluate());
    }

    [Fact]
    public void TALParser_ParseProgramShouldConsumeAllTokens() {
        (City c, int cEnt, Train t, int tEnt, World w) = init();

        string program = @"
        
        WHILE TestTrain.Iron > 0 {
            UNLOAD TestTrain.Iron Iron; 
        }
        GO TO Mine; 
        WHILE Mine.Iron > 0 {
            LOAD Mine.Iron Iron; 
        }
        GO TO Factory; 
        WAIT; 
        WHILE TestTrain.Wood >= Mine.Iron {
            WAIT;
        }
        GO TO Mine; 
        UNLOAD 100 Wood; 
        GO TO Factory; 
        
        "; 
        List<TALToken> tokens = TALLexer.Tokenize(program); 
        TALParser.ParseProgram<Train, City>(tokens, new TrainWorld(w), t); 
        Assert.Empty(tokens); 
    }

    [Fact]
    public void TALParser_ParseProgramShouldReturnABodyWithOneInstructionForEachStatement() {
        (City c, int cEnt, Train t, int tEnt, World w) = init();
        string program = @"
            LOAD Factory.Fuel / 2 Fuel; 
            GO TO Mine; 
            UNLOAD TestTrain.Fuel Fuel; 
            WHILE (Mine.Fuel > 0) AND (Factory.Iron < 1000) {
                WAIT;
            }
            LOAD Mine.Iron Iron; 
            GO TO Factory; 
            UNLOAD TestTrain.Iron Iron; 
        ";
        TALBody<Train, City> ast = TALParser.ParseProgram<Train, City>(program, new TrainWorld(w), t); 
        Assert.Equal(7, ast.InstructionCount); 
    }

    [Fact]
    public void TALParser_ConditionalsWithParenthesesShouldEvaluateFirst() {
        (City c, int cEnt, Train t, int tEnt, World w) = init();
        c.Inv.Add(ItemID.Glass, 100);
        c.Inv.TakeAll(ItemID.Fuel);
        string program = $"({c.ID}.Fuel > 10) OR ({c.ID}.Glass < 100)";
        List<TALToken> toks = TALLexer.Tokenize(program);
        ITALExpression e = TALParser.ParseConditionalExp<Train, City>(toks, new TrainWorld(w), t);
        TALOrExpression orExp = Assert.IsType<TALOrExpression>(e);
        Assert.False((bool)orExp.Evaluate());
        c.Inv.TakeAll(ItemID.Glass);
        Assert.True((bool)orExp.Evaluate());
    }
}
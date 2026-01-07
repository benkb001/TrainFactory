using TrainGame.Systems; 
using TrainGame.Constants; 
using TrainGame.ECS; 
using TrainGame.Components;
using System.IO; 

public class TALParserTest {

    private (City, int, Train, int, World) init() {
        World w = WorldFactory.Build(); 
        Inventory inv = new Inventory("Test", 1, 1); 
        City c = new City(CityID.Factory, inv); 
        Train t = new Train(inv, c, Id: "TestTrain");
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
        TALExpression e = TALParser.ParseIntExp(tokens, w, t); 
        Assert.Equal(ExpressionType.Int, e.Type);
        Assert.Equal(10, e.IntVal);
        
        tokens = TALLexer.Tokenize("10 + 20"); 
        e = TALParser.ParseIntExp(tokens, w, t); 
        Assert.Equal(ExpressionType.Add, e.Type);
        Assert.Equal(10, e.E1.IntVal); 
        Assert.Equal(20, e.E2.IntVal); 
        Assert.Equal(ExpressionType.Int, e.E1.Type);
        Assert.Equal(ExpressionType.Int, e.E2.Type);

        tokens = TALLexer.Tokenize($"{t.Id}.Iron"); 
        e = TALParser.ParseIntExp(tokens, w, t); 
        Assert.Equal(AccessType.Train, e.AcType); 
        Assert.Equal(ExpressionType.Access, e.Type); 
        Assert.Equal(t, e.GetTrain());
        Assert.Equal("Iron", e.GetItemID()); 
        
        tokens = TALLexer.Tokenize($"{CityID.Factory}.Wood"); 
        e = TALParser.ParseIntExp(tokens, w, t); 
        Assert.Equal(AccessType.City, e.AcType); 
        Assert.Equal(ExpressionType.Access, e.Type); 
        Assert.Equal(c, e.GetCity());
        Assert.Equal("Wood", e.GetItemID()); 
    }

    [Fact]
    public void TALParser_ParseIntExpressionShouldHandleNestedOperations() {
        (City c, int cEnt, Train t, int tEnt, World w) = init();
        List<TALToken> tokens = TALLexer.Tokenize($"{t.Id}.{ItemID.Iron} + {c.Id}.{ItemID.Wood} * 10 - 100 / 2"); 
        TALExpression e = TALParser.ParseIntExp(tokens, w, t); 
        Assert.Equal(ExpressionType.Add, e.Type);
        Assert.Equal(ExpressionType.Access, e.E1.Type); 
        Assert.Equal(ExpressionType.Multiply, e.E2.Type); 
        Assert.Equal(ExpressionType.Access, e.E2.E1.Type); 
        Assert.Equal(ExpressionType.Subtract, e.E2.E2.Type); 
        Assert.Equal(ExpressionType.Int, e.E2.E2.E1.Type); 
        Assert.Equal(ExpressionType.Divide, e.E2.E2.E2.Type);
        Assert.Equal(ExpressionType.Int, e.E2.E2.E2.E1.Type); 
        Assert.Equal(ExpressionType.Int, e.E2.E2.E2.E2.Type); 
    }

    [Fact]
    public void TALParser_ParseBooleanShouldReturnBooleanExpressionType() {
        (City c, int cEnt, Train t, int tEnt, World w) = init();
        List<TALToken> tokens = TALLexer.Tokenize($"true"); 
        TALExpression e = TALParser.ParseBooleanExp(tokens, w, t); 

        Assert.Equal(ExpressionType.Bool, e.Type); 
        Assert.True(e.BoolVal); 

        tokens = TALLexer.Tokenize($"10 > 20"); 
        e = TALParser.ParseBooleanExp(tokens, w, t); 
        Assert.Equal(ExpressionType.Conditional, e.Type);
        Assert.Equal(ConditionType.Greater, e.Condition.Type);
        Assert.Equal(ExpressionType.Int, e.Condition.E1.Type); 
        Assert.Equal(10, e.Condition.E1.IntVal); 
        Assert.Equal(ExpressionType.Int, e.Condition.E2.Type); 
        Assert.Equal(20, e.Condition.E2.IntVal); 
    }

    [Fact]
    public void TALParser_ParseConditionalShouldReturnConditionalExpressionType() {
        (City c, int cEnt, Train t, int tEnt, World w) = init();
        List<TALToken> tokens = TALLexer.Tokenize($"true AND false"); 

        TALExpression e = TALParser.ParseConditionalExp(tokens, w, t); 

        Assert.Equal(ExpressionType.Conditional, e.Type); 
        Assert.Equal(ConditionType.And, e.Condition.Type);
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
        TALBody ast = TALParser.ParseProgram(tokens, w, t); 
        Assert.Empty(tokens); 
    }

    [Fact]
    public void TALParser_ParseProgramShouldReturnABodyWithOneInstructionForEachStatement() {
        (City c, int cEnt, Train t, int tEnt, World w) = init();
        string program = @"
            LOAD Factory.Fuel / 2 Fuel; 
            GO TO Mine; 
            UNLOAD TestTrain.Fuel Fuel; 
            WHILE Mine.Fuel > 0 {
                WAIT;
            }
            LOAD Mine.Iron Iron; 
            GO TO Factory; 
            UNLOAD TestTrain.Iron Iron; 
        ";
        TALBody ast = TALParser.ParseProgram(program, w, t); 
        Assert.Equal(7, ast.InstructionCount); 
    }
}
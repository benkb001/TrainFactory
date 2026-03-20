using TrainGame.Systems; 

using System;
using TrainGame.Components; 

public class TALConditionalTest {

    [Fact]
    public void TALConditional_EvaluateShouldReflectConditionalType() {

        TALBoolExpression eTrue = new TALBoolExpression(true);
        TALBoolExpression eFalse = new TALBoolExpression(false);

        TALAndExpression cAndFalse = new TALAndExpression(eTrue, eFalse); 
        Assert.False((bool)cAndFalse.Evaluate()); 

        TALAndExpression cAndTrue = new TALAndExpression(eTrue, eTrue); 
        Assert.True((bool)cAndTrue.Evaluate()); 

        TALEqualExpression cEqualFalse = new TALEqualExpression(eTrue, eFalse); 
        Assert.False((bool)cEqualFalse.Evaluate()); 

        TALEqualExpression cEqualTrue = new TALEqualExpression(eFalse, eFalse); 
        Assert.True((bool)cEqualTrue.Evaluate()); 

        TALIntExpression eInt10 = new TALIntExpression(10);
        TALIntExpression eInt0 = new TALIntExpression(0);

        TALEqualExpression c10Equal0 = new TALEqualExpression(eInt10, eInt0); 
        Assert.False((bool)c10Equal0.Evaluate()); 
        
        TALEqualExpression c10Equal10 = new TALEqualExpression(eInt10, eInt10); 
        Assert.True((bool)c10Equal10.Evaluate()); 

        TALGreaterExpression c10Greater0 = new TALGreaterExpression(eInt10, eInt0); 
        Assert.True((bool)c10Greater0.Evaluate()); 

        TALGreaterExpression c0Greater10 = new TALGreaterExpression(eInt0, eInt10); 
        Assert.False((bool)c0Greater10.Evaluate()); 

        TALGreaterEqualExpression c10GreaterEqual10 = new TALGreaterEqualExpression(eInt10, eInt10); 
        Assert.True((bool)c10GreaterEqual10.Evaluate()); 

        TALLessExpression c0Less10 = new TALLessExpression(eInt0, eInt10); 
        Assert.True((bool)c0Less10.Evaluate()); 

        TALLessEqualExpression c0LessEqual0 = new TALLessEqualExpression(eInt0, eInt0); 
        Assert.True((bool)c0LessEqual0.Evaluate()); 

        TALNotExpression cNotFalse = new TALNotExpression(eFalse); 
        Assert.True((bool)cNotFalse.Evaluate()); 

        TALNotEqualExpression c0NotEqual10 = new TALNotEqualExpression(eInt0, eInt10); 
        Assert.True((bool)c0NotEqual10.Evaluate()); 

        TALNotEqualExpression c0NotEqual0 = new TALNotEqualExpression(eInt0, eInt0); 
        Assert.False((bool)c0NotEqual0.Evaluate()); 

        TALOrExpression cTrueOrFalse = new TALOrExpression(eTrue, eFalse); 
        Assert.True((bool)cTrueOrFalse.Evaluate()); 
    }
}
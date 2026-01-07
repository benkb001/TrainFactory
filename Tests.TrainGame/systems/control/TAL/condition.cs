using TrainGame.Systems; 

using System;
using TrainGame.Components; 

public class TALConditionalTest {

    [Fact]
    public void TALConditional_EvaluateShouldReflectConditionalType() {
        TALConditional cTrue = new TALConditional(ConditionType.True);
        Assert.True(cTrue.Evaluate()); 

        TALConditional cFalse = new TALConditional(ConditionType.False);
        Assert.False(cFalse.Evaluate()); 

        TALExpression eTrue = TALExpression.Conditional(cTrue); 
        TALExpression eFalse = TALExpression.Conditional(cFalse); 

        TALConditional cAndFalse = new TALConditional(ConditionType.And, eTrue, eFalse); 
        Assert.False(cAndFalse.Evaluate()); 

        TALConditional cAndTrue = new TALConditional(ConditionType.And, eTrue, eTrue); 
        Assert.True(cAndTrue.Evaluate()); 

        TALConditional cEqualFalse = new TALConditional(ConditionType.Equal, eTrue, eFalse); 
        Assert.False(cEqualFalse.Evaluate()); 

        TALConditional cEqualTrue = new TALConditional(ConditionType.Equal, eFalse, eFalse); 
        Assert.True(cEqualTrue.Evaluate()); 

        TALExpression eInt10 = TALExpression.Int(10); 
        TALExpression eInt0 = TALExpression.Int(0); 

        TALConditional c10Equal0 = new TALConditional(ConditionType.Equal, eInt10, eInt0); 
        Assert.False(c10Equal0.Evaluate()); 
        
        TALConditional c10Equal10 = new TALConditional(ConditionType.Equal, eInt10, eInt10); 
        Assert.True(c10Equal10.Evaluate()); 

        TALConditional c10Greater0 = new TALConditional(ConditionType.Greater, eInt10, eInt0); 
        Assert.True(c10Greater0.Evaluate()); 

        TALConditional c0Greater10 = new TALConditional(ConditionType.Greater, eInt0, eInt10); 
        Assert.False(c0Greater10.Evaluate()); 

        TALConditional c10GreaterEqual10 = new TALConditional(ConditionType.GreaterEqual, eInt10, eInt10); 
        Assert.True(c10GreaterEqual10.Evaluate()); 

        TALConditional c0Less10 = new TALConditional(ConditionType.Less, eInt0, eInt10); 
        Assert.True(c0Less10.Evaluate()); 

        TALConditional c0LessEqual0 = new TALConditional(ConditionType.LessEqual, eInt0, eInt0); 
        Assert.True(c0LessEqual0.Evaluate()); 

        TALConditional cNotFalse = new TALConditional(ConditionType.Not, eFalse); 
        Assert.True(cNotFalse.Evaluate()); 

        TALConditional c0NotEqual10 = new TALConditional(ConditionType.NotEqual, eInt0, eInt10); 
        Assert.True(c0NotEqual10.Evaluate()); 

        TALConditional c0NotEqual0 = new TALConditional(ConditionType.NotEqual, eInt0, eInt0); 
        Assert.False(c0NotEqual0.Evaluate()); 

        TALConditional cTrueOrFalse = new TALConditional(ConditionType.Or, eTrue, eFalse); 
        Assert.True(cTrueOrFalse.Evaluate()); 

        Assert.False((new TALConditional(ConditionType.Or, eFalse, eFalse)).Evaluate()); 
    }
}
using TrainGame.Systems; 

using System;
using System.Linq;
using TrainGame.Components; 
using TrainGame.Constants;

public class TALExpressionTest {
    [Fact]
    public void TALExpression_IntegerMathShouldEvaluateCorrectly() {
        TALAddExpression e10Add10 = new TALAddExpression(new TALIntExpression(10), new TALIntExpression(10));
        Assert.Equal(20, (int)e10Add10.Evaluate());
        Assert.Equal(-30, (int)(new TALMultiplyExpression(new TALIntExpression(-5), new TALIntExpression(6)).Evaluate())); 
        Assert.Equal(-10, (int)(new TALDivideExpression(new TALIntExpression(-100), new TALIntExpression(1)).Evaluate()));
        Assert.Equal(0, (int)(new TALSubtractExpression(new TALIntExpression(10), new TALIntExpression(10)).Evaluate()));
    }
}
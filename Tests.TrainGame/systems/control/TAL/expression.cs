using TrainGame.Systems; 

using System;
using System.Linq;
using TrainGame.Components; 
using TrainGame.Constants;

public class TALExpressionTest {
    [Fact]
    public void TALExpression_EvaluateShouldChangeWithExpressionType() {
        TALExpression eTrue = TALExpression.Bool(true); 
        TALExpression eFalse = TALExpression.Bool(false); 
        Assert.True((bool)eTrue.Evaluate()); 
        Assert.False((bool)eFalse.Evaluate()); 

        TALExpression eInt10 = TALExpression.Int(10); 
        Assert.Equal(10, (int)eInt10.Evaluate()); 

        TALExpression eConditionalTrue = TALExpression.Conditional(new TALConditional(ConditionType.True)); 
        Assert.True((bool)eConditionalTrue.Evaluate()); 

        City cDefault = CityWrap.GetTest(); 
        TALExpression eCity = TALExpression.City(cDefault); 
        Assert.Equal(cDefault, eCity.Evaluate()); 

        cDefault.Inv.Add(ItemID.Iron, 10); 
        TALExpression eAccessCity = TALExpression.AccessCity(ItemID.Iron, cDefault);
        Assert.Equal(10, (int)eAccessCity.Evaluate());

        Train t = TrainWrap.GetTestTrain(); 
        Cart c = new Cart(CartType.Freight); 
        t.AddCart(c); 
        t.Carts[CartType.Freight].Add(ItemID.Wood, 20); 
        Assert.Equal(20, (int)TALExpression.AccessTrain(ItemID.Wood, t).Evaluate()); 

        Assert.Equal(ItemID.Iron, (string)TALExpression.ItemID(ItemID.Iron).Evaluate()); 

        TALExpression e10Add10 = TALExpression.Add(TALExpression.Int(10), TALExpression.Int(10)); 
        Assert.Equal(20, (int)e10Add10.Evaluate());
        Assert.Equal(-30, (int)TALExpression.Multiply(TALExpression.Int(-6), TALExpression.Int(5)).Evaluate());
        Assert.Equal(-10, (int)TALExpression.Divide(TALExpression.Int(100), TALExpression.Int(-10)).Evaluate());
        Assert.Equal(0, (int)TALExpression.Subtract(TALExpression.Int(20), TALExpression.Int(20)).Evaluate());
    }
}
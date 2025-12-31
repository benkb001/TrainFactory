namespace TrainGame.Systems; 

using System;
using TrainGame.Components; 

public enum ExpressionType {
    Conditional, 
    Access,
    Add,
    Subtract,
    Multiply,
    Divide, 
    Int, 
    Float,
    Bool, 
    Train, 
    City, 
    ItemID
}

public enum AccessType {
    Train, 
    City
}

public class TALExpression {
    private ExpressionType type; 
    private AccessType accessType; 
    private Train train; 
    private City city; 
    private string itemID; 
    private TALConditional condition; 
    private int i; 
    private bool b; 
    private float f; 
    private TALExpression e1; 
    private TALExpression e2; 
    
    public TALExpression(ExpressionType type) {
        this.type = type; 
        this.e1 = null; 
        this.e2 = null; 
    }

    public static TALExpression Bool(bool b) {
        TALExpression e = new TALExpression(ExpressionType.Bool); 
        e.b = b; 
        return e; 
    }

    public static TALExpression Int(int i) {
        TALExpression e = new TALExpression(ExpressionType.Int); 
        e.i = i; 
        return e; 
    }

    public static TALExpression Float(float f) {
        TALExpression e = new TALExpression(ExpressionType.Float); 
        e.f = f;
        return e; 
    }

    public static TALExpression Conditional(TALConditional c) {
        TALExpression e = new TALExpression(ExpressionType.Conditional); 
        e.condition = c; 
        return e; 
    }

    public static TALExpression Train(Train t) {
        TALExpression e = new TALExpression(ExpressionType.Train); 
        e.train = t; 
        return e; 
    }

    public static TALExpression City(City c) {
        TALExpression e = new TALExpression(ExpressionType.City); 
        e.city = c; 
        return e; 
    }

    public static TALExpression ItemID(string id) {
        TALExpression e = new TALExpression(ExpressionType.ItemID); 
        e.itemID = id; 
        return e; 
    }

    public static TALExpression AccessTrain(string itemID, Train train) {
        TALExpression e = new TALExpression(ExpressionType.Access); 
        e.accessType = AccessType.Train; 
        e.itemID = itemID; 
        e.train = train; 
        return e; 
    }

    public static TALExpression AccessCity(string itemID, City city) {
        TALExpression e = new TALExpression(ExpressionType.Access); 
        e.accessType = AccessType.City; 
        e.itemID = itemID; 
        e.city = city; 
        return e; 
    }

    public static TALExpression Add(TALExpression e1, TALExpression e2) {
        TALExpression e = new TALExpression(ExpressionType.Add); 
        e.e1 = e1; 
        e.e2 = e2; 
        return e; 
    }

    public static TALExpression Subtract(TALExpression e1, TALExpression e2) {
        TALExpression e = new TALExpression(ExpressionType.Subtract); 
        e.e1 = e1; 
        e.e2 = e2; 
        return e; 
    }

    public static TALExpression Multiply(TALExpression e1, TALExpression e2) {
        TALExpression e = new TALExpression(ExpressionType.Multiply); 
        e.e1 = e1; 
        e.e2 = e2; 
        return e; 
    }

    public static TALExpression Divide(TALExpression e1, TALExpression e2) {
        TALExpression e = new TALExpression(ExpressionType.Divide); 
        e.e1 = e1; 
        e.e2 = e2; 
        return e; 
    }

    public object Evaluate() {
        return type switch {
            ExpressionType.Bool => b, 
            ExpressionType.Int => i, 
            ExpressionType.Float => f,
            ExpressionType.Train => train, 
            ExpressionType.City => city, 
            ExpressionType.Add => (int)e1.Evaluate() + (int)e2.Evaluate(), 
            ExpressionType.Subtract => (int)e1.Evaluate() - (int)e2.Evaluate(), 
            ExpressionType.Divide => (int)e1.Evaluate() / (int)e2.Evaluate(), 
            ExpressionType.Multiply => (int)e1.Evaluate() * (int)e2.Evaluate(), 
            ExpressionType.Conditional => condition.Evaluate(), 
            ExpressionType.Access => accessType switch {
                AccessType.Train => train.Inv.ItemCount(itemID), 
                AccessType.City => city.Inv.ItemCount(itemID),
                _ => 0
            }, 
            ExpressionType.ItemID => itemID,
            _ => 0
        };
    }
}

namespace TrainGame.Systems; 

using System;
using System.Linq;
using TrainGame.Components; 

public enum ExpressionType {
    And, 
    Access,
    Add,
    Equal, 
    False,
    Greater, 
    GreaterEqual, 
    Less, 
    LessEqual, 
    Multiply,
    Not,
    NotEqual, 
    Or, 
    Subtract,
    True,
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
    private int i; 
    private bool b; 
    private float f; 
    private TALExpression e1; 
    private TALExpression e2; 

    public ExpressionType Type => type; 
    public AccessType AcType => accessType; 
    public int IntVal => i; 
    public bool BoolVal => b; 
    public TALExpression E1 => e1; 
    public TALExpression E2 => e2; 
    
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

    public static TALExpression Greater(TALExpression e1, TALExpression e2) {
        TALExpression e = new TALExpression(ExpressionType.Greater); 
        e.e1 = e1; 
        e.e2 = e2;
        return e; 
    }

    public static TALExpression GreaterEqual(TALExpression e1, TALExpression e2) {
        TALExpression e = new TALExpression(ExpressionType.GreaterEqual); 
        e.e1 = e1; 
        e.e2 = e2; 
        return e; 
    }


    public static TALExpression Less(TALExpression e1, TALExpression e2) {
        TALExpression e = new TALExpression(ExpressionType.Less); 
        e.e1 = e1; 
        e.e2 = e2; 
        return e; 
    }

    public static TALExpression LessEqual(TALExpression e1, TALExpression e2) {
        TALExpression e = new TALExpression(ExpressionType.LessEqual); 
        e.e1 = e1; 
        e.e2 = e2; 
        return e; 
    }

    public static TALExpression Equal(TALExpression e1, TALExpression e2) {
        TALExpression e = new TALExpression(ExpressionType.Equal); 
        e.e1 = e1; 
        e.e2 = e2; 
        return e; 
    }

    public static TALExpression Not(TALExpression e1) {
        TALExpression e = new TALExpression(ExpressionType.Not); 
        e.e1 = e1; 
        return e; 
    }

    public static TALExpression NotEqual(TALExpression e1, TALExpression e2) {
        TALExpression e = new TALExpression(ExpressionType.NotEqual); 
        e.e1 = e1; 
        e.e2 = e2; 
        return e; 
    }

    public static TALExpression And(TALExpression e1, TALExpression e2) {
        TALExpression e = new TALExpression(ExpressionType.And); 
        e.e1 = e1; 
        e.e2 = e2; 
        return e; 
    }

    public static TALExpression Or(TALExpression e1, TALExpression e2) {
        TALExpression e = new TALExpression(ExpressionType.Or); 
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
            ExpressionType.Access => accessType switch {
                AccessType.Train => InventoryWrap.ItemCount(train.GetInventories(), itemID), 
                AccessType.City => city.Inv.ItemCount(itemID),
                _ => 0
            }, 
            ExpressionType.ItemID => itemID,
            ExpressionType.And => ((bool)e1.Evaluate()) && ((bool)e2.Evaluate()), 
            ExpressionType.Equal => e1.Evaluate().Equals(e2.Evaluate()),
            ExpressionType.False => false, 
            ExpressionType.Greater => (int)e1.Evaluate() > (int)e2.Evaluate(), 
            ExpressionType.GreaterEqual => (int)e1.Evaluate() >= (int)e2.Evaluate(), 
            ExpressionType.Less => (int)e1.Evaluate() < (int)e2.Evaluate(), 
            ExpressionType.LessEqual => (int)e1.Evaluate() <= (int)e2.Evaluate(),  
            ExpressionType.Not => !(bool)e1.Evaluate(), 
            ExpressionType.NotEqual => !(e1.Evaluate().Equals(e2.Evaluate())),
            ExpressionType.Or => (bool)e1.Evaluate() || (bool)e2.Evaluate(), 
            ExpressionType.True => true,
            _ => 0
        };
    }

    public Train GetTrain() {
        return train; 
    }

    public City GetCity() {
        return city; 
    }

    public string GetItemID() {
        return itemID; 
    }
}

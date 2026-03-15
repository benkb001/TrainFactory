namespace TrainGame.Systems; 

using System;
using System.Linq;
using TrainGame.Components; 

public enum ExpressionType {
    Train, 
    City, 
    ItemID
}

public enum AccessType {
    Train, 
    City
}

public interface ITALExpression {
    object Evaluate();
}

public class TALBoolExpression : ITALExpression {
    
    bool b;

    public TALBoolExpression(bool b) {
        this.b = b;
    }

    public object Evaluate() {
        return b;
    }
}

public class TALAndExpression : ITALExpression {
    ITALExpression e1; 
    ITALExpression e2; 

    public TALAndExpression(ITALExpression e1, ITALExpression e2) {
        this.e1 = e1; 
        this.e2 = e2; 
    }

    public object Evaluate() {
        return (bool)e1.Evaluate() && (bool)e2.Evaluate();
    }
}

public class TALAccessTrainExpression<T> : ITALExpression where T : ITrain {
    private T train; 
    private string itemID;

    public TALAccessTrainExpression(T train, string itemID) {
        this.train = train;
        this.itemID = itemID;
    }

    public object Evaluate() {
        return train.ItemCount(itemID);
    }
}

public class TALAccessCityExpression<C> : ITALExpression where C : ICity {
    private C city; 
    private string itemID; 

    public TALAccessCityExpression(C city, string itemID) {
        this.city = city; 
        this.itemID = itemID;
    }

    public object Evaluate() {
        return city.ItemCount(itemID);
    }
}

public class TALAddExpression : ITALExpression {
    private ITALExpression e1; 
    private ITALExpression e2; 

    public TALAddExpression(ITALExpression e1, ITALExpression e2) {
        this.e1 = e1; 
        this.e2 = e2; 
    }

    public object Evaluate() {
        return (int)e1.Evaluate() + (int)e2.Evaluate();
    }
}
public class TALEqualExpression : ITALExpression {
    private ITALExpression e1; 
    private ITALExpression e2; 

    public TALEqualExpression(ITALExpression e1, ITALExpression e2) {
        this.e1 = e1; 
        this.e2 = e2; 
    }

    public object Evaluate() {
        return e1.Evaluate().Equals(e2.Evaluate());
    }
}

public class TALGreaterExpression : ITALExpression {
    private ITALExpression e1; 
    private ITALExpression e2; 

    public TALGreaterExpression(ITALExpression e1, ITALExpression e2) {
        this.e1 = e1; 
        this.e2 = e2; 
    }

    public object Evaluate() {
        return (int)e1.Evaluate() > (int)e2.Evaluate();
    }
}

public class TALLessExpression : ITALExpression {
    private ITALExpression e1; 
    private ITALExpression e2; 

    public TALLessExpression(ITALExpression e1, ITALExpression e2) {
        this.e1 = e1; 
        this.e2 = e2; 
    }

    public object Evaluate() {
        return (int)e1.Evaluate() < (int)e2.Evaluate();
    }
}

public class TALGreaterEqualExpression : ITALExpression {
    private ITALExpression e1; 
    private ITALExpression e2; 

    public TALGreaterEqualExpression(ITALExpression e1, ITALExpression e2) {
        this.e1 = e1; 
        this.e2 = e2; 
    }

    public object Evaluate() {
        return (int)e1.Evaluate() >= (int)e2.Evaluate();
    }
}

public class TALLessEqualExpression : ITALExpression {
    private ITALExpression e1; 
    private ITALExpression e2; 

    public TALLessEqualExpression(ITALExpression e1, ITALExpression e2) {
        this.e1 = e1; 
        this.e2 = e2; 
    }

    public object Evaluate() {
        return (int)e1.Evaluate() <= (int)e2.Evaluate();
    }
}

public class TALIntExpression : ITALExpression {
    int i; 

    public TALIntExpression(int i) {
        this.i = i;
    }

    public object Evaluate() {
        return i;
    }
}

public class TALSubtractExpression : ITALExpression {
    private ITALExpression e1; 
    private ITALExpression e2; 

    public TALSubtractExpression(ITALExpression e1, ITALExpression e2) {
        this.e1 = e1; 
        this.e2 = e2; 
    }

    public object Evaluate() {
        return (int)e1.Evaluate() - (int)e2.Evaluate();
    }
}

public class TALMultiplyExpression : ITALExpression {
    private ITALExpression e1; 
    private ITALExpression e2; 

    public TALMultiplyExpression(ITALExpression e1, ITALExpression e2) {
        this.e1 = e1; 
        this.e2 = e2; 
    }

    public object Evaluate() {
        return (int)e1.Evaluate() * (int)e2.Evaluate();
    }
}

public class TALDivideExpression : ITALExpression {
    private ITALExpression e1; 
    private ITALExpression e2; 

    public TALDivideExpression(ITALExpression e1, ITALExpression e2) {
        this.e1 = e1; 
        this.e2 = e2; 
    }

    public object Evaluate() {
        return (int)e1.Evaluate() / (int)e2.Evaluate();
    }
}

public class TALNotExpression : ITALExpression {
    private ITALExpression e1; 

    public TALNotExpression(ITALExpression e1) {
        this.e1 = e1; 
    }

    public object Evaluate() {
        return !((bool)e1.Evaluate());
    }
}

public class TALNotEqualExpression : ITALExpression {
    private ITALExpression e1; 
    private ITALExpression e2; 

    public TALNotEqualExpression(ITALExpression e1, ITALExpression e2) {
        this.e1 = e1; 
        this.e2 = e2; 
    }

    public object Evaluate() {
        return !(e1.Evaluate().Equals(e2.Evaluate()));
    }
}

public class TALOrExpression : ITALExpression {
    private ITALExpression e1; 
    private ITALExpression e2; 

    public TALOrExpression(ITALExpression e1, ITALExpression e2) {
        this.e1 = e1; 
        this.e2 = e2; 
    }

    public object Evaluate() {
        return (bool)e1.Evaluate() || (bool)e2.Evaluate();
    }
}

public class TALItemIDExpression : ITALExpression {
    string itemID;

    public TALItemIDExpression(string itemID) {
        this.itemID = itemID;
    }

    public object Evaluate() {
        return itemID;
    }
}

public class TALCityExpression<C> : ITALExpression where C : ICity {
    C city; 

    public TALCityExpression(C city) {
        this.city = city;
    }

    public object Evaluate() {
        return city;
    }
}

public class TALTrainExpression<T> : ITALExpression where T : ITrain {
    T train; 

    public TALTrainExpression(T train) {
        this.train = train;
    }

    public object Evaluate() {
        return train;
    }
}
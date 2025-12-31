namespace TrainGame.Systems; 

using System;
using TrainGame.Components; 

public enum ConditionType {
    And, 
    Equal, 
    False,
    Greater, 
    GreaterEqual, 
    Less, 
    LessEqual, 
    Not,
    NotEqual, 
    Or, 
    True
}

public class TALConditional {
    private ConditionType type; 
    private TALExpression e1; 
    private TALExpression e2; 

    public TALConditional(ConditionType type, TALExpression e1 = null, TALExpression e2 = null) {
        this.type = type; 
        this.e1 = e1; 
        this.e2 = e2; 
    }

    public bool Evaluate() {
        return type switch {
            ConditionType.And => (bool)e1.Evaluate() && (bool)e2.Evaluate(), 
            ConditionType.Equal => (bool)e1.Evaluate() == (bool)e2.Evaluate(), 
            ConditionType.False => false, 
            ConditionType.Greater => (int)e1.Evaluate() > (int)e2.Evaluate(), 
            ConditionType.GreaterEqual => (int)e1.Evaluate() > (int)e2.Evaluate(), 
            ConditionType.Less => (int)e1.Evaluate() < (int)e2.Evaluate(), 
            ConditionType.LessEqual => (int)e1.Evaluate() <= (int)e2.Evaluate(),  
            ConditionType.Not => !(bool)e1.Evaluate(), 
            ConditionType.NotEqual => e1.Evaluate() != e2.Evaluate(),
            ConditionType.Or => (bool)e1.Evaluate() || (bool)e2.Evaluate(), 
            ConditionType.True => true,
            _ => false
        };
    }
}
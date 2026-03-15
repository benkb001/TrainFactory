namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 
using TrainGame.Utils; 

public enum InstructionType {
    Go, 
    Wait, 
    Load, 
    Unload, 
    While
}

public enum InstructionEffect {
    IncrementNextInstruction,
    None,
    Pause,
    StopExecution
}

public interface ITALInstruction<T, C> 
    where T : ITrain 
    where C : ICity {
    List<InstructionEffect> Execute(T train, ITrainWorld<T, C> w);
}

public class TALWaitInstruction<T, C> : ITALInstruction<T, C> where T : ITrain 
    where C : ICity{
    public TALWaitInstruction() {}

    public List<InstructionEffect> Execute(T train, ITrainWorld<T, C> w) {
        return new List<InstructionEffect>() { InstructionEffect.IncrementNextInstruction, InstructionEffect.StopExecution };
    }
}

public class TALGoInstruction<T, C> : ITALInstruction<T, C> where T : ITrain 
    where C : ICity {
    private C city; 
    public C GetCity() => city; 
    
    public TALGoInstruction(C city) {
        this.city = city;
    }

    public List<InstructionEffect> Execute(T train, ITrainWorld<T, C> w) {
        TrainState s = w.Embark(train, city);
        List<InstructionEffect> es = new();
        
        es.Add( s switch {
            TrainState.AtCity => InstructionEffect.IncrementNextInstruction,
            TrainState.NoPath => InstructionEffect.Pause,
            TrainState.OnMidPath => InstructionEffect.None,
            TrainState.OnLastPath => InstructionEffect.IncrementNextInstruction,
            _ => InstructionEffect.Pause
        });
        es.Add(InstructionEffect.StopExecution);
        return es;
    }
}

public class TALLoadInstruction<T, C> : ITALInstruction<T, C> where T : ITrain 
    where C : ICity {
    public ITALExpression E1; 
    public ITALExpression E2; 

    public TALLoadInstruction(ITALExpression E1, ITALExpression E2) {
        this.E1 = E1; 
        this.E2 = E2; 
    }

    public List<InstructionEffect> Execute(T train, ITrainWorld<T, C> w) {
        int amount = (int)E1.Evaluate(); 
        string itemID = (string)E2.Evaluate(); 
        w.Load(train, itemID, amount);
        return new List<InstructionEffect>() { InstructionEffect.IncrementNextInstruction };
    }
}

public class TALUnloadInstruction<T, C> : ITALInstruction<T, C> where T : ITrain 
    where C : ICity {
    public ITALExpression E1; 
    public ITALExpression E2; 

    public TALUnloadInstruction(ITALExpression E1, ITALExpression E2) {
        this.E1 = E1; 
        this.E2 = E2; 
    }

    public List<InstructionEffect> Execute(T train, ITrainWorld<T, C> w) {
        int amount = (int)E1.Evaluate(); 
        string itemID = (string)E2.Evaluate(); 
        w.Unload(train, itemID, amount);
        return new List<InstructionEffect>() { InstructionEffect.IncrementNextInstruction };
    }
}

public class TALWhileInstruction<T, C> : ITALInstruction<T, C> where T : ITrain 
    where C : ICity {
    ITALExpression Condition;
    ITALBody<T, C> Body; 

    public TALWhileInstruction(ITALExpression Condition, ITALBody<T, C> body) {
        this.Condition = Condition; 
        this.Body = body;
    }

    public List<InstructionEffect> Execute(T train, ITrainWorld<T, C> w) {
        if ((bool)Condition.Evaluate()) {
            Body.Execute(w); 
            return new List<InstructionEffect>() { InstructionEffect.StopExecution };
        } else {
            return new List<InstructionEffect>() { InstructionEffect.IncrementNextInstruction };
        }
    }
}

public class TALBody<T, C> : ITALBody<T, C> where T : ITrain 
    where C : ICity {
    private List<ITALInstruction<T, C>> instructions; 
    private int nextInstruction; 
    private bool paused = false; 
    private T train; 

    public int InstructionCount => instructions.Count; 
    public int NextInstruction() => nextInstruction; 
    public bool Paused() => paused; 

    public TALBody(List<ITALInstruction<T, C>> instructions, T train, int nextInstruction = 0) {
        this.instructions = instructions; 
        this.train = train; 
        this.nextInstruction = nextInstruction;
    }

    public void Execute(ITrainWorld<T, C> w) {
        if (paused || train.IsTraveling()) {
            return; 
        }

        bool executing = true; 
        while (executing && !paused && nextInstruction < instructions.Count) {
            ITALInstruction<T, C> i = instructions[nextInstruction];
            foreach (InstructionEffect e in i.Execute(train, w)) {
                switch (e) {
                    case InstructionEffect.StopExecution: 
                        executing = false; 
                        break;
                    case InstructionEffect.IncrementNextInstruction: 
                        nextInstruction++; 
                        break;
                    case InstructionEffect.Pause: 
                        paused = true; 
                        break;
                    default: 
                        break;
                }
            }

            if (nextInstruction >= instructions.Count) {
                nextInstruction = 0; 
                executing = false; 
            }
        }
    }

    public void Pause() {
        paused = true; 
    }

    public void Unpause() {
        paused = false; 
    }
}
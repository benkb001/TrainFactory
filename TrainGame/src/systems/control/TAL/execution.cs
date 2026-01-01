namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

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

public class TALInstruction {
    public InstructionType Type;
    public City C; 
    public TALConditional Condition; 
    public TALExpression E1; 
    public TALExpression E2; 
    public TALBody Body; 

    public TALInstruction(InstructionType type) {
        this.Type = type; 
    }

    public static TALInstruction Go(City c) {
        TALInstruction i = new TALInstruction(InstructionType.Go); 
        i.C = c; 
        return i; 
    }

    public static TALInstruction Wait() {
        TALInstruction i = new TALInstruction(InstructionType.Wait); 
        return i; 
    }

    public static TALInstruction Load(TALExpression E1, TALExpression E2) {
        TALInstruction i = new TALInstruction(InstructionType.Load);
        i.E1 = E1; 
        i.E2 = E2;
        return i; 
    }

    public static TALInstruction Unload(TALExpression E1, TALExpression E2) {
        TALInstruction i = new TALInstruction(InstructionType.Unload);
        i.E1 = E1; 
        i.E2 = E2;
        return i; 
    }

    public static TALInstruction While(TALConditional Condition, TALBody Body) {
        TALInstruction i = new TALInstruction(InstructionType.While);
        i.Condition = Condition; 
        i.Body = Body; 
        return i; 
    }
}

public class TALBody {
    private List<TALInstruction> instructions; 
    private int nextInstruction; 
    public int InstructionCount => instructions.Count; 
    private Train train; 

    public TALBody(List<TALInstruction> instructions, Train train) {
        this.instructions = instructions; 
        this.train = train; 
        nextInstruction = 0; 
    }

    public void Execute(World w) {
        if (train.IsTraveling()) {
            return; 
        }

        City city; 
        int amount; 
        string itemID; 
        bool executing = true; 
        while (executing) {
            TALInstruction i = instructions[nextInstruction];
            switch (i.Type) {
                case InstructionType.Go: 
                    city = i.C; 
                    TrainWrap.Embark(train, city, w); 
                    nextInstruction++; 
                    executing = false; 
                    break; 

                case InstructionType.Load: 
                    amount = (int)i.E1.Evaluate(); 
                    itemID = (string)i.E2.Evaluate(); 
                    city = train.ComingFrom; 
                    city.Inv.TransferTo(train.Inv, itemID, amount);
                    nextInstruction++; 
                    break; 

                case InstructionType.Unload: 
                    amount = (int)i.E1.Evaluate(); 
                    itemID = (string)i.E2.Evaluate(); 
                    city = train.ComingFrom; 
                    train.Inv.TransferTo(city.Inv, itemID, amount);
                    nextInstruction++; 
                    break; 

                case InstructionType.Wait: 
                    nextInstruction++; 
                    break; 

                case InstructionType.While: 
                    if (i.Condition.Evaluate()) {
                        i.Body.Execute(w); 
                        executing = false; 
                    } else {
                        nextInstruction++; 
                    }

                    break; 
                    
                default: 
                    executing = false; 
                    break;
            }

            if (nextInstruction >= instructions.Count) {
                nextInstruction = 0; 
                executing = false; 
            }

        }
        
    }
}


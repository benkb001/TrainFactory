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

public class TALInstruction {
    public InstructionType Type;
    public City C; 
    public TALExpression E1; 
    public TALExpression E2; 
    public TALExpression Condition;
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

    public static TALInstruction While(TALExpression E1, TALBody Body) {
        //ICKY: Could do some assertion that E1 evalutes to a boolean
        TALInstruction i = new TALInstruction(InstructionType.While);
        i.Condition = E1; 
        i.Body = Body; 
        return i; 
    }
}

public class TALBody {
    private List<TALInstruction> instructions; 
    private int nextInstruction; 
    private bool paused = false; 
    private Train train; 

    public int InstructionCount => instructions.Count; 
    public int NextInstruction => nextInstruction; 
    public bool Paused => paused; 

    public TALBody(List<TALInstruction> instructions, Train train, int nextInstruction = 0) {
        this.instructions = instructions; 
        this.train = train; 
        this.nextInstruction = nextInstruction;
    }

    public void Execute(World w) {
        if (paused || train.IsTraveling()) {
            return; 
        }

        City city; 
        int amount; 
        string itemID; 
        bool executing = true; 
        while (executing && !paused && nextInstruction < instructions.Count) {
            TALInstruction i = instructions[nextInstruction];
            switch (i.Type) {
                case InstructionType.Go: 
                    
                    city = i.C; 

                    if (train.ComingFrom == city) {
                        nextInstruction++;
                        continue;
                    }

                    List<City> all = w
                    .GetMatchingEntities([typeof(City), typeof(Data)])
                    .Select(e => w.GetComponent<City>(e))
                    .ToList();

                    List<City> path = Util.ShortestPathUnweighted(all, train.ComingFrom, city); 

                    if (path != null && path.Count > 0) {
                        City next = path[0]; 
                        TrainWrap.Embark(train, next, w); 
                        executing = false; 
                        if (next == city) {
                            nextInstruction++;
                        }
                    } else {
                        //pause, there is no way for the train to get to its destination,
                        //as the player hasn't unlocked the required railroads yet
                        paused = true; 
                    }
                    
                    break; 

                case InstructionType.Load: 
                    amount = (int)i.E1.Evaluate(); 
                    itemID = (string)i.E2.Evaluate(); 
                    city = train.ComingFrom; 
                    city.Inv.TransferTo(train.GetInventories(), itemID, amount);
                    nextInstruction++; 
                    break; 

                case InstructionType.Unload: 
                    amount = (int)i.E1.Evaluate(); 
                    itemID = (string)i.E2.Evaluate(); 
                    city = train.ComingFrom; 
                    city.Inv.TransferFrom(train.GetInventories(), itemID, amount); 
                    nextInstruction++; 
                    break; 

                case InstructionType.Wait: 
                    nextInstruction++; 
                    executing = false;
                    break; 

                case InstructionType.While: 
                    if ((bool)i.Condition.Evaluate()) {
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

    public void Pause() {
        paused = true; 
    }

    public void Unpause() {
        paused = false; 
    }
}


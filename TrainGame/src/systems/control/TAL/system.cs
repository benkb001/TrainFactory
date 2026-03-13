namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Constants; 
using TrainGame.Utils; 
using TrainGame.Callbacks; 

public class TALExecutionSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Train), typeof(Data)], (w, e) => { 
            Train t = w.GetComponent<Train>(e); 
            if (t.Executable != null) {
                t.Executable.Execute(w); 
            }
        });
    }
}

public static class TAL {

    private static List<(string, string, string, int)> programData = new() {
        (ItemID.Iron, CityID.Mine, CityID.Factory, 2),
        (ItemID.Water, CityID.Coast, CityID.Factory, 10),
        (ItemID.Sand, CityID.Coast, CityID.Factory, 1),
        (ItemID.Fuel, CityID.Refinery, CityID.Factory, 10),
        (ItemID.Wood, CityID.Greenhouse, CityID.Factory, 1),
        (ItemID.Oil, CityID.Reservoir, CityID.Refinery, 2),
        (ItemID.Water, CityID.Coast, CityID.Refinery, 2),
        (ItemID.Iron, CityID.Mine, CityID.Refinery, 10),
        (ItemID.Water, CityID.Coast, CityID.Greenhouse, 2),
        (ItemID.Oil, CityID.Reservoir, CityID.Factory, 10),
        (ItemID.Lubricant, CityID.Refinery, CityID.TrainYard, 4),
        (ItemID.Petroleum, CityID.Refinery, CityID.TrainYard, 4),
        (ItemID.Fuel, CityID.Refinery, CityID.TrainYard, 20),
        (ItemID.Mythril, CityID.Mine, CityID.TrainYard, 1),
        (ItemID.Iron, CityID.Mine, CityID.TrainYard, 10),
        (ItemID.Fuel, CityID.Refinery, CityID.Coast, 10),
        (ItemID.Fuel, CityID.Refinery, CityID.Mine, 10),
        (ItemID.Fuel, CityID.Refinery, CityID.Reservoir, 10)
    };

    private static string loopScript(string productID, string citySrc, string cityDest, int proportion) {
        int b = 100; 
        int minAtDestToLeave = b; 
        int minAtSrcToLeave = b * proportion; 

        string condition = $"{citySrc}.{productID} < {minAtSrcToLeave} OR {cityDest}.{productID} > {minAtDestToLeave}";
        string directions = $"LOAD {citySrc}.{productID} / {proportion} {productID}; GO TO {cityDest};";
        directions += $"UNLOAD SELF.{productID} {productID}; GO TO {citySrc};";

        return "WHILE " + condition +  " { WAIT; } " + directions;
    }

    private static string loopExplanation(string productID, string citySrc, string cityDest) {
        return (@"
        Begins at the " + citySrc + @". Brings a portion of the " + productID + @" 
        to the " + cityDest + "if there is enough " + productID + @"
        at the " + citySrc + "and little enough at the " + cityDest)
        .Replace("\n", "");
    }

    private static Dictionary<string, string> initScriptExplanations() {
        Dictionary<string, string> scripts = new(); 
        foreach ((string itemID, string src, string dest, int _) in programData) {
            string title = $"{itemID} To {dest}"; 
            string explanation = loopExplanation(itemID, src, dest); 
            scripts[title] = explanation;
        }
        return scripts;
    }

    private static Dictionary<string, string> initScripts() {
        Dictionary<string, string> scripts = new(); 
        foreach ((string itemID, string src, string dest, int proportion) in programData) {
            string title = $"{itemID} To {dest}"; 
            string program = loopScript(itemID, src, dest, proportion);
            scripts[title] = program;
        }
        return scripts;
    }

    public static Dictionary<string, string> ScriptExplanations = initScriptExplanations(); 
    public static Dictionary<string, string> Scripts = initScripts(); 

    public static void BuyTrainProgram(string program, Train t, World w, string programName = "") {
        bool hasMotherboard = false; 
        Inventory inv;

        if (t.ComingFrom.Inv.ItemCount(ItemID.Motherboard) >= 1) {
            inv = t.ComingFrom.Inv; 
            hasMotherboard = true; 
        } else {
            if (t.Inv.ItemCount(ItemID.Motherboard) >= 1) {
                hasMotherboard = true; 
            }
            inv = t.Inv; 
        }

        if (hasMotherboard) {
            try {
                TAL.SetTrainProgram(program, t, w, programName: programName); 
                EntityFactory.AddToast(w, 150, 75, $"Successfully set {t.Id} program!");
                inv.Take(ItemID.Motherboard, 1); 
            } catch (InvalidOperationException e) {
                EntityFactory.AddToast(w, 150, 75, $"Failed to compile program for {t.Id}, {e}");
            }
        } else {
            EntityFactory.AddToast(w, 150, 75, $"You must place a Motherboard in {t.Id}'s inventory to program it!");
        }
    }

    public static TALBody SetTrainProgram(string program, Train t, World w, int nextInstruction = 0, string programName = "") {
        TALBody body = TALParser.ParseProgram(program, w, t, nextInstruction); 
        t.SetProgram(program, programName); 
        t.SetExecutable(body); 
        return body;
    }

    public static Dictionary<string, string> PlayerScripts = new(); 
}
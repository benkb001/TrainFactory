namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

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

public class TAL {

    public const string IronFactoryLoop = "Iron To Factory";
    public const string SandFactoryLoop = "Sand To Factory"; 
    public const string WoodFactoryLoop = "Wood To Factory"; 
    public const string WaterFactoryLoop = "Water To Factory"; 
    public const string OilToFactory = "Oil To Factory";
    public const string MythrilToFactory = "TODO"; 
    public const string CobaltToFactory = "TODO"; 
    public const string AdamantiteToFactory = "TODO";

    public const string LubricantToTrainYard = "Lubricant To Train Yard"; 
    public const string PetroleumToTrainYard = "Petroleum To Train Yard"; 
    public const string FuelToTrainYard = "Fuel To Train Yard";
    public const string MythrilToTrainYard = "Mythril To Train Yard";
    public const string IronToTrainYard = "Iron To Train Yard";

    public const string OilToRefinery = "Oil To Refinery";
    public const string WaterToRefinery = "Water To Refinery";
    public const string IronToRefinery = "Iron To Refinery";

    public const string FuelToCoast = "Fuel To Coast";
    public const string WaterToGreenhouse = "Water To Greenhouse";

    public const string FuelLoop = "Fuel Loop"; 

    private string loopScript(string productID, string citySrc, string cityDest, int proportion) {
        int b = 100; 
        int minAtDestToLeave = b; 
        int minAtSrcToLeave = b * proportion; 

        string condition = $"{citySrc}.{productID} > {minAtDestToLeave} OR {cityDest}.{productID} < {minAtSrcToLeave}";
        string directions = $"LOAD {citySrc}.{productID} / {proportion} {productID}; GO TO {cityDest};";
        directions += $"UNLOAD SELF.{productID} {productID}; GO TO {citySrc}";

        return "WHILE " + condition +  " { WAIT; } " + directions;
    }

    public static Dictionary<string, string> Scripts = new() {
        [IronFactoryLoop] = @"
            WHILE Factory.Iron > 100 OR Mine.Iron < 100 {
                WAIT;
            }

            LOAD Mine.Iron / 2 Iron; 
            GO TO Factory; 

            UNLOAD SELF.Iron Iron; 
            GO TO Mine;
        ",
        [WaterFactoryLoop] = @"
            WHILE Coast.Water < 1000 OR Factory.Water > 100 {
                WAIT;
            }

            LOAD Coast.Water / 10 Water;
            GO TO Factory; 

            UNLOAD SELF.Water Water;
            GO TO Coast;
        ",
        [SandFactoryLoop] = @"
            WHILE Coast.Sand < 100 OR Factory.Sand > 100 {
                WAIT;
            }
            
            LOAD Coast.Sand Sand; 
            GO TO Factory; 

            UNLOAD SELF.Sand Sand;
            GO TO Coast;
        ",
        [WoodFactoryLoop] = @"
            WHILE Greenhouse.Wood < 100 OR Factory.Wood > 100 {
                WAIT;
            }

            LOAD Greenhouse.Wood Wood;
            GO TO Factory; 

            UNLOAD SELF.Wood Wood;
            GO TO Greenhouse;
        ",
        [OilToRefinery] = @"
            WHILE Refinery.Oil > 100 OR Reservoir.Oil < 100 {
                WAIT;
            }
            LOAD Reservoir.Oil / 2 Oil; 
            GO TO Factory; 
            GO TO Greenhouse; 
            GO TO Refinery; 
            UNLOAD SELF.Oil Oil; 
            GO TO Greenhouse;
            GO TO Factory;
            GO TO Reservoir; 
        ",
        [WaterToRefinery] = @"
            WHILE Refinery.Water > 100 OR Coast.Water < 100 {
                WAIT;
            }
            LOAD Coast.Water / 2 Water; 
            GO TO Refinery; 
            UNLOAD Self.Water Water; 
            GO TO Coast;
        ",
        [IronToRefinery] = @"
            WHILE Refinery.Iron > 100 OR Mine.Iron < 1000 {
                WAIT;
            }
            LOAD Mine.Iron / 10 Iron; 
            GO TO Factory; 
            GO TO Greenhouse; 
            GO TO Refinery; 
            UNLOAD Self.Iron Iron; 
            GO TO Greenhouse; 
            GO TO Factory; 
            GO TO Mine;
        ",
        [WaterToGreenhouse] = @"
            WHILE Greenhouse.Water > 100 OR Coast.Water < 200 {
                WAIT;
            }
            LOAD Coast.Water / 2 Water; 
            GO TO Factory; 
            GO TO Greenhouse; 
            UNLOAD Self.Water Water; 
            GO TO Factory; 
            GO TO Coast;
        ",
        [OilToFactory] = @"
            WHILE Factory.Oil > 100 OR Reservoir.Oil < 1000 {
                WAIT;
            }

            LOAD Reservoir.Oil / 10 Oil;
            GO TO Factory; 
            
            UNLOAD SELF.Oil Oil; 
            GO TO Reservoir;
        ",
        [LubricantToTrainYard] = "TODO",
        [PetroleumToTrainYard] = "TODO",
        [FuelToTrainYard] = "TODO",
        [MythrilToTrainYard] = "TODO",
        [IronToTrainYard] = "TODO"

    };

    private static string loopExplanation(string productID, string citySrc, string cityDest) {
        return (@"
        Begins at the " + citySrc + @". Brings a portion of the " + productID + @" 
        to the " + cityDest + "if there is enough " + productID + @"
        at the " + citySrc + "and little enough at the " + cityDest)
        .Replace("\n", "");
    }

    public static Dictionary<string, string> ScriptExplanations = new() {
        [IronFactoryLoop] = loopExplanation(ItemID.Iron, CityID.Mine, CityID.Factory),
        [SandFactoryLoop] = loopExplanation(ItemID.Sand, CityID.Coast, CityID.Factory),
        [WaterFactoryLoop] = loopExplanation(ItemID.Water, CityID.Coast, CityID.Factory),
        [WoodFactoryLoop] = loopExplanation(ItemID.Wood, CityID.Greenhouse, CityID.Factory), 
        [OilToRefinery] = loopExplanation(ItemID.Oil, CityID.Reservoir, CityID.Refinery),
        [WaterToRefinery] = loopExplanation(ItemID.Water, CityID.Coast, CityID.Refinery),
        [IronToRefinery] = loopExplanation(ItemID.Iron, CityID.Mine, CityID.Refinery),

    };

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
            } catch (InvalidOperationException) {
                EntityFactory.AddToast(w, 150, 75, $"Failed to compile program for {t.Id}");
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
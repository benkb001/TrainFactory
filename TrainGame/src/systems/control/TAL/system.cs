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

    public const string IronFactoryLoop = "MineFactoryLoop";
    public const string SandFactoryLoop = "SandFactoryLoop"; 
    public const string WoodFactoryLoop = "WoodFactoryLoop"; 
    public const string WaterFactoryLoop = "WaterFactoryLoop"; 
    public const string FuelLoop = "Fuel Loop"; 

    public static Dictionary<string, string> Scripts = new() {
        [IronFactoryLoop] = @"
            LOAD Factory.Fuel / 2 Fuel; 
            GO TO Mine; 
            UNLOAD SELF.Fuel Fuel; 
            WHILE Mine.Fuel > 0 {
                WAIT;
            }
            LOAD Mine.Iron Iron; 
            GO TO Factory; 
            UNLOAD SELF.Iron Iron; 
        ",
        [WaterFactoryLoop] = @"
            LOAD Factory.Fuel / 2 Fuel; 
            GO TO Coast;
            UNLOAD SELF.Fuel Fuel;
            WHILE Coast.Fuel > 0 {
                WAIT;
            }
            LOAD Coast.Water Water; 
            GO TO Factory; 
            UNLOAD SELF.Water Water;
        ",
        [SandFactoryLoop] = @"
            LOAD Factory.Fuel / 2 Fuel; 
            GO TO Coast;
            UNLOAD SELF.Fuel Fuel;
            WHILE Coast.Fuel > 0 {
                WAIT;
            }
            LOAD Coast.Sand Sand; 
            GO TO Factory; 
            UNLOAD SELF.Sand Sand;
        ",
        [WoodFactoryLoop] = @"
            LOAD Factory.Water Water; 
            GO TO Greenhouse;
            UNLOAD SELF.Water Water;
            WHILE Greenhouse.Water > 0 {
                WAIT;
            }
            LOAD Greenhouse.Wood Wood; 
            GO TO Factory; 
            UNLOAD SELF.Wood Wood;
        ",
        [FuelLoop] = @"
            LOAD Factory.Fuel / 2 Fuel; 
            GO TO Coast; 
            UNLOAD SELF.Fuel Fuel; 
            WHILE Coast.Fuel > 0 {
                WAIT;
            }
            LOAD Coast.Water Water; 
            GO TO Factory; 
            GO TO Greenhouse; 
            UNLOAD SELF.Water Water; 
            WHILE Greenhouse.Water > 0 {
                WAIT;
            }
            LOAD Greenhouse.Wood Wood; 
            GO TO Factory; 
            UNLOAD SELF.Wood Wood; 
        ",

    };

    private static string loopExplanation(string productID, string cityID) {
        return @"
        Trains with this program begin at the factory. They will 
        load half of the factory's fuel, then go to the " + cityID + @" and 
        drop off all their fuel. They will wait until there is no 
        more fuel, and then take all the " + productID + @". 
        Then, they drop it all of at the factory. Then they continue 
        the loop. 
        ".Replace("\n", "");
    }

    public static Dictionary<string, string> ScriptExplanations = new() {
        [IronFactoryLoop] = loopExplanation(ItemID.Iron, CityID.Mine),
        [SandFactoryLoop] = loopExplanation(ItemID.Sand, CityID.Coast),
        [WaterFactoryLoop] = loopExplanation(ItemID.Water, CityID.Coast),
        [WoodFactoryLoop] = loopExplanation(ItemID.Wood, CityID.Greenhouse),
        [FuelLoop] = @"
            Begins at factory. Brings half the factory's fuel to the coast.
            Waits until there is no more fuel at the coast and brings all the 
            water at the coast to the greenhouse. Waits until 
            there is no more water at the greenhouse, and brings all the 
            wood to the factory. 
        ".Replace("\n", "")
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
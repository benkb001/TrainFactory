namespace TrainGame.Constants;

using System.Collections.Generic;
using System.Linq;

public static class ItemID {
    public const string Accelerator = "Accelerator";
    public const string Adamantite = "Adamntite"; 
    public const string AdamantiteDrill = "Adamantite Drill"; 
    public const string AirResistor = "Air Resistor";
    public const string AntiGravity = "Anti Gravity";
    public const string Armor1 = "Armor1"; 
    public const string Armor2 = "Armor2"; 
    public const string Armor3 = "Armor3"; 
    public const string Assembler = "Assembler"; 
    public const string Cobalt = "Cobalt"; 
    public const string CobaltDrill = "Cobalt Drill"; 
    public const string CombustionController = "Combustion Controller";
    public const string Credit = "Credit";
    public const string DepotUpgrade = "Depot Upgrade"; 
    public const string Duplicator = "Duplicator";
    public const string Drill = "Drill"; 
    public const string Engine = "Engine";
    public const string Excavator = "Excavator"; 
    public const string Fuel = "Fuel"; 
    public const string Gasifier = "Gasifier";
    public const string Greenhouse = "Greenhouse"; 
    public const string Glass = "Glass"; 
    public const string Gun = "Gun"; 
    public const string Gun2 = "Gun2"; 
    public const string Gun3 = "Gun3"; 
    public const string Iron = "Iron"; 
    public const string Kiln = "Kiln"; 
    public const string Lubricant = "Lubricant";
    public const string MachineUpgrade = "Machine Upgrade"; 
    public const string Motherboard = "Motherboard"; 
    public const string Mythril = "Mythril";
    public const string MythrilDrill = "Mythril Drill";
    public const string Oil = "Oil"; 
    public const string OilRig = "Oil Rig";
    public const string Petroleum = "Petroleum";
    public const string PocketDimension = "Pocket Dimension";
    public const string Pump = "Pump"; 
    public const string Rail = "Rail"; 
    public const string Refinery = "Refinery";
    public const string Sand = "Sand"; 
    public const string SmartAssembler = "Smart Assembler";
    public const string Water = "Water"; 
    public const string Wood = "Wood"; 
    public const string TimeCrystal = "Time Crystal"; 

    public static readonly List<string> All = [
        Accelerator, Adamantite, AdamantiteDrill, AirResistor, AntiGravity, Armor1, Armor2, 
        Armor3, Assembler, Cobalt, CobaltDrill, Credit, DepotUpgrade,
        Drill, Duplicator, Engine, Excavator, Fuel, Gasifier, Greenhouse,
        Glass, Gun, Gun2, Gun3, Iron, 
        Kiln, Lubricant, MachineUpgrade, Motherboard, Mythril, MythrilDrill, Oil, 
        OilRig, Petroleum, PocketDimension, Pump, Rail, Refinery, Sand, 
        SmartAssembler, TimeCrystal, Water, Wood
    ]; 

    public static readonly List<string> Liquids = [
        Oil, Lubricant, Petroleum, Water
    ]; 

    public static readonly List<string> Solids = All.Where(s => !Liquids.Contains(s)).ToList(); 
}
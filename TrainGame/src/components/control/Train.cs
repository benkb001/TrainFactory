namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Systems; 

public class Train : IInventorySource, IID, ITrain {

    private float milesPerHour; 
    private float power; 
    private float mass; 
    private WorldTime left; 
    private WorldTime arrivalTime; 
    private WorldTime lastMoved = new WorldTime();
    private bool isTraveling; 
    private bool isEmbarking; 
    private static HashSet<string> usedIDs = new(); 
    private string program; 
    private string programName = "None"; 
    private Vector2 origin;
    private Vector2 destination;
    private Vector2 position; 
    private Vector2 journey;
    private Vector2 moved => position - origin;
    private float milesOfFuel;
    private float massMilesPerFuel = Constants.MassMilesPerFuel;

    public WorldTime DepartureTime => left; 
    public WorldTime ArrivalTime => arrivalTime; 
    public readonly Inventory Inv; 
    public Dictionary<CartType, Inventory> Carts;
    public readonly string Id; 
    public string ID => Id;
    public bool HasPlayer = false; 
    public float MilesPerHour => milesPerHour; 
    public float Mass => mass; 
    public bool IsEmbarking => isEmbarking; 
    public string Program => program; 
    public string ProgramName => programName; 
    public float Power => power; 
    public Vector2 Position => position; 
    public Vector2 Destination => destination;
    public float MilesOfFuel => milesOfFuel;
    public float JourneyCompletion => (moved.Length()) / (journey.Length());

    public const string DefaultID = ""; 

    public Train(Inventory Inv, Vector2 position, Dictionary<CartType, Inventory> Carts, string Id, 
    float milesPerHour = 0f, float power = 0f, float mass = 1f, float milesOfFuel = 25f) {
        
        this.Id = Id; 

        this.Inv = Inv; 
        this.isTraveling = false; 
        this.position = position;
        this.origin = position;

        if (mass <= 0f) {
            throw new InvalidOperationException($"Mass {mass} invalid, must be > 0"); 
        }

        if (power == 0f && mass == 1f) {
            power = milesPerHour; 
        }

        this.power = power; 
        this.mass = mass;
        this.milesOfFuel = milesOfFuel; 
        setMPH(); 

        this.Carts = Carts; 

        this.left = new WorldTime(); 
        this.arrivalTime = new WorldTime(); 
    }

    public void Embark(Vector2 destination, WorldTime now) {
        this.isEmbarking = true; 
        
        this.left = now.Clone(); 
        this.lastMoved = now.Clone();
        this.isTraveling = true;
        this.destination = destination;

        journey = destination - position;
        float journeyMiles = journey.Length();
        float hours = journeyMiles / milesPerHour; 
        int fuelToTake = 2 * (int)Math.Ceiling(((journeyMiles - milesOfFuel) * mass) / massMilesPerFuel);
        
        int taken = Inv.Take(ItemID.Fuel, fuelToTake).Count;
        taken += Carts[CartType.Freight].Take(ItemID.Fuel, fuelToTake - taken).Count;
        milesOfFuel += (massMilesPerFuel * taken) / mass; 

        //TODO: this maybe should be recalculated each frame, 
        //because train might slow down if it runs into another
        //train
        this.arrivalTime = now + new WorldTime(hours: hours); 
    }
    
    public void Update() {
        if (!isTraveling || !IsArriving()) {
            return; 
        }
        
        this.isTraveling = false; 
        position = destination;
        origin = destination;

        //TODO: Test
        //ICKY
        HasPlayer = false;
    }

    //IsArriving MUST be called before Update or it will never be true
    public bool IsArriving() {
        return isTraveling && (moved.Length() >= journey.Length());
    }

    public bool IsTraveling() {
        return isTraveling; 
    }

    public void AddCart(CartType type) {
        mass += Constants.CartMass[type]; 
        Carts[type].Upgrade(); 
        setMPH(); 
    }

    public void UpgradeInventoryExponential() {
        foreach (CartType type in Cart.AllTypes) {
            int levelsAdded = Carts[type].UpgradeExponential();
            mass += Constants.CartMass[type] * levelsAdded;
        }
        setMPH();
    }

    public void Move(WorldTime now, Train inFront = null) {
        float hours = (now - lastMoved).InHours(); 
        float moved = milesPerHour * hours; 

        if (milesOfFuel <= 0f) {
            moved = Math.Min(moved, Math.Max(Constants.MinSpeed, (float)moved / 10f));
        } else {
            milesOfFuel -= moved;
        }

        Vector2 newPosition = (Vector2.Normalize(journey) * moved) + position;

        if (float.IsNaN(newPosition.X)) {
            newPosition = Vector2.Zero;
        }

        if (inFront != null) {
            float inFrontLen = (journey - inFront.Position).Length();
            float curLen = (journey - newPosition).Length();

            if (inFrontLen > curLen) {
                newPosition = inFront.Position;
            }
        }

        position = newPosition; 

        if (this.moved.Length() >= this.journey.Length()) {
            position = destination;
        }
        
        lastMoved = now.Clone();
    }
    
    //TODO; TEST
    public void UpgradeInventory() {
        Inv.Upgrade(); 
        mass += Constants.InvUpgradeMass; 
        setMPH(); 
    }

    public void UpgradePower(float p = 0f) {
        power += p; 
        setMPH(); 
    }

    public void UpgradePowerExponential() {
        power = power * Constants.ExponentialTrainPowerUpgradeFactor; 
        setMPH();
    }

    public void UpgradeMassMilesPerFuel(float m) {
        float prev = massMilesPerFuel;
        massMilesPerFuel += m;
        milesOfFuel *= (massMilesPerFuel / prev);
    }

    public void UpgradeMassMilesPerFuelExponential() {
        float prev = massMilesPerFuel;
        massMilesPerFuel *= Constants.ExponentialMilesPerFuelUpgradeFactor; 
        milesOfFuel *= (massMilesPerFuel / prev);
    }

    public List<Inventory> GetInventories() {
        List<Inventory> invs = new(); 
        invs.Add(Inv); 
        foreach (KeyValuePair<CartType, Inventory> kvp in Carts) {
            if (kvp.Value.Level > 0) {
                invs.Add(kvp.Value); 
            }
        }
        return invs; 
    }

    public void EndEmbarking() {
        isEmbarking = false; 
    }

    public string GetID() {
        return Id; 
    }

    public void SetPosition(float x, float y) {
        this.position = new Vector2(x, y);
    }

    public void SetProgram(string program, string programName = "") {
        this.program = program; 
        this.programName = programName; 
    }

    public string GetSummary() {
        string summary = $"{Id}\n"; 
        summary += $"MPH: {MilesPerHour}\n"; 
        summary += $"Program: {ProgramName}\n"; 
        summary += $"Miles Per Fuel: {massMilesPerFuel / mass}\n";
        summary += $"Miles Left: {MilesOfFuel}\n";
        return summary;
    }

    public int ItemCount(string itemID) {
        return Inv.ItemCount(itemID) + Carts.Values.Aggregate(0, (acc, cur) => acc + cur.ItemCount(itemID));
    }

    public static string GetCartID(CartType type, string trainID) {
        return $"{trainID} {type} Cart";
    }

    private void setMPH() {
        milesPerHour = power / mass; 
    }
}
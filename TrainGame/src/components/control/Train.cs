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

public class Train : IInventorySource, IID {

    private City comingFrom; 
    private City goingTo; 
    private float milesPerHour; 
    private float power; 
    private float mass; 
    private WorldTime left; 
    private WorldTime arrivalTime; 
    private bool isTraveling; 
    private bool isEmbarking; 
    private static HashSet<string> usedIDs = new(); 
    private string program; 

    public WorldTime DepartureTime => left; 
    public WorldTime ArrivalTime => arrivalTime; 
    public City ComingFrom => comingFrom; 
    public City GoingTo => goingTo; 
    public readonly Inventory Inv; 
    public Dictionary<CartType, Inventory> Carts;
    public readonly string Id; 
    public bool HasPlayer = false; 
    public float MilesPerHour => milesPerHour; 
    public float Mass => mass; 
    public bool IsEmbarking => isEmbarking; 
    public string Program => program; 
    public float Power => power; 

    public const string DefaultID = ""; 

    public Train(Inventory Inv, City origin, string Id = DefaultID, float milesPerHour = 0f, float power = 0f, float mass = 1f,
        Dictionary<CartType, Inventory> Carts = null) {
        
        if (ID.Used(Id) || Id.Equals("")) {
            Id = ID.GetNext(Constants.TrainStr); 
        }
        
        this.Id = Id; 
        usedIDs.Add(Id); 

        this.Inv = Inv; 
        this.comingFrom = origin;
        this.goingTo = origin; 
        this.isTraveling = false; 

        if (mass <= 0f) {
            throw new InvalidOperationException($"Mass {mass} invalid, must be > 0"); 
        }

        if (power == 0f && mass == 1f) {
            power = milesPerHour; 
        }

        this.power = power; 
        this.mass = mass;
        setMPH(); 

        if (Carts == null) {
            this.Carts = new Dictionary<CartType, Inventory>(); 
            this.Carts[CartType.Freight] = new Inventory($"{Id} Freight Cart", Constants.CartRows, Constants.CartCols, 0);
            this.Carts[CartType.Liquid] = new Inventory($"{Id} Liquid Cart", Constants.CartRows, Constants.CartCols, 0);
        } else {
            this.Carts = Carts; 
        }

        origin.AddTrain(this);

        this.left = new WorldTime(); 
        this.arrivalTime = new WorldTime(); 
    }

    public void Embark(City destination, WorldTime now) {
        this.isEmbarking = true; 

        if (this.comingFrom == destination) {
            throw new InvalidOperationException($"Train {Id} attempted to embark to the city it is at: {comingFrom.CityId}"); 
        }

        if (HasPlayer) {
            this.comingFrom.HasPlayer = false; 
        }
        
        this.goingTo = destination; 
        this.left = now.Clone(); 
        this.isTraveling = true;
        this.comingFrom.RemoveTrain(this); 

        Vector2 journey = comingFrom.RealPosition - goingTo.RealPosition; 
        float hours = journey.Length() / milesPerHour; 
        this.arrivalTime = now + new WorldTime(hours: hours); 
    }
    
    public Vector2 GetMapPosition(WorldTime cur) {
        if (!isTraveling) {
            return comingFrom.MapPosition; 
        }

        Vector2 journey = goingTo.RealPosition - comingFrom.RealPosition; 

        float hours = (cur - left).InHours(); 
        float moved = milesPerHour * hours; 
        float proportion_moved = moved / journey.Length(); 
    
        Vector2 map_journey = goingTo.MapPosition - comingFrom.MapPosition; 

        float map_moved = map_journey.Length() * proportion_moved; 
        return (Vector2.Normalize(map_journey) * map_moved) + comingFrom.MapPosition; 
    }
    
    public void Update(WorldTime cur) {
        if (!isTraveling || !IsArriving(cur)) {
            return; 
        }
        
        this.comingFrom = this.goingTo; 
        this.isTraveling = false; 
        this.goingTo.AddTrain(this);
        //TODO: Test
        if (HasPlayer) {
            this.goingTo.HasPlayer = true; 
            HasPlayer = false; 
        }
    }

    //IsArriving MUST be called before Update or it will never be true
    public bool IsArriving(WorldTime cur) {
        return isTraveling && cur.IsAfterOrAt(arrivalTime); 
    }

    public bool IsTraveling() {
        return isTraveling; 
    }

    //TODO: TEST 
    public void AddCart(Cart cart) {
        mass += Constants.CartMass[cart.Type]; 
        Carts[cart.Type].Upgrade();
        setMPH(); 
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

    public void SetProgram(string program) {
        this.program = program; 
    }

    private void setMPH() {
        milesPerHour = power / mass; 
    }
}
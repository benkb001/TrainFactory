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

public class Train : IInventorySource, IID {

    private City comingFrom; 
    private City goingTo; 
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
    private TALBody executable; 
    private Vector2 position; 
    private Vector2 journey;
    private Vector2 mapJourney;
    private Vector2 moved => position - comingFrom.Position;

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
    public string ProgramName => programName; 
    public float Power => power; 
    public TALBody Executable => executable; 
    public Vector2 Position => position; 

    public const string DefaultID = ""; 

    public Train(Inventory Inv, City origin, string Id = DefaultID, float milesPerHour = 0f, float power = 0f, float mass = 1f,
        Dictionary<CartType, Inventory> Carts = null) {
        
        if (Id.Equals("")) {
            Id = ID.GetNext(Constants.TrainStr); 
        } else {
            ID.Use(Id); 
        }
        
        this.Id = Id; 

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
            foreach (CartType type in Cart.AllTypes) {
                Inventory curInv = new Inventory(GetCartID(type, Id), 
                    Constants.CartRows, Constants.CartCols, 0, type);
                this.Carts[type] = curInv; 
            }
        } else {
            this.Carts = Carts; 
        }

        origin.AddTrain(this);

        this.left = new WorldTime(); 
        this.arrivalTime = new WorldTime(); 

        position = origin.Position;
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
        this.lastMoved = now.Clone();
        this.isTraveling = true;
        this.comingFrom.RemoveTrain(this); 

        journey = goingTo.RealPosition - comingFrom.RealPosition; 
        mapJourney = goingTo.MapPosition - comingFrom.MapPosition; 
        float hours = journey.Length() / milesPerHour; 
        //TODO: this maybe should be recalculated each frame, 
        //because train might slow down if it runs into another
        //train
        this.arrivalTime = now + new WorldTime(hours: hours); 
        destination.SendTrain(this);
    }
    
    public Vector2 GetMapPosition() {
        if (!isTraveling) {
            return comingFrom.MapPosition; 
        }

        float proportion_moved = moved.Length() / journey.Length();
        float map_moved = mapJourney.Length() * proportion_moved; 
        return (Vector2.Normalize(mapJourney) * map_moved) + comingFrom.MapPosition; 
    }
    
    public void Update() {
        if (!isTraveling || !IsArriving()) {
            return; 
        }
        
        this.goingTo.ReceiveTrain(this);
        this.comingFrom = this.goingTo; 
        this.isTraveling = false; 
        position = goingTo.Position;

        //TODO: Test
        if (HasPlayer) {
            this.goingTo.HasPlayer = true; 
            HasPlayer = false; 
        }
    }

    //IsArriving MUST be called before Update or it will never be true
    public bool IsArriving() {
        bool arriving = isTraveling && moved.Length() >= journey.Length();
        return arriving; 
    }

    public bool IsTraveling() {
        return isTraveling; 
    }

    //TODO: REMOVE, keep the one that just takes type
    public void AddCart(Cart cart) {
        mass += Constants.CartMass[cart.Type]; 
        Carts[cart.Type].Upgrade();
        setMPH(); 
    }

    public void AddCart(CartType type) {
        mass += Constants.CartMass[type]; 
        Carts[type].Upgrade(); 
        setMPH(); 
    }

    public void Move(WorldTime now, Train inFront = null) {
        float hours = (now - lastMoved).InHours(); 
        float moved = milesPerHour * hours; 

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

    public void SetProgram(string program, string programName = "") {
        this.program = program; 
        this.programName = programName; 
    }

    public void SetExecutable(TALBody executable) {
        this.executable = executable; 
    }

    public static string GetCartID(CartType type, string trainID) {
        return $"{trainID} {type} Cart";
    }

    private void setMPH() {
        milesPerHour = power / mass; 
    }
}
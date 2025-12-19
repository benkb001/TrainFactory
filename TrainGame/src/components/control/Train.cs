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

public class Train {

    private City comingFrom; 
    private City goingTo; 
    private float milesPerHour; 
    private float power; 
    private float mass; 
    private WorldTime left; 
    private bool isTraveling; 
    private static HashSet<string> usedIDs = new(); 

    public City ComingFrom => comingFrom; 
    public City GoingTo => goingTo; 
    public readonly Inventory Inv; 
    public readonly List<Cart> Carts; 
    public readonly string Id; 
    public bool HasPlayer = false; 
    public float MilesPerHour => milesPerHour; 
    public float Mass => mass; 

    public Train(Inventory Inv, City origin, string Id = "", float milesPerHour = 0f, float power = 0f, float mass = 1f) {
        if (ID.Used(Id)) {
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

        Carts = new(); 

        origin.AddTrain(this);
    }

    public void Embark(City destination, WorldTime at) {
        if (this.comingFrom == destination) {
            throw new InvalidOperationException($"Train {Id} attempted to embark to the city it is at: {comingFrom.CityId}"); 
        }

        if (HasPlayer) {
            this.comingFrom.HasPlayer = false; 
        }
        
        this.goingTo = destination; 
        this.left = at.Clone(); 
        this.isTraveling = true;
        this.comingFrom.RemoveTrain(this); 
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

    public bool IsArriving(WorldTime cur) {
        if (!isTraveling) {
            return false; 
        }
        
        Vector2 journey = comingFrom.RealPosition - goingTo.RealPosition; 
        float hours = (cur - left).InHours(); 
        float moved = milesPerHour * hours; 
        return moved >= journey.Length(); 
    }

    public bool IsTraveling() {
        return isTraveling; 
    }

    //TODO: TEST 
    public void AddCart(Cart cart) {
        Carts.Add(cart); 
        mass += cart.Mass; 
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

    private void setMPH() {
        milesPerHour = power / mass; 
    }
}
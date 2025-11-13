namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Utils; 
public class Train {
    private City comingFrom; 
    private City goingTo; 
    public City ComingFrom => comingFrom; 
    public City GoingTo => goingTo; 
    public readonly Inventory Inv; 
    public readonly string Id; 
    private float milesPerHour; 
    private WorldTime left; 
    private bool isTraveling; 

    public Train(Inventory Inv, City origin, string Id = "", float milesPerHour = 0f) {
        this.Id = Id; 
        this.milesPerHour = milesPerHour; 
        this.Inv = Inv; 
        this.comingFrom = origin;
        this.goingTo = origin; 
        this.isTraveling = false; 
    }

    public void Embark(City destination, WorldTime at) {
        if (this.comingFrom == destination) {
            throw new InvalidOperationException($"Train {Id} attempted to embark to the city it is at: {comingFrom.CityId}"); 
        }
        this.goingTo = destination; 
        this.left = at.Clone(); 
        this.isTraveling = true; 
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
}
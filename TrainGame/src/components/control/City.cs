namespace TrainGame.Components;

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.ECS; 

public class City : IID {
    private string cityId; 
    private float uiX; 
    private float uiY;

    private Vector2 mapPosition; 
    private Vector2 realPosition; 

    public Vector2 MapPosition => mapPosition; 
    public Vector2 RealPosition => realPosition; 

    public float UiX => uiX; 
    public float UiY => uiY;

    public string CityId => cityId; 
    public string Id => cityId; 

    public readonly Inventory Inv;
    public readonly List<City> AdjacentCities = []; 

    public static float UIWidth = 50f; 
    public static float UIHeight = 50f;
    public bool HasPlayer = false; 
    
    private Dictionary<string, Train> trains = new();
    public Dictionary<string, Train> Trains => trains; 

    private Dictionary<string, Machine> machines = new(); 
    public Dictionary<string, Machine> Machines => machines; 

    private List<Cart> carts = new(); 
    public List<Cart> Carts => carts; 
    
    public static string GetInvID(string cityID) {
        return $"{cityID} Depot";
    }

    public City(string cityId, Inventory Inv, float uiX = 0f, float uiY = 0f, float realX = 0f, float realY = 0f) {
        this.cityId = cityId; 
        this.Inv = Inv; 
        this.uiX = uiX; 
        this.uiY = uiY;

        this.mapPosition = new Vector2(uiX, uiY); 
        this.realPosition = new Vector2(realX, realY); 
    }

    public void AddConnection(City c) {
        if (!AdjacentCities.Contains(c)) {
            AdjacentCities.Add(c); 
        }

        if (!c.AdjacentCities.Contains(this)) {
            c.AdjacentCities.Add(this); 
        }
    }

    //todo: test
    public void AddMachine(Machine m) {
        machines[m.Id] = m; 
        m.SetCity(this); 
    }

    //todo: test
    public void AddTrain(Train t) {
        trains[t.Id] = t; 
    }

    //todo: test
    public void AddCart(Cart c) {
        carts.Add(c); 
    }

    //todo: test
    public void RemoveCart(Cart c) {
        carts.Remove(c); 
    }

    //todo: test
    public void RemoveTrain(Train t) {
        trains.Remove(t.Id); 
    }

    public string GetID() {
        return cityId; 
    }

    public void AddConnections(List<City> cities) {
        foreach (City c in cities) {
            AdjacentCities.Add(c); 
        }
    }
}
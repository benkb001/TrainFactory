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

    private Dictionary<CartType, int> carts = new(); 

    private Dictionary<City, List<Train>> trainsEnRoute = new();
    public Dictionary<City, List<Train>> TrainsEnRoute => trainsEnRoute; 

    public Vector2 Position => realPosition;
    
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

        foreach (CartType type in Cart.AllTypes) {
            carts[type] = 0; 
        }
    }

    public void AddConnection(City c) {
        if (!AdjacentCities.Contains(c)) {
            AdjacentCities.Add(c); 
            trainsEnRoute[c] = new();
        }

        if (!c.AdjacentCities.Contains(this)) {
            c.AdjacentCities.Add(this); 
            c.trainsEnRoute[this] = new();
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

    //todo: REMOVE, keep the one with type
    public void AddCart(Cart c) {
        carts[c.Type] += 1; 
    }

    public void AddCart(CartType type) {
        carts[type] += 1; 
    }

    //todo: REMOVE
    public void RemoveCart(Cart c) {
        carts[c.Type] -= 1; 
    }

    public void RemoveCart(CartType type) {
        carts[type] -= 1; 
    }

    public void ReceiveTrain(Train t) {
        trainsEnRoute[t.ComingFrom].Remove(t);
        AddTrain(t);
    }

    //todo: test
    public void RemoveTrain(Train t) {
        trains.Remove(t.Id); 
    }

    public void SendTrain(Train t) {
        trainsEnRoute[t.ComingFrom].Add(t);
    }

    public string GetID() {
        return cityId; 
    }

    public void AddConnections(List<City> cities) {
        foreach (City c in cities) {
            AddConnection(c);
        }
    }

    public int NumCarts(CartType type) {
        return carts[type]; 
    }
}
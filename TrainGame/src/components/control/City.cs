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

public class City {
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

    public readonly Inventory Inv;
    public readonly List<City> AdjacentCities = []; 

    public static float UIWidth = 100f; 
    public static float UIHeight = 100f;

    public City(string cityId, Inventory Inv, float uiX = 0f, float uiY = 0f, float realX = 0f, float realY = 0f) {
        this.cityId = cityId; 
        this.Inv = Inv; 
        this.uiX = uiX; 
        this.uiY = uiY;

        this.mapPosition = new Vector2(uiX, uiY); 
        this.realPosition = new Vector2(realX, realY); 
    }

    public void AddConnection(City c) {
        AdjacentCities.Add(c); 
    }

    public void AddConnections(List<City> cities) {
        foreach (City c in cities) {
            AdjacentCities.Add(c); 
        }
    }
}
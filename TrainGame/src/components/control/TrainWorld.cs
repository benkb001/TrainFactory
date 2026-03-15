namespace TrainGame.Components;

using System.Collections.Generic;
using System.Linq;
using TrainGame.ECS;
using TrainGame.Utils;

public class TrainWorld : ITrainWorld<Train, City> {
    private World w; 

    public TrainWorld(World w) {
        this.w = w; 
    }

    public Train GetTrain(string id) {
        return ID.GetComponent<Train>(id, w);
    }

    public City GetCity(string id) {
        return ID.GetComponent<City>(id, w);
    }

    public TrainState Embark(Train train, City city) {

        if (train.ComingFrom == city) {
            return TrainState.AtCity;
        }

        List<City> all = w
        .GetMatchingEntities([typeof(City), typeof(Data)])
        .Select(e => w.GetComponent<City>(e))
        .ToList();

        List<City> path = Util.ShortestPathUnweighted(all, train.ComingFrom, city); 

        if (path != null && path.Count > 0) {
            City next = path[0]; 
            TrainWrap.Embark(train, next, w); 
            if (next == city) {
                return TrainState.OnLastPath;
            } else {
                return TrainState.OnMidPath;
            }
        } else {
            return TrainState.NoPath;
        }
    }

    public void Load(Train train, string itemID, int count) {
        City city = train.ComingFrom; 
        city.Inv.TransferTo(train.GetInventories(), itemID, count);
    }

    public void Unload(Train train, string itemID, int count) {
        City city = train.ComingFrom; 
        city.Inv.TransferFrom(train.GetInventories(), itemID, count); 
    }
}
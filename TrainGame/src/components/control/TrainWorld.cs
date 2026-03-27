namespace TrainGame.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using TrainGame.ECS;
using TrainGame.Utils;

public class ComingFromCity {
    private City city; 
    public ComingFromCity(City city) {
        this.city = city; 
    }

    public static implicit operator City(ComingFromCity c) {
        return c.city;
    }
}

public class GoingToCity {
    private City city; 
    public GoingToCity(City city) {
        this.city = city; 
    }

    public static implicit operator City(GoingToCity c) {
        return c.city;
    }
}

public class TrainWorld : ITrainWorld<Train, City> {
    private World w; 

    public TrainWorld(World w) {
        this.w = w; 
    }

    public Train GetTrain(string id) {
        return ComponentID.GetComponent<Train>(id, w);
    }

    public City GetCity(string id) {
        return ComponentID.GetComponent<City>(id, w);
    }

    public TrainState Embark(Train train, City dest) {

        int trainEnt = ComponentID.GetEntity<Train>(train.ID, w);
        (City comingFrom, bool hasComingFrom) = TrainWrap.GetComingFrom(w, trainEnt); 

        if (!hasComingFrom) {
            throw new InvalidOperationException(
                $"Train {train.ID} cannot continue program because it has no ComingFromCity");
        }

        if (comingFrom == dest) {
            return TrainState.AtCity;
        }

        List<City> all = w
        .GetMatchingEntities([typeof(City), typeof(Data)])
        .Select(e => w.GetComponent<City>(e))
        .ToList();

        List<City> path = Util.ShortestPathUnweighted(all, comingFrom, dest); 

        if (path != null && path.Count > 0) {
            City next = path[0]; 
            TrainWrap.Embark(train, trainEnt, next, w); 
            if (next == dest) {
                return TrainState.OnLastPath;
            } else {
                return TrainState.OnMidPath;
            }
        } else {
            return TrainState.NoPath;
        }
    }

    public void Load(Train train, string itemID, int count) {
        City at = TrainWrap.GetComingFrom(w, train);
        at.Inv.TransferTo(train.GetInventories(), itemID, count);
    }

    public void Unload(Train train, string itemID, int count) {
        City at = TrainWrap.GetComingFrom(w, train);
        at.Inv.TransferFrom(train.GetInventories(), itemID, count); 
    }
}
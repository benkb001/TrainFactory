namespace TrainGame.Components;

public interface ITrain {
    int ItemCount(string itemID);
    bool IsTraveling();
}

public interface ICity {
    int ItemCount(string itemID);
}

public enum TrainState {
    AtCity, 
    NoPath,
    OnLastPath,
    OnMidPath
}

public interface ITrainWorld<T, C> 
where T : ITrain 
where C : ICity {
    T GetTrain(string trainID);
    C GetCity(string cityID);
    TrainState Embark(T train, C city);
    void Load(T train, string itemID, int count);
    void Unload(T train, string itemID, int count);
}

public interface ITALBody<T, C> where T : ITrain where C : ICity {
    void Execute(ITrainWorld<T, C> w);
    bool Paused();
    int NextInstruction();
    void Unpause();
    void Pause();
}
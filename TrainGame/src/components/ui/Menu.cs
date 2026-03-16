namespace TrainGame.Components;

using System;

public class Menu {
    private static Menu inst; 
    public static Menu Get() {
        if (inst is null) {
            inst = new Menu(); 
        }
        return inst; 
    }

    private Train train; 
    private City city; 
    private Machine machine;

    public Train GetTrain() => train; 
    public City GetCity() => city; 
    public Machine GetMachine() => machine;
    public readonly int TrainEntity; 

    public Menu(Train train = null, City city = null, Machine machine = null, int TrainEntity = -1) {
        this.train = train; 
        this.city = city;
        this.machine = machine; 
        this.TrainEntity = TrainEntity;
    } 
}
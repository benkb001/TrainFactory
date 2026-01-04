namespace TrainGame.Components; 
using TrainGame.ECS;

public static class TrainWrap {
    public static Train GetTestTrain() {
        Inventory inv = new Inventory("Test", 1, 1); 
        City c = new City("Test", inv); 
        return new Train(inv, c); 
    }
    public static int Add(World w, Train t) {
        int e = EntityFactory.Add(w, setData: true); 
        w.SetComponent<Train>(e, t); 
        return e; 
    }

    public static void Embark(Train t, City dest, World w) {
        t.Embark(dest, w.Time); 
    }
}
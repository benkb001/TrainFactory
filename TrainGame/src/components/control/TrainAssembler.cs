
namespace TrainGame.Components; 

using TrainGame.Constants; 
using TrainGame.Utils; 

public class TrainAssembler : IAssembler {
    private City c; 
    private Machine m;

    public TrainAssembler(City c, Machine m) {
        this.c = c; 
        this.m = m; 
    }

    public Machine GetMachine() {
        return m; 
    }

    public City GetCity() {
        return c; 
    }

    public void Assemble() {
        string id = ID.GetNext(Constants.TrainStr); 
        Inventory inv = new Inventory($"{id}_inv", Constants.TrainRows, Constants.TrainCols); 
        new Train(inv, c, Id: id, 
            power: Constants.TrainDefaultPower, mass: Constants.TrainDefaultMass);
    }       
}
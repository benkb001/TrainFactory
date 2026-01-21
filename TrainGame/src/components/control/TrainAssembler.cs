
namespace TrainGame.Components; 

using System; 
using TrainGame.Constants; 
using TrainGame.Utils; 

public class TrainAssembler : IAssembler<Train> {
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

    public Train Assemble() {
        Inventory inv = new Inventory(ID.GetNext("Loc"), Constants.TrainRows, Constants.TrainCols); 
        inv.SetSolid(); 
        return new Train(inv, c, 
            power: Constants.TrainDefaultPower, mass: Constants.TrainDefaultMass);
    }       
}
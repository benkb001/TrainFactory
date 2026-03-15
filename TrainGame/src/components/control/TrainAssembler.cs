
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
        return TrainWrap.Assemble(c);
    }       
}
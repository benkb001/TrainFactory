namespace TrainGame.Components; 

using TrainGame.Systems;
using TrainGame.Utils;

public class MachineInterfaceData : IInterfaceData {

    private Machine machine;
    private City city;

    public Machine GetMachine() => machine;
    public City GetCity() => city;
    
    public MachineInterfaceData(Machine machine, City city) {
        this.machine = machine; 
        this.city = city;
    }
    
    public SceneType GetSceneType() => SceneType.MachineInterface;
    public Menu GetMenu() => new Menu(machine: machine, city: city);
}
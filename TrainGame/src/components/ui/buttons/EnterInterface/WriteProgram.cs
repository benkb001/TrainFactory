namespace TrainGame.Components; 

using TrainGame.Systems; 

public class WriteProgramInterfaceData : IInterfaceData {
    private string programName; 
    private string program; 
    private Train train; 
    public Train GetTrain() => train; 
    public string ProgramName => programName; 
    public string Program => program; 
    
    public WriteProgramInterfaceData(Train train, string program, string programName) {
        this.train = train; 
        this.program = program; 
        this.programName = programName; 
    }

    public Menu GetMenu() {
        return new Menu(train: train); 
    }

    public SceneType GetSceneType() {
        return SceneType.WriteProgramInterface; 
    }
}

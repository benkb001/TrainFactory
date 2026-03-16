namespace TrainGame.Components; 

using TrainGame.Systems; 

public class WriteProgramInterfaceData : IInterfaceData {
    private string programName; 
    private string program; 
    private Train train; 
    private int trainEntity;
    public Train GetTrain() => train; 
    public string ProgramName => programName; 
    public string Program => program; 
    public int TrainEntity => trainEntity;
    
    public WriteProgramInterfaceData(Train train, int trainEntity, string program, string programName) {
        this.train = train; 
        this.program = program; 
        this.programName = programName; 
        this.trainEntity = trainEntity;
    }

    public Menu GetMenu() {
        return new Menu(train: train); 
    }

    public SceneType GetSceneType() {
        return SceneType.WriteProgramInterface; 
    }
}

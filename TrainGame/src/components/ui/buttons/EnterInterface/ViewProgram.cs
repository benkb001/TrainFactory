namespace TrainGame.Components; 

using TrainGame.Systems; 

public class ViewProgramInterfaceData : IInterfaceData {
    private string programName; 
    private string program; 
    private string programExplanation; 
    private Train train; 
    public Train GetTrain() => train; 
    public string ProgramName => programName; 
    public string Program => program; 
    public string ProgramExplanation => programExplanation; 
    
    public ViewProgramInterfaceData(string programName, string program, string programExplanation, Train train) {
        this.train = train; 
        this.program = program; 
        this.programName = programName; 
        this.programExplanation = programExplanation; 
    }

    public Menu GetMenu() {
        return new Menu(train: train); 
    }

    public SceneType GetSceneType() {
        return SceneType.ViewProgramInterface; 
    }
}

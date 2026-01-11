namespace TrainGame.Components; 

public class SetTrainProgramButton {
    private string scriptName; 
    private string program; 
    private Train train; 

    public string ScriptName => scriptName;
    public string Program => program;  
    public Train GetTrain() => train; 

    public SetTrainProgramButton(string scriptName, Train train, string program) {
        this.scriptName = scriptName; 
        this.train = train; 
        this.program = program; 
    }
}
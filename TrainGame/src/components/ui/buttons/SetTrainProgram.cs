namespace TrainGame.Components; 

public class SetTrainProgramButton {
    private string scriptName; 
    private string program; 
    private Train train; 
    private int trainEntity;

    public string ScriptName => scriptName;
    public string ProgramName => scriptName;
    public string Program => program;  
    public int TrainEntity => trainEntity;
    public Train GetTrain() => train; 

    public SetTrainProgramButton(string scriptName, Train train, int trainEntity, string program) {
        this.scriptName = scriptName; 
        this.train = train; 
        this.program = program; 
        this.trainEntity = trainEntity;
    }

    public void SetProgram(string programName, string program) {
        this.scriptName = programName; 
        this.program = program; 
    }
}
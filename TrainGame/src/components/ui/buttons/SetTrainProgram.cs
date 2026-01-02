namespace TrainGame.Components; 

public class SetTrainProgramButton {
    private string scriptName; 
    private Train train; 

    public string ScriptName => scriptName; 
    public Train GetTrain() => train; 

    public SetTrainProgramButton(string scriptName, Train train) {
        this.scriptName = scriptName; 
        this.train = train; 
    }
}
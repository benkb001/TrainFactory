namespace TrainGame.Components;

class TrainEmbarkedMessage {
    private Train train; 
    public Train GetTrain() => train; 

    public TrainEmbarkedMessage(Train train) {
        this.train = train; 
    }
}
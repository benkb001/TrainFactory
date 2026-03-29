namespace TrainGame.Components;

using TrainGame.Utils;

public class ParryHPBar {
    private Parrier parrier; 
    public Parrier GetParrier() => parrier;

    public ParryHPBar(Parrier p) {
        parrier = p; 
    }
}
namespace TrainGame.Components;

public class Vampired {
    public readonly int VampiredByEntity; 
    public readonly int Damage;

    public Vampired(int e, int damage) {
        this.VampiredByEntity = e; 
        this.Damage = damage;
    }
}
namespace TrainGame.Components;

using TrainGame.Utils;

public class Parrier {
    public bool Parrying;
    public readonly int MaxHP;
    public int HP;

    public Parrier(int MaxHP, int HP = -1) {
        this.MaxHP = MaxHP; 
        this.HP = HP == -1 ? MaxHP : HP; 
    }

    public void Reset() {
        this.HP = this.MaxHP; 
    }
}
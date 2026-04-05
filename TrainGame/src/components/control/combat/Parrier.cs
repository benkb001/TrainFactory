namespace TrainGame.Components;

using TrainGame.Utils;
using TrainGame.Constants;

public class Parrier {
    public bool Parrying;
    private Health health;
    public Health GetHealth() => health;
    public int MaxHP => health.MaxHP;
    public int HP => health.HP;

    public Parrier(int MaxHP = Constants.PlayerParrierHP, int HP = Constants.PlayerParrierHP) {
        this.health = new Health(MaxHP); 
        health.SetHP(HP);
    }

    public void Reset() {
        health.ResetHP();
    }
}
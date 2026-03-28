namespace TrainGame.Components;

public class Vampiric : IBulletTrait {
    public readonly int Damage;
    public Vampiric(int damage) {
        this.Damage = damage;
    }
}
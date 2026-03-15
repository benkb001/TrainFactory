namespace TrainGame.Components;

public class HealthPotion {
    private int hp; 
    public int HP => hp; 

    public HealthPotion(int hp) {
        this.hp = hp; 
    }
}

public class DamagePotion {
    private int dmg; 
    public int DMG => dmg; 

    public DamagePotion(int dmg) {
        this.dmg = dmg; 
    }
}
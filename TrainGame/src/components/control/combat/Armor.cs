namespace TrainGame.Components; 

using TrainGame.Constants; 

public class Armor {
    private int defense; 
    public int Defense => defense; 

    public Armor(int defense = 0) {
        this.defense = defense; 
    }
}
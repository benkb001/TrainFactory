namespace TrainGame.Components; 

using TrainGame.Constants; 

public class Armor : IEquippable {
    private int defense; 
    private int tempDefense; 
    public int Defense => defense + tempDefense; 

    public Armor(int defense = 0) {
        this.defense = defense; 
        tempDefense = 0; 
    }

    public void AddTempDefense(int tDefense) {
        tempDefense += tDefense;
    }

    public void ResetTempDefense() {
        tempDefense = 0;
    }
}
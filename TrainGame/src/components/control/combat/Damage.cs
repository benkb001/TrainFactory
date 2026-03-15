namespace TrainGame.Components;

class Damage {
    private int dmg; 
    private int tempDMG; 
    public int DMG => dmg + tempDMG; 

    public Damage(int dmg) {
        this.dmg = dmg; 
        tempDMG = 0; 
    }

    public void AddTempDamage(int tempDMG) {
        this.tempDMG += tempDMG;
    }

    public void ResetDMG() {
        tempDMG = 0; 
    }
}
namespace TrainGame.Components; 

public enum EnemyType {
    Artillery, //Big, Shoots vertically, homing bullets
    Barbarian, //Melee attacks around it
    Default,
    MachineGun, //Shoots a lot of bullets
    Ninja, //Dashes around, shoots occasionally
    Robot, //moves left to right and shoots up/down in bursts
    Shotgun, //Shoots in a small spread
    Sniper, //bullets are warned, travel far and fast and hit hard 
    Volley, //Shoots in a large spread
    Warrior //Shoots in a very wide spread, has high hp and damage, high reload time
}
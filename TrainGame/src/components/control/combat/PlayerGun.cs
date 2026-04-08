namespace TrainGame.Components;

using System;
using System.Collections.Generic;
using TrainGame.Components;
using TrainGame.Systems;

public class PlayerGun : IEquippable {

    private IShootPattern shootPattern;
    private Shooter shooter;

    private int damagePerUpgrade;
    private int damageLevel; 
    public readonly int MaxDamageLevel; 

    public int DamageLevel => damageLevel;
    public IShootPattern InstantiateShootPattern() => shootPattern.Clone();
    public Shooter InstantiateShooter() => shooter.Clone();

    public PlayerGun(Shooter shooter, IShootPattern shootPattern, int damagePerUpgrade = 10, int maxDamageLevel = 3) {
        this.shooter = shooter; 
        this.shootPattern = shootPattern;
        this.damagePerUpgrade = damagePerUpgrade;
        this.MaxDamageLevel = maxDamageLevel;
        this.damageLevel = 1; 
    }

    public void UpgradeDamage() {
        if (damageLevel < MaxDamageLevel) {
            ShooterWrap.UpgradeDamage(shootPattern, damagePerUpgrade);
            damageLevel++;
        }
    }
}
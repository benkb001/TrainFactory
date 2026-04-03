namespace TrainGame.Components;

using System;
using System.Collections.Generic;
using TrainGame.Components;

public class PlayerGun : IEquippable {

    private IShootPattern shootPattern;
    private Shooter shooter;
    public IShootPattern GetShootPattern() => shootPattern;
    public Shooter GetShooter() => shooter; 

    public PlayerGun(Shooter shooter, IShootPattern shootPattern) {
        this.shooter = shooter; 
        this.shootPattern = shootPattern;
    }
}
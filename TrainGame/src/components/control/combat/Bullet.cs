namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public class Bullet {
    private int maxFramesActive; 
    public int MaxFramesActive => maxFramesActive; 
    
    private int framesActive; 
    private int damage; 
    private bool isRemovedOnCollision;
    private BulletType bulletType; 
    private OnExpireEffect onExpireEffect;
    private OnCollideEffect onCollideEffect;

    public int Damage => damage; 
    public int FramesActive => framesActive; 
    public bool IsRemovedOnCollision => isRemovedOnCollision; 
    public BulletType GetBulletType() => bulletType; 
    public OnExpireEffect GetOnExpireEffect() => onExpireEffect;
    public OnCollideEffect GetOnCollideEffect() => onCollideEffect;

    public Bullet(int damage = 1, BulletType bulletType = BulletType.Default, 
        OnExpireEffect onExpireEffect = OnExpireEffect.Default, int maxFramesActive = 120, 
        bool isRemovedOnCollision = true) {
        this.damage = damage; 
        this.bulletType = bulletType; 
        this.onExpireEffect = onExpireEffect;
        this.maxFramesActive = maxFramesActive;
        this.isRemovedOnCollision = isRemovedOnCollision; 
        this.framesActive = 0;
    }

    public void Decay() {
        framesActive++; 
    }

    public bool ShouldRemove => framesActive > maxFramesActive; 
}
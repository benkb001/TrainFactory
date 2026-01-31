namespace TrainGame.Systems; 

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
    private static int maxFramesActive = 600; 
    public int MaxFramesActive => maxFramesActive; 
    
    private int framesActive; 
    private int damage; 
    private BulletType bulletType; 
    private OnExpireEffect onExpireEffect; 

    public int Damage => damage; 
    public int FramesActive => framesActive; 
    public BulletType GetBulletType() => bulletType; 
    public OnExpireEffect GetOnExpireEffect() => onExpireEffect;

    public Bullet(int damage = 1, BulletType bulletType = BulletType.Default, 
        OnExpireEffect onExpireEffect = OnExpireEffect.Default) {
        this.damage = damage; 
        this.bulletType = bulletType; 
        this.onExpireEffect = onExpireEffect;
        this.framesActive = 0;
    }

    public void Decay() {
        framesActive++; 
    }

    public bool ShouldRemove => framesActive > maxFramesActive; 
}

public class EnemyBullet {}
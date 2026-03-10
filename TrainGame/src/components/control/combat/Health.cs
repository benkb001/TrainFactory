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

public class Health {
    private int hp; 
    private int maxHP; 
    public int InvincibleFrames = 0;
    public int HP => hp; 
    public int MaxHP => maxHP; 

    public Health(int maxHP) {
        this.hp = maxHP;
        this.maxHP = maxHP;  
    }

    public void ReceiveDamage(int damage) {
        damage = InvincibleFrames > 0 ? 0 : Math.Max(0, damage); 
        hp = Math.Max(0, hp - damage); 
    }

    public void AddHP(int increase) {
        hp += increase; 
    }

    public void ResetHP() {
        hp = maxHP; 
    }

    public void SetHP(int hp) {
        this.hp = Math.Max(0, hp); 
        this.hp = Math.Min(hp, maxHP);
    }
}

public class Enemy {
    public readonly EnemyType Type; 
    
    public Enemy(EnemyType Type = EnemyType.Default) {
        this.Type = Type; 
    }
}
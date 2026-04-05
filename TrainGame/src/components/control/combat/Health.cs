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
    public int TempHP = 0; 
    public int TempMaxHP = 0;
    public int InvincibleFrames = 0;
    public int HP => hp + TempHP; 
    public int MaxHP => maxHP + TempMaxHP; 

    public Health(int maxHP) {
        this.hp = maxHP;
        this.maxHP = maxHP;  
    }

    public void ReceiveDamage(int damage) {
        damage = InvincibleFrames > 0 ? 0 : Math.Max(0, damage); 
        int damageToTemp = Math.Min(damage, TempHP);
        TempHP -= damageToTemp;
        damage -= damageToTemp;
        hp = Math.Max(0, hp - damage); 
    }

    public void AddHP(int increase) {
        hp += increase; 
        hp = Math.Min(hp, MaxHP); 
    }

    public void ResetHP() {
        hp = maxHP; 
        TempHP = 0; 
        TempMaxHP = 0;
    }

    public void SetHP(int hp) {
        this.hp = Math.Min(hp, maxHP);
        this.hp = Math.Max(0, this.hp); 
        this.TempHP = Math.Max(0, hp - this.hp);
    }
}

public class Enemy : IFlag<Enemy> {
    private static Enemy e; 
    //ICKY: don't need to use the same class for flag and type
    public readonly EnemyType Type; 
    
    public Enemy(EnemyType Type = EnemyType.Default) {
        this.Type = Type; 
    }

    public static Enemy Get() {
        if (e == null) {
            e = new Enemy();
        }
        return e;
    }
}
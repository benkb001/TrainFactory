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

class Health {
    private int hp; 
    private int maxHP; 
    public int HP => hp; 
    public int MaxHP => maxHP; 

    public Health(int maxHP) {
        this.hp = maxHP;
        this.maxHP = maxHP;  
    }

    public void ReceiveDamage(int damage) {
        hp = Math.Max(0, hp - damage); 
    }

    public void ResetHP() {
        hp = maxHP; 
    }
}

public class Enemy {}
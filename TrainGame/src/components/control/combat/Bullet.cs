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

    public int TempDMG; 
    public int Damage => damage + TempDMG; 
    public int FramesActive => framesActive; 

    public Bullet(int damage, int maxFramesActive = 120, int tempDMG = 0) {
        this.damage = damage; 
        this.maxFramesActive = maxFramesActive;
        this.TempDMG = tempDMG;
    }

    public void Decay() {
        framesActive++; 
    }

    public bool ShouldRemove => framesActive > maxFramesActive; 

    public Bullet Clone() {
        return new Bullet(damage, maxFramesActive, TempDMG);
    }
}
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

class Bullet {
    private int damage; 
    private WorldTime created; 
    public int Damage => damage; 

    public Bullet(WorldTime created, int damage = 1) {
        this.created = created;
        this.damage = damage; 
    }
}

public class PlayerBullet {}
public class EnemyBullet {}
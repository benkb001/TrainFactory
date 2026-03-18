namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants;
using TrainGame.Callbacks; 

public class EnemyWrap {

    public static Type[] EnemySignature = [typeof(Enemy), typeof(Health), typeof(Active)];

    public static EnemyWrap Draw(World w, Vector2 pos, EnemyType enemyType) {
        EnemyConst e = EnemyID.Enemies[enemyType];

        int enemyEnt = EntityFactory.AddUI(w, pos, e.Size, e.Size, setOutline: true); 
        Health h = new Health(e.HP);
        w.SetComponent<Health>(enemyEnt, h); 
        
        Shooter shooter = e.GetShooter();
        w.SetComponent<Shooter>(enemyEnt, shooter); 

        w.SetComponent<Enemy>(enemyEnt, new Enemy(enemyType)); 

        Movement movement = new Movement(
            speed: e.MoveSpeed,
            ticksBetweenMovement: e.TicksBetweenMovement,
            Type: e.MType,
            patternLength: e.MovePatternLength,
            ticksToMove: e.TicksToMove
        );
        w.SetComponent<Movement>(enemyEnt, movement); 

        w.SetComponent<Collidable>(enemyEnt, new Collidable()); 
        Armor armor = new Armor(e.Armor);
        w.SetComponent<Armor>(enemyEnt, armor); 
        
        return new EnemyWrap(enemyEnt, h, armor, movement);
    }

    public static int GetFirst(World w) {
        List<int> es = w.GetMatchingEntities(EnemySignature); 
        return es.Count > 0 ? es[0] : -1; 
    }

    private Armor armor; 
    private Movement movement; 
    private Health health; 
    private int e; 

    public Health GetHealth() => health; 
    public int Entity => e; 
    public Armor GetArmor() => armor; 
    public Movement GetMovement() => movement; 

    private EnemyWrap(int e, Health health, Armor armor, Movement movement) {
        this.e = e; 
        this.armor = armor; 
        this.movement = movement;
        this.health = health; 
    }
}
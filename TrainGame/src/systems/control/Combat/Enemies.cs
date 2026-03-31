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

        IMovementType movement = e.GetMovement();
        IShootPattern sp = e.GetShootPattern();
        MovementRegistry.AddMovement(w, movement, enemyEnt);
        ShootPatternRegistry.Add(w, sp, enemyEnt);

        w.SetComponent<Collidable>(enemyEnt, new Collidable()); 
        Armor armor = new Armor(e.Armor);
        w.SetComponent<Armor>(enemyEnt, armor); 
        
        foreach (IEnemyTrait trait in e.Traits) {
            EnemyTraitRegistry.Add(w, trait, enemyEnt);
        }
        
        return new EnemyWrap(enemyEnt, h, armor);
    }

    public static int GetFirst(World w) {
        List<int> es = w.GetMatchingEntities(EnemySignature); 
        return es.Count > 0 ? es[0] : -1; 
    }

    private Armor armor; 
    private Health health; 
    private int e; 

    public Health GetHealth() => health; 
    public int Entity => e; 
    public Armor GetArmor() => armor; 

    private EnemyWrap(int e, Health health, Armor armor) {
        this.e = e; 
        this.armor = armor; 
        this.health = health; 
    }
}
using TrainGame.Systems; 

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

[Collection("Sequential")]
public class DamageSystemTest {
    private (World, int) init(int damage = 1) {
        World w = WorldFactory.Build(); 

        int playerEnt = EntityFactory.AddUI(w, Vector2.Zero, 10, 10);
        w.SetComponent<Health>(playerEnt, new Health(10)); 
        w.SetComponent<Player>(playerEnt, new Player()); 
        
        int bulletEnt = EntityFactory.AddUI(w, Vector2.Zero, 2, 2);
        w.SetComponent<Bullet>(bulletEnt, new Bullet(damage)); 
        w.SetComponent<Enemy>(bulletEnt, new Enemy()); 

        MovementSystem.SetCollisionSpace(Vector2.Zero);
        return (w, playerEnt); 
    }

    [Fact]
    public void DamageSystem_EnemyBulletsCollidingWithPlayerShouldDamagePlayer() {
        (World w, int playerEnt) = init(); 

        w.Update(); 

        Assert.Equal(9, w.GetComponent<Health>(playerEnt).HP); 
    }

    [Fact]
    public void DamageSystem_ArmorShouldReduceIncomingDamageByItsDefense() {
        (World w, int playerEnt) = init(); 

        w.SetComponent<Armor>(playerEnt, new Armor(1)); 
        w.Update(); 
        Assert.Equal(10, w.GetComponent<Health>(playerEnt).HP); 
    }

    [Fact]
    public void DamageSystem_ParrierShouldReduceIncomingDamageToZero() {
        (World w, int playerEnt) = init(10); 
        
        Parrier p = new Parrier(); 
        p.StartParry(w.Time); 
        w.SetComponent<Parrier>(playerEnt, p);
        w.Update(); 
        Assert.Equal(10, w.GetComponent<Health>(playerEnt).HP); 
    }
}
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

class Damage {
    private int dmg; 
    private int tempDMG; 
    public int DMG => dmg + tempDMG; 

    public Damage(int dmg) {
        this.dmg = dmg; 
        tempDMG = 0; 
    }

    public void AddTempDamage(int tempDMG) {
        this.tempDMG += tempDMG;
    }

    public void ResetDMG() {
        tempDMG = 0; 
    }
}

public static class ShootSystem {
    public static void Register(World w) {
        w.AddSystem((w) => {
            if (VirtualMouse.LeftPressed()) {
                Shooter shooter = PlayerWrap.GetShooter(w); 
                if (shooter != null) {
                    Vector2 mousePos = w.GetWorldMouseCoordinates(); 
                    int itemEnt = PlayerWrap.GetHeldItemEnt(w); 
                    (Frame f, bool s2) = w.GetComponentSafe<Frame>(itemEnt); 

                    if (s2) {
                        //for homing, we need to pass a 'target ent', for now we 
                        //will do this. We could later have another system that 
                        //changes the homing entity for bullets with [Homing, Bullet, Player, Active] 
                        //to query for the closest [Enemy, Health, Frame, Active]
                        int enemyEnt = EnemyWrap.GetFirst(w);
                        ShooterWrap.TryShoot(w, shooter, f, mousePos, ShooterType.Player, enemyEnt);
                    }
                }
            }
        });
    }
}
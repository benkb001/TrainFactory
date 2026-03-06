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
                List<int> es = w.GetMatchingEntities([typeof(HeldItem), typeof(Active)])
                .Where(e => Weapons.GunMap.ContainsKey(w.GetComponent<HeldItem>(e).ItemId))
                .ToList(); 

                if (es.Count > 0) {
                    int e = es[0]; 

                    HeldItem gun = w.GetComponent<HeldItem>(e); 
                    Shooter shooter = Weapons.GunMap[gun.ItemID];
                    Vector2 mousePos = w.GetWorldMouseCoordinates(); 
                    (Frame f, bool s) = w.GetComponentSafe<Frame>(e); 

                    if (s) {
                        ShooterWrap.TryShoot(w, shooter, f, mousePos, ShooterType.Player);
                    }
                }
            }
        });
    }
}
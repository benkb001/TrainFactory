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

    private static WorldTime lastShot = new WorldTime(ticks: -24); 

    public static void Register(World w) {
        w.AddSystem((w) => {
            if (VirtualMouse.LeftPressed()) {
                if (w.Time.IsAfterOrAt(lastShot + new WorldTime(ticks: 24))) {
                    lastShot = w.Time.Clone(); 
                    List<int> es = w.GetMatchingEntities([typeof(HeldItem), typeof(Active)])
                    .Where(e => Weapons.GunMap.ContainsKey(w.GetComponent<HeldItem>(e).ItemId))
                    .ToList(); 

                    if (es.Count > 0) {
                        int e = es[0]; 

                        (Damage baseDamage, bool s1) = w.GetComponentSafe<Damage>(e);
                        int baseDMG = s1 ? baseDamage.DMG : 0; 
                        HeldItem gun = w.GetComponent<HeldItem>(e); 
                        int damage = Weapons.GunMap[gun.ItemId] + baseDMG; 
 
                        (Frame f, bool s2) = w.GetComponentSafe<Frame>(gun.LabelEntity);
                        Vector2 pos = s2 ? f.Position : Vector2.Zero; 

                        Vector2 mousePos = w.GetWorldMouseCoordinates(); 
                        Velocity bulletVelocity = new Velocity(Vector2.Normalize(mousePos - pos) * Constants.BulletSpeed);
                        int bulletEnt = EntityFactory.AddUI(w, pos, Constants.BulletSize, Constants.BulletSize, setOutline: true);
                        w.SetComponent<Velocity>(bulletEnt, bulletVelocity);
                        w.SetComponent<Bullet>(bulletEnt, new Bullet(damage));
                        w.SetComponent<Player>(bulletEnt, new Player()); 
                    }
                }
            }
        });
    }
}
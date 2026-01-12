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

public static class ShootSystem {

    private static WorldTime lastShot = new WorldTime(); 

    public static void Register(World w) {
        w.AddSystem((w) => {
            if (VirtualMouse.LeftPressed()) {
                if (w.Time.IsAfterOrAt(lastShot + new WorldTime(ticks: 12))) {

                    lastShot = w.Time.Clone(); 
                    List<HeldItem> ls = w.GetComponentArray<HeldItem>().Where(
                        kvp => Weapons.GunMap.ContainsKey(kvp.Value.ItemId)).Select(
                        kvp => kvp.Value).ToList();
                    
                    if (ls.Count > 0) {
                        HeldItem gun = ls[0];
                        int damage = Weapons.GunMap[gun.ItemId]; 
                        (Frame f, bool success) = w.GetComponentSafe<Frame>(gun.LabelEntity);
                        Vector2 pos = f.Position; 

                        Vector2 mousePos = w.GetWorldMouseCoordinates(); 
                        Velocity bulletVelocity = new Velocity(Vector2.Normalize(mousePos - pos) * Constants.BulletSpeed);
                        int bulletEnt = EntityFactory.AddUI(w, pos, 5, 5, setOutline: true);
                        w.SetComponent<Velocity>(bulletEnt, bulletVelocity);
                        w.SetComponent<Bullet>(bulletEnt, new Bullet(w.Time, damage));
                        w.SetComponent<PlayerBullet>(bulletEnt, new PlayerBullet()); 
                    }
                }
            }
        });
    }
}
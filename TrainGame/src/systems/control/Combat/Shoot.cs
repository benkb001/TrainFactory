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
    public static void Register(World w) {
        w.AddSystem((w) => {
            if ((VirtualMouse.LeftPressed() && (w.Time.Ticks == 0)) || VirtualMouse.LeftClicked()) {
                List<HeldItem> ls = w.GetComponentArray<HeldItem>().Where(
                    kvp => Weapons.GunMap.ContainsKey(kvp.Value.ItemId)).Select(
                    kvp => kvp.Value).ToList();
                
                if (ls.Count > 0) {
                    HeldItem gun = ls[0];
                    int damage = Weapons.GunMap[gun.ItemId]; 
                    (Frame f, bool success) = w.GetComponentSafe<Frame>(gun.LabelEntity);
                    Vector2 pos = f.Position; 
                    Console.WriteLine($"Shoot pos: {pos}, damage: {damage}");
                }
            }
        });
    }
}
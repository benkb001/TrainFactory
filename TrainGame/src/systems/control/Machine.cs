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
using TrainGame.Constants; 
using TrainGame.Utils; 

public class MachineUpdateSystem {
    public static void Register(World w) {
        Func<int, int> orderer = (e) => w.GetComponent<Machine>(e).Priority;
        Type[] ts = [typeof(Machine), typeof(Data)]; 
        Action<World, int> tf = (w, e) => {
            Machine m = w.GetComponent<Machine>(e); 
        
            if (m.State == CraftState.Idle) {
                m.StoreRecipe();
                int numCraftable = m.GetNumCraftable();
                if (numCraftable > 0) {
                    m.StartRecipe(numCraftable); 
                }
            }

            if (m.State == CraftState.Crafting) {
                m.UpdateCrafting(w.MiliticksPerUpdate); 
            }

            if (m.CraftComplete && m.State == CraftState.Crafting) {
                m.FinishRecipe(); 
            }

            if (m.State == CraftState.Delivering) {
                m.DeliverRecipe(); 
            }

        };
        w.AddSystem(ts, tf, orderer);
    }

    public static void RegisterConsumeTimeCrystals(World w) {
        w.AddSystem([typeof(Machine), typeof(Data)], (w, e) => {
            Machine m = w.GetComponent<Machine>(e); 
            if (m.Priority < 2 && m.NumRecipeToStore > 0) {
                float productPerTimeCrystal = m.GetProductsPerTimeCrystal(w.Time); 
                int numTimeCrystalsToTake = 1; 

                if (productPerTimeCrystal < 1f) {
                    numTimeCrystalsToTake = (int)(1f / productPerTimeCrystal); 
                }

                Inventory.Item i = m.Inv.Take(ItemID.TimeCrystal, numTimeCrystalsToTake); 
                m.Inv.Add(m.ProductItemId, (int)(i.Count * productPerTimeCrystal)); 
            }
        });
    }

    public static void RegisterEndFrame(World w) {
        w.AddSystem([typeof(Machine), typeof(Data)], (w, e) => {
            w.GetComponent<Machine>(e).EndFrame(); 
        });
    }
}
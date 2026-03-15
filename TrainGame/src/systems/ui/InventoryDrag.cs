namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

public class CurrentInventory {
    public readonly Inventory Inv; 

    public CurrentInventory(Inventory inv) {
        this.Inv = inv;
    }
}

public static class InventoryDragSystem {
    public const float Threshold = 30f; 
    private static Type[] types = [typeof(Frame), typeof(Draggable), typeof(Inventory.Item), 
    typeof(CurrentInventory), typeof(Active)]; 

    private static Action<World, int> transformer = (w, e) => {
        Draggable d = w.GetComponent<Draggable>(e); 
        if (d.IsBeingDropped()) {
            Inventory.Item curItem = w.GetComponent<Inventory.Item>(e); 
            Inventory.Item targetItem = curItem; 

            Inventory curInv = w.GetComponent<CurrentInventory>(e).Inv;
            Inventory targetInv = curInv;
            
            Vector2 closest = new Vector2(float.PositiveInfinity, float.PositiveInfinity); 
            Vector2 heldPosition = w.GetComponent<Frame>(e).Position; 
            Vector2 targetVector = d.SnapPosition; 

            List<int> itemEntities = w.GetMatchingEntities(types); 
            
            foreach (int itemEntity in itemEntities) {
                //if the item we're potentially moving it towards is the same as the item we're holding
                if (itemEntity == e) {
                    continue; 
                }

                Inventory.Item potentialTargetItem = w.GetComponent<Inventory.Item>(itemEntity); 
                
                Vector2 potentialTargetVector = w.GetComponent<Frame>(itemEntity).Position;
                Vector2 dist = potentialTargetVector - heldPosition; 
                
                if (closest.Length() > dist.Length()) {
                    closest = dist; 
                    targetInv = w.GetComponent<CurrentInventory>(itemEntity).Inv;
                    targetItem = potentialTargetItem; 
                    targetVector = potentialTargetVector; 
                } 
            }

            if (closest.Length() < Threshold) {
                int invOrganizeMsgEntity = EntityFactory.Add(w); 

                w.SetComponent<InventoryOrganizeMessage>(invOrganizeMsgEntity, new InventoryOrganizeMessage(
                    curInv,
                    targetInv,
                    curItem, 
                    targetItem, 
                    d, 
                    targetVector
                )); 
            }
        }
    }; 

    public static void Register(World world) {
        world.AddSystem(types, transformer); 
    }
}
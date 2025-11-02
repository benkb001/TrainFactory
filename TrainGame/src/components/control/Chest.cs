namespace TrainGame.Components;

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

public class Chest {
    private DrawInventoryMessage chestInvMessage; 
    private DrawInventoryMessage playerInvMessage; 
    public DrawInventoryMessage ChestInvMessage => chestInvMessage; 
    public DrawInventoryMessage PlayerInvMessage => playerInvMessage; 
    public float Margin => margin; 
    public float Padding => padding; 
    public float CellSize => cellSize; 
    float margin = 10f; 
    float padding = 2f;
    float cellSize = 100f; 
    private Vector2 chestInvDrawPosition; 

    public Chest(Inventory chestInv, int chestInvEntity, Inventory playerInv, int playerInvEntity) {
        chestInvDrawPosition = new Vector2(margin, margin); 
        float chestHeight = chestInv.GetRows() * cellSize; 
        
        chestInvMessage = new DrawInventoryMessage(
            Width: chestInv.GetCols() * cellSize,
            Height: chestHeight,
            Position: chestInvDrawPosition,
            Inv: chestInv, 
            Entity: chestInvEntity, 
            Padding: padding
        ); 

        playerInvMessage = new DrawInventoryMessage(
            Width: playerInv.GetCols() * cellSize, 
            Height: playerInv.GetRows() * cellSize, 
            Position: chestInvDrawPosition + new Vector2(0, chestHeight + margin), 
            Inv: playerInv, 
            Entity: playerInvEntity, 
            Padding: padding
        ); 
    }
}
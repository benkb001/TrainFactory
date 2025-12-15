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

    public float Margin => margin; 
    public float CellSize => cellSize; 
    float margin = 10f; 
    float cellSize = 100f; 
    public Vector2 ChestInvDrawPosition; 
    public Vector2 PlayerInvDrawPosition; 
    public Inventory ChestInv; 
    public Inventory PlayerInv; 
    public float ChestInvWidth; 
    public float ChestInvHeight; 
    public float PlayerInvWidth; 
    public float PlayerInvHeight; 

    public Chest(Inventory chestInv, Inventory playerInv) {
        ChestInv = chestInv; 
        PlayerInv = playerInv; 
        ChestInvDrawPosition = new Vector2(margin, margin); 
        ChestInvHeight = chestInv.GetRows() * cellSize; 
        ChestInvWidth = chestInv.GetCols() * cellSize; 
        PlayerInvHeight = playerInv.GetRows() * cellSize;
        PlayerInvWidth = playerInv.GetCols() * cellSize; 
        PlayerInvDrawPosition = new Vector2(margin, margin * 2 + ChestInvHeight); 
    }
}
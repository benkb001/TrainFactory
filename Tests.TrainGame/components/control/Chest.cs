using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
public class ChestTest {
    [Fact]
    public void Chest_ShouldSetCorrectInventoryPositionsAndSizes() {
        int playerRows = 2; 
        int playerCols = 2; 
        int chestRows = 2; 
        int chestCols = 2; 

        Inventory playerInv = new Inventory("Player", playerRows, playerCols); 
        Inventory chestInv = new Inventory("Chest", chestRows, chestCols); 

        Chest chest = new Chest(chestInv, playerInv);

        float correctChestWidth = chest.CellSize * chestCols; 
        float correctChestHeight = chest.CellSize * chestRows; 

        float correctPlayerWidth = chest.CellSize * playerCols; 
        float correctPlayerHeight = chest.CellSize * playerRows; 
        float correctPlayerX = chest.Margin;
        float correctPlayerY = (chest.Margin * 2) + correctChestHeight;

        Assert.Equal(correctPlayerWidth, chest.PlayerInvWidth); 
        Assert.Equal(correctPlayerHeight, chest.PlayerInvHeight); 
        Assert.Equal(correctChestWidth, chest.ChestInvWidth); 
        Assert.Equal(correctChestHeight, chest.ChestInvHeight);
        Assert.Equal(new Vector2(correctPlayerX, correctPlayerY), chest.PlayerInvDrawPosition);
    }

    [Fact]
    public void Chest_ShouldSetCorrectInventoryAndEntities() {
        int playerRows = 2; 
        int playerCols = 2; 
        int chestRows = 2; 
        int chestCols = 2; 

        Inventory playerInv = new Inventory("Player", playerRows, playerCols); 
        Inventory chestInv = new Inventory("Chest", chestRows, chestCols); 

        Chest chest = new Chest(chestInv, playerInv);

        Assert.Equal(playerInv, chest.PlayerInv);
        Assert.Equal(chestInv, chest.ChestInv); 
    }
}
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

        Assert.Equal(chest.Margin, chest.ChestInvMessage.Position.X); 
        Assert.Equal(chest.Margin, chest.ChestInvMessage.Position.Y); 
        Assert.Equal(correctChestWidth, chest.ChestInvMessage.Width);
        Assert.Equal(correctChestHeight, chest.ChestInvMessage.Height); 
        Assert.Equal(correctPlayerWidth, chest.PlayerInvMessage.Width); 
        Assert.Equal(correctPlayerHeight, chest.PlayerInvMessage.Height); 
        Assert.Equal(correctPlayerX, chest.PlayerInvMessage.Position.X); 
        Assert.Equal(correctPlayerY, chest.PlayerInvMessage.Position.Y); 
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

        Assert.Equal(playerInv, chest.PlayerInvMessage.Inv);
        Assert.Equal(chestInv, chest.ChestInvMessage.Inv); 
    }
}
using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 

public class InventoryTest {
    [Fact]
    public void Inventory_ShouldRespectConstructorArguments() {
        Inventory i = new Inventory("Test", 15, 25); 
        Assert.Equal("Test", i.GetId()); 
        Assert.Equal(15, i.GetRows()); 
        Assert.Equal(25, i.GetCols()); 
    }

    [Fact] 
    public void InventoryItem_ShouldRespectConstructorArguments() {
        Inventory i = new Inventory("Test", 2, 2); 
        Inventory.Item item = new Inventory.Item(Inv: i, Row: 1, Column: 2, ItemId: "Orange", Count: 3); 
        Assert.Equal("Test", item.Inv.GetId()); 
        Assert.Equal(1, item.Row); 
        Assert.Equal(2, item.Column); 
        Assert.Equal("Orange", item.ItemId); 
        Assert.Equal(3, item.Count); 
    }

    [Fact] 
    public void InventoryItem_ShouldKnowIfSomethingIsEmpty() {
        Inventory.Item item = new Inventory.Item(ItemId: "Apple", Count: 1);
        Assert.False(item.IsEmpty()); 

        Inventory.Item empty = new Inventory.Item(ItemId: "", Count: 0); 
        Assert.True(empty.IsEmpty()); 
    }

    [Fact]
    public void Inventory_ShouldInitiallyBeEmpty() {
        Inventory inv = new Inventory("Test", 10, 10); 

        bool allEmpty = true; 
        int i = 0; 
        while (i < 10 && allEmpty) {
            int j = 0; 
            while (j < 10 && allEmpty) {
                Inventory.Item item = inv.Get(i, j); 
                if (!item.IsEmpty()) {
                    allEmpty = false; 
                }
                j++; 
            }
            i++; 
        }
        Assert.True(allEmpty); 
    }

    [Fact]
    public void Inventory_ShouldAddItemsToZeroZeroIfEmpty() {
        Inventory inv = new Inventory("Test", 10, 10); 

        Assert.Equal("", inv.Get(0, 0).ItemId); 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 1)); 
        Assert.Equal("Apple", inv.Get(0, 0).ItemId);
        Assert.Equal(1, inv.Get(0, 0).Count); 
    }

    [Fact] 
    public void Inventory_ShouldAddToNextEmptySlotIfPositionUnspecifiedAndNoCellHasTheSpecifiedItemIdYet() {
        Inventory inv = new Inventory("Test", 10, 10); 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 1)); 
        inv.Add(new Inventory.Item(ItemId: "Banana", Count: 1)); 

        Assert.Equal("Banana", inv.Get(0, 1).ItemId); 
        Assert.Equal(1, inv.Get(0, 1).Count); 
    }

    [Fact]
    public void Inventory_ShouldAddToSpecifiedCell() {
        Inventory inv = new Inventory("Test", 10, 10); 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 1), 5, 3);
        Assert.Equal("Apple", inv.Get(5, 3).ItemId); 
        Assert.Equal(1, inv.Get(5, 3).Count); 
    }

    [Fact] 
    public void Inventory_ShouldAddToFirstCellWithSameItemIdIfAnyExistAndPositionUnspecified() {
        Inventory inv = new Inventory("Test", 10, 10); 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 1), 3, 3); 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 1)); 
        
        Assert.Equal(2, inv.Get(3, 3).Count); 

        //different position
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 1), 4, 4); 
        //this one should still go into 3, 3
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 1)); 
        Assert.Equal(3, inv.Get(3, 3).Count); 
    }

    [Fact] 
    public void Inventory_AddShouldReturnFalseIfSpecifiedCellHasADifferentItemId() {
        Inventory inv = new Inventory("Test", 10, 5); 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 10), 4, 4); 
        Assert.False(inv.Add(new Inventory.Item(ItemId: "Banana", Count: 1), 4, 4)); 
    }

    [Fact]
    public void Inventory_ShouldReturnFalseIfTryingToAddItemWithNewItemIdAndFull() {
        Inventory inv = new Inventory("Test", 5, 5); 
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 5; j++) {
                inv.Add(new Inventory.Item(ItemId: "Apple", Count: 1), i, j);
            }
        }
        Assert.False(inv.Add(new Inventory.Item(ItemId: "Banana", Count: 1))); 
    }

    [Fact]
    public void Inventory_TakeShouldReturnTheItemAtSpecifiedIndex() {
        Inventory inv = new Inventory("Test", 5, 5); 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 10), 3, 2); 
        Inventory.Item item = inv.Take(3, 2); 
        Assert.Equal("Apple", item.ItemId); 
        Assert.Equal(10, item.Count);
    }

    [Fact]
    public void Inventory_ShouldThrowErrorWhenAccessingInvalidPositions() {
        Inventory inv = new Inventory("Test", 10, 5); 

        Assert.Throws<InvalidOperationException>(() => {
            inv.Take(-1, 0); 
        }); 

        Assert.Throws<InvalidOperationException>(() => {
            inv.Take(10, 0); 
        }); 

        Assert.Throws<InvalidOperationException>(() => {
            inv.Take(0, -1); 
        }); 

        Assert.Throws<InvalidOperationException>(() => {
            inv.Take(0, 5); 
        }); 
    }

    [Fact]
    public void InventoryItem_ToStringShouldReturnItemIdCountPair() {
        Inventory.Item item = new Inventory.Item(ItemId: "Apple", Count: 10);
        Assert.Equal("Apple: 10", item.ToString()); 
    }
}
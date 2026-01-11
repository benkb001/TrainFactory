using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.Constants; 

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
        Assert.Equal(0, inv.Add(new Inventory.Item(ItemId: "Banana", Count: 1), 4, 4)); 
    }

    [Fact]
    public void Inventory_ShouldReturnFalseIfTryingToAddItemWithNewItemIdAndFull() {
        Inventory inv = new Inventory("Test", 5, 5); 
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 5; j++) {
                inv.Add(new Inventory.Item(ItemId: "Apple", Count: 1), i, j);
            }
        }
        Assert.Equal(0, inv.Add(new Inventory.Item(ItemId: "Banana", Count: 1))); 
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
    public void Inventory_TakeShouldReturnAnItemWithAsManyAsCouldBeProvided() {
        Inventory inv = new Inventory("Test", 2, 2); 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 5)); 
        Assert.Equal(5, inv.Take("Apple", 6).Count); 
    }

    [Fact]
    public void Inventory_AddShouldRespectStackSizes() {
        Inventory inv = new Inventory("Test", 1, 1); 
        Inventory.Item wood = new Inventory.Item(ItemId: ItemID.Wood, Count: 999999); 
        int woodStackSize = Constants.ItemStackSize(ItemID.Wood); 
        Assert.Equal(woodStackSize, inv.Add(wood));
        Inventory.Item added = inv.Get(0); 
        Assert.Equal(woodStackSize, added.Count); 
    }

    [Fact]
    public void Inventory_StackSizeShouldGrowWithLevel() {
        Inventory inv = new Inventory("Test", 1, 1); 
        inv.Upgrade(); 
        Inventory.Item wood = new Inventory.Item(ItemId: ItemID.Wood, Count: 999999); 
        int woodStackSize = Constants.ItemStackSize(ItemID.Wood); 
        Assert.True(inv.Add(wood) > woodStackSize);
        Inventory.Item added = inv.Get(0); 
        Assert.True(added.Count > woodStackSize); 
    }

    [Fact]
    public void Inventory_AddShouldDistributeItemsWhenIndexIsNotSpecified() {
        Inventory inv = new Inventory("Test", 1, 3); 
        int woodStackSize = Constants.ItemStackSize(ItemID.Wood); 
        int num_adding = woodStackSize * 2; 
        Inventory.Item wood = new Inventory.Item(ItemId: ItemID.Wood, Count: num_adding);

        inv.Add(new Inventory.Item(ItemId: "Other", Count: 1), 0, 1); 
        Assert.Equal(num_adding, inv.Add(wood));
        Assert.Equal(woodStackSize, inv.Get(0).Count); 
        Assert.Equal(woodStackSize, inv.Get(2).Count);  
    }

    [Fact]
    public void Inventory_AddShouldNotDistributeWhenAddingToASpecificIndex() {
        Inventory inv = new Inventory("Test", 1, 3); 
        int woodStackSize = Constants.ItemStackSize(ItemID.Wood); 
        int num_adding = woodStackSize * 2; 
        Inventory.Item wood = new Inventory.Item(ItemId: ItemID.Wood, Count: num_adding);

        Assert.Equal(woodStackSize, inv.Add(wood, 0, 1));
        Assert.Equal(woodStackSize, inv.Get(0, 1).Count); 
    }

    [Fact]
    public void Inventory_ShouldNotAddItemsNotInWhitelistIfItHasAWhitelist() {
        Inventory inv = new Inventory("Test", 10, 10); 
        inv.Whitelist("Good"); 
        Inventory.Item bad = new Inventory.Item(ItemId: "Bad", Count: 1); 
        Assert.Equal(0, inv.Add(bad));
        Assert.Equal(0, inv.Add(bad, 0, 0));
    }

    [Fact]
    public void Inventory_SolidInvShouldNotAddLiquids() {
        Inventory inv = new Inventory("Test", 10, 10); 
        inv.SetSolid(); 
        Inventory.Item liquid = new Inventory.Item(ItemId: ItemID.Liquids[0], Count: 10); 
        Assert.Equal(0, inv.Add(liquid)); 
    }

    [Fact]
    public void Inventory_LiquidInvShouldAddLiquids() {
        Inventory inv = new Inventory("Test", 10, 10); 
        inv.SetLiquid(); 
        Inventory.Item liquid = new Inventory.Item(ItemId: ItemID.Liquids[0], Count: 1);
        Assert.Equal(1, inv.Add(liquid)); 
    }

    [Fact]
    public void Inventory_TransferFromListShouldTakeItemCountItems() {
        List<Inventory> invs = new(); 
        for (int i = 0; i < 4; i++) {
            Inventory inv = new Inventory($"T{i}", 1, 1); 
            inv.Add("Test", 1); 
            invs.Add(inv); 
        }
        Inventory invOther = new Inventory($"Other", 1, 1); 
        int taken = invOther.TransferFrom(invs, "Test", 3); 
        Assert.Equal(3, taken); 
        Assert.Equal(0, invs[0].ItemCount("Test")); 
        Assert.Equal(0, invs[2].ItemCount("Test")); 
        Assert.Equal(1, invs[3].ItemCount("Test")); 
    }

    [Fact]
    public void Inventory_TransferToListShouldSpreadItemsBetweenInventories() {
        List<Inventory> invs = new(); 
        for (int i = 0; i < 4; i++) {
            Inventory inv = new Inventory($"T{i}", 1, 1); 
            invs.Add(inv); 
        }
        Inventory invOther = new Inventory($"Other", 1, 4); 
        for (int i = 0; i < 4; i++) {
            invOther.Add("StackSize1", 1); 
        }
        int given = invOther.TransferTo(invs, "StackSize1", 4); 
        Assert.Equal(4, given); 
        Assert.Equal(1, invs[0].ItemCount("StackSize1")); 
        Assert.Equal(1, invs[3].ItemCount("StackSize1")); 
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
    public void Inventory_ItemCountShouldBeEqualToNumAddedBeforeTaking() {
        Inventory inv = new Inventory("Test", 1, 1); 
        inv.Add("Apple", 10); 
        Assert.Equal(10, inv.ItemCount("Apple")); 
    }


    [Fact]
    public void Inventory_TransferToShouldSendAsManyItemsAsPossibleToOtherInventories() {
        List<Inventory> invs = new(); 

        Inventory from = new Inventory("Test", 2, 2); 
        from.Add("StackSize1", 4); 

        Inventory to1 = new Inventory("T1", 1, 1); 
        invs.Add(to1); 
        Inventory to2 = new Inventory("T2", 1, 1); 
        invs.Add(to2); 

        from.TransferTo(invs, "StackSize1", 3);
        Assert.Equal(2, from.ItemCount("StackSize1")); 

        Assert.Equal(1, to1.ItemCount("StackSize1")); 
        Assert.Equal(1, to2.ItemCount("StackSize1")); 
    }

    [Fact]
    public void Inventory_TransferFromShouldTakeAsManyItemsAsPossibleFromInventories() {
        List<Inventory> invs = new(); 

        Inventory to = new Inventory("Test", 2, 2); 

        Inventory from1 = new Inventory("T1", 1, 1); 
        invs.Add(from1); 
        Inventory from2 = new Inventory("T2", 1, 1); 
        invs.Add(from2); 

        from1.Add("StackSize1", 1); 
        from2.Add("StackSize1", 1); 

        to.TransferFrom(invs, "StackSize1", 3); 
        Assert.Equal(2, to.ItemCount("StackSize1"));
        Assert.Equal(0, from1.ItemCount("StackSize1"));
        Assert.Equal(0, from2.ItemCount("StackSize1"));
    }

    [Fact]
    public void Inventory_ItemCountShouldSayTheNumberOfItemsContained() {
        Inventory inv = new Inventory("Test", 2, 2); 
        Assert.Equal(0, inv.ItemCount("Apple")); 
        inv.Add(new Inventory.Item(ItemId: "Apple", Count: 2)); 
        Assert.Equal(2, inv.ItemCount("Apple")); 
    }

    [Fact]
    public void InventoryItem_ToStringShouldReturnItemIdCountPair() {
        Inventory.Item item = new Inventory.Item(ItemId: "Apple", Count: 10);
        Assert.Equal("Apple: 10", item.ToString()); 
    }
}
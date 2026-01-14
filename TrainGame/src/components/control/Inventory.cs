namespace TrainGame.Components;

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.ECS; 
using TrainGame.Constants; 

public class Inventory : IID {
    private string inventoryId; 

    private List<Item> items; 
    private Dictionary<string, int> itemCountMap; 
    private int rows; 
    private int cols; 
    private int level; 
    private HashSet<string> whitelist;
    private bool filtered = false; 
    private CartType filter; 

    public int Level => level; 
    public string Id => inventoryId; 
    public int Rows => rows; 
    public int Cols => cols; 
    public CartType Filter => filter; 

    public Inventory(string id, int r, int c, int level = 1, CartType type = CartType.General) {
        inventoryId = id;
        rows = r; 
        cols = c; 
        this.level = level; 
        items = new(); 
        whitelist = new(); 
        itemCountMap = new(); 

        for (int i = 0; i < r; i++) {
            for (int j = 0; j < c; j++) {
                items.Add(new Item(Row: i, Column: j, Inv: this)); 
            }
        }

        if (type == CartType.Freight) {
            SetSolid(); 
        } else if (type == CartType.Liquid) {
            SetLiquid(); 
        }

        filter = type; 
    }

    //todo: need to re-factor in other places because it might add a portion of the items but not all
    //returns the number of items it added to the inventory
    public int Add(Item i, bool newCell = false) {

        if (filtered && !whitelist.Contains(i.ItemId)) {
            return 0; 
        }

        int index = -1; 

        if (!newCell) {
            index = items.FindIndex(item => item.ItemId == i.ItemId && item.Count < stackSize(i.ItemId));
        }

        if (index == -1 ) {
            index = items.FindIndex(item => item.IsEmpty());
        }
        
        if (index == -1) {
            return 0; 
        }

        (int num_adding, int num_remaining) = getNumToAdd(i, index); 
        i.Count = num_adding; 

        (int row, int col) = GetRowColIndex(index); 
        i.Row = row; 
        i.Column = col; 

        items[index].ItemId = i.ItemId; 
        items[index].Count += i.Count; 
        i.Inv = this;
        
        editItemCountMap(i.ItemId, num_adding); 

        if (num_remaining > 0) {
            return num_adding + Add(new Item(ItemId: i.ItemId, Count: num_remaining)); 
        }

        return num_adding; 
    }

    public int Add(string itemID, int count) {
        return Add(new Inventory.Item(ItemId: itemID, Count: count)); 
    }

    public int Add(Item i, int row, int col) {
        if (filtered && !whitelist.Contains(i.ItemId)) {
            return 0; 
        }

        ensureValidIndices(row, col); 
        int idx = getIndex(row, col); 
        if (items[idx].ItemId == i.ItemId || items[idx].IsEmpty()) {
            (int num_adding, int _) = getNumToAdd(i, idx); 
            i.Row = row; 
            i.Column = col; 
            i.Inv = this; 
            items[idx].ItemId = i.ItemId; 
            items[idx].Count += num_adding; 
            editItemCountMap(i.ItemId, num_adding); 
            return num_adding; 
        }
        return 0; 
    }

    public bool TakeRecipe(Dictionary<string, int> recipe) {
        foreach (KeyValuePair<string, int> kvp in recipe) {
            if (ItemCount(kvp.Key) < kvp.Value) {
                return false; 
            }
        }

        foreach (KeyValuePair<string, int> kvp in recipe) {
            Take(kvp.Key, kvp.Value); 
        }

        return true; 
    }

    //TODO: Test
    public Item Take(string itemId, int count) {
        int found = 0; 

        for (int i = 0; i < items.Count && found < count; i++) {
            if (items[i].ItemId == itemId) {
                int taken = Math.Min(count - found, items[i].Count); 
                if (taken == items[i].Count) {
                    items[i].ItemId = "";
                }
                found += taken; 
                items[i].Count -= taken; 
            }
        }

        editItemCountMap(itemId, -1 * found); 
        return new Item(ItemId: itemId, Count: found); 
    }

    public Item TakeAll(string itemID) {
        return Take(itemID, ItemCount(itemID)); 
    }

    //TODO: Test
    public int ItemCount(string itemId) {
        if (itemCountMap.ContainsKey(itemId)) {
            return itemCountMap[itemId];
        }

        return 0; 
    }

    public void Upgrade() {
        level++; 
    }

    public Item Take(int row, int col) {
        ensureValidIndices(row, col); 
        int idx = getIndex(row, col);
        Item i = items[idx]; 
        i.Row = -1; 
        i.Column = -1; 
        items[idx] = new Item(Row: row, Column: col, Inv: this); 
        editItemCountMap(i.ItemId, -1 * i.Count);
        return i; 
    }

    public Item Take(int row, int col, int Count) {
        ensureValidIndices(row, col); 
        int idx = getIndex(row, col);
        Item i = items[idx]; 

        int taken = Math.Min(i.Count, Count); 
        string id = i.ItemId; 

        i.Count -= taken; 

        if (i.Count == 0) {
            i.ItemId = ""; 
        }

        editItemCountMap(id, -1 * taken); 
        return new Item(Row: -1, Column: -1, Inv: this, ItemId: id, Count: taken);
    }

    //todo: test
    public void TransferTo(Inventory otherInv, string itemID, int count) {
        Inventory.Item taken = Take(itemID, count); 
        int takenCount = taken.Count; 
        int added = otherInv.Add(taken); 
        Add(itemID, takenCount - added);
    }

    public void TransferAllTo(Inventory otherInv, string itemID) {
        TransferTo(otherInv, itemID, ItemCount(itemID)); 
    }

    public int TransferFrom(List<Inventory> invs, string itemID, int itemCount) { 
        int taken = 0; 
        int i = 0; 

        while (taken < itemCount && i < invs.Count) {
            taken += invs[i].Take(itemID, itemCount - taken).Count; 
            i++;
        }

        int added = Add(itemID, taken); 
        i = 0; 
        int addedBack = 0; 
        int numToAddBack = taken - added;

        while (addedBack < numToAddBack && i < invs.Count) {
            addedBack += invs[i].Add(itemID, added - numToAddBack);
            i++; 
        }

        return added; 
    }

    public int TransferTo(List<Inventory> invs, string itemID, int itemCount) {
        int added = 0; 
        int i = 0; 

        int taken = Take(itemID, itemCount).Count; 

        while (added < taken && i < invs.Count) {
            added += invs[i].Add(itemID, taken - added);
            i++;
        }

        i = 0; 
        int numToAddBack = taken - added; 
        Add(itemID, numToAddBack); 

        return added; 
    }

    public Item Get(int index) {
        (int row, int col) = GetRowColIndex(index); 
        return Get(row, col); 
    }

    public Item Get(int row, int col) {
        ensureValidIndices(row, col); 
        return items[getIndex(row, col)];
    }

    public string GetId() {
        return inventoryId; 
    }

    public string GetItemId(int index) {
        return items[index].ItemId; 
    }

    public string GetItemId(int row, int col) {
        return GetItemId(getIndex(row, col)); 
    }

    public int GetCols() {
        return cols; 
    }

    public int GetRows() {
        return rows; 
    }

    public string GetContentsFormatted() {
        Dictionary<string, int> contents = new(); 
        foreach (Item i in items) {
            contents[i.ItemId] = contents.ContainsKey(i.ItemId) ? contents[i.ItemId] + i.Count : i.Count; 
        }
        return string.Join(Environment.NewLine, contents.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
    }

    public void Whitelist(string itemId) {
        filtered = true; 
        whitelist.Add(itemId); 
    }

    public void SetSolid() {
        filter = CartType.Freight; 
        foreach (string s in ItemID.Solids) {
            Whitelist(s); 
        }
    }

    public void SetLiquid() {
        filter = CartType.Liquid; 
        foreach (string s in ItemID.Liquids) {
            Whitelist(s); 
        }
    }

    public string GetID() {
        return Id; 
    }

    private (int, int) getNumToAdd(Item i, int index) {
        Item cur = Get(index); 
        if (cur.ItemId == "") {
            cur.Count = 0; 
        }

        int stack_size = stackSize(i.ItemId); 
        int num_adding = Math.Min(stack_size - cur.Count, i.Count);
        int num_remaining = i.Count - num_adding; 
        return (num_adding, num_remaining); 
    }

    private (int, int) GetRowColIndex(int idx) {
        ensureValidIndices(idx); 
        return (idx / cols, idx % cols); 
    }

    private int getIndex(int row, int col) {
        return (row * cols) + col; 
    }

    private void ensureValidIndices(int index) {
        if (index < 0 || index > items.Count) {
            throw new InvalidOperationException($"Index {index} out of bounds for {rows}x{cols} inventory"); 
        }
    }

    private void ensureValidIndices(int row, int col) {
        if (row < 0 || col < 0 || row >= rows || col >= cols) {
            throw new InvalidOperationException($"Inventory position {row}, {col} invalid for a {rows}x{cols} inventory"); 
        }

        ensureValidIndices(getIndex(row, col)); 
    }

    private void editItemCountMap(string item, int delta) {
        if (itemCountMap.ContainsKey(item)) {
            itemCountMap[item] += delta; 
        } else {
            itemCountMap[item] = delta; 
        }
    }

    public void EnsureValidIndices(int row, int col) {
        ensureValidIndices(row, col); 
    }

    private int stackSize(string itemId) {
        return Constants.ItemStackSize(itemId) * level; 
    }

    public class Item {
        //TODO: Make private idk 
        public string Id => ItemId; 
        public string ItemId; 
        public int Count; 
        public int Row; 
        public int Column; 
        public Inventory Inv; 

        public Item(Inventory Inv = null, string ItemId = "", int Count = 0, int Row = 0, int Column = 0) {
            this.ItemId = ItemId; 
            this.Count = Count; 
            this.Row = Row; 
            this.Column = Column; 
            this.Inv = Inv; 
        }

        public override string ToString() {
            if (Count == 0 || ItemId == "") {
                return ""; 
            }
            return $"{ItemId}: {Count}";
        }

        public bool IsEmpty() {
            return ItemId == "" || Count == 0; 
        }
    }
}

public static class InventoryWrap {
    public static Inventory GetyByEntityOrId(World w, int entity = -1, string inventoryId = "") {
        if (w.ComponentContainsEntity<Inventory>(entity)) {
            return w.GetComponent<Inventory>(entity); 
        }
        return GetByID(w, inventoryId); 
    }

    public static Inventory GetPlayerInv(World w) {
        return GetByID(w, Constants.PlayerInvID); 
    }

    public static Inventory GetByID(World w, string inventoryID) {
        return w.GetComponentArray<Inventory>().Where(pair => pair.Value.GetId() == inventoryID).FirstOrDefault().Value; 
    }

    public static int GetEntity(string inventoryId, World w) {
        return w.GetMatchingEntities([typeof(Data), typeof(Inventory)]).Where(
            e => w.GetComponent<Inventory>(e).Id == inventoryId).FirstOrDefault(); 
    }

    public static (float, float) GetUI(Inventory inv, float scale = 1f) {
        float width = inv.GetCols() * (Constants.InventoryCellSize + Constants.InventoryPadding) + 
            Constants.InventoryPadding; 
        float height = inv.GetRows() * (Constants.InventoryCellSize + Constants.InventoryPadding) +
            Constants.InventoryPadding + Constants.LabelHeight;
        return (width * scale, height * scale);
    }

    public static int ItemCount(List<Inventory> invs, string itemID) {
        return invs.Aggregate(0, (acc, inv) => acc + inv.ItemCount(itemID));
    }
}
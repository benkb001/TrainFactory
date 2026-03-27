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
using TrainGame.Utils;

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
    public string ID => inventoryId;
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
                items.Add(new Item()); 
            }
        }

        if (type == CartType.Freight) {
            SetSolid(); 
        } else if (type == CartType.Liquid) {
            SetLiquid(); 
        }

        filter = type; 
    }

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

        items[index].ItemId = i.ItemId; 
        items[index].Count += i.Count; 
        
        editItemCountMap(i.ItemId, num_adding); 

        if (num_adding > 0 && num_remaining > 0) {
            return num_adding + Add(new Item(ItemId: i.ItemId, Count: num_remaining)); 
        }

        return num_adding; 
    }

    public int Add(string itemID, int count) {
        return Add(new Inventory.Item(ItemId: itemID, Count: count)); 
    }

    public Dictionary<string, int> Add(Dictionary<string, int> items) {
        Dictionary<string, int> added = new(); 
        foreach (KeyValuePair<string, int> kvp in items) {
            string itemID = kvp.Key; 
            int count = kvp.Value; 
            added[itemID] = Add(itemID, count); 
        }

        return added; 
    }

    public int Add(Item i, int row, int col) {
        if (filtered && !whitelist.Contains(i.ItemId)) {
            return 0; 
        }

        ensureValidIndices(row, col); 
        int idx = getIndex(row, col); 
        if (items[idx].ItemId == i.ItemId || items[idx].IsEmpty()) {
            (int num_adding, int _) = getNumToAdd(i, idx); 
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

    //ICKY
    public (int, int) GetIndices(Item item) {
        for (int i = 0; i < items.Count; i++) {
            if (items[i] == item) {
                return GetRowColIndex(i);
            }
        }

        return (-1, -1);
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

    public int UpgradeExponential() {
        int prevLevel = level; 
        level = Math.Max(level + 1, (int)(level * Constants.ExponentialInvSizeUpgradeFactor));
        return level - prevLevel;
    }

    public Item Take(int row, int col) {
        ensureValidIndices(row, col); 
        int idx = getIndex(row, col);
        Item i = items[idx]; 
        items[idx] = new Item(); 
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
        return new Item(ItemId: id, Count: taken);
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

    public void AddItemTo(Inventory targetInv, int curRow, int curCol, int targetRow, int targetCol) {
        Item curItem = this.Take(curRow, curCol);
        Item targetItem = targetInv.Take(targetRow, targetCol);

        if (targetItem.ItemId == curItem.ItemId) {
            if (targetItem.Count == stackSize(targetItem.ID)) {
                this.Add(targetItem, curRow, curCol); 
                targetInv.Add(curItem, targetRow, targetCol);
                return;
            }
            curItem.Count += targetItem.Count;
            targetItem.Count = 0; 
            targetItem.ItemId = "";
        }

        int addedToTarget = targetInv.Add(curItem, targetRow, targetCol); 
        int addedToCur = this.Add(targetItem, curRow, curCol);

        this.Add(curItem.ID, curItem.Count - addedToTarget); 
        targetInv.Add(targetItem.ID, targetItem.Count - addedToCur);
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

    public void SetArmor() {
        filter = CartType.Armor; 
        foreach (string s in EquipmentID.Armor) {
            Whitelist(s); 
        }
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

    public bool AreValidIndices(int row, int col) {
        try {
            ensureValidIndices(row, col); 
            return true; 
        } catch {
            return false;
        }
    }

    private int stackSize(string itemId) {
        return Constants.ItemStackSize(itemId) * level; 
    }

    public class Item {
        //TODO: Make private idk 
        public string Id => ItemId; 
        public string ID => ItemId; 
        public string ItemId; 
        public int Count; 

        public Item(string ItemId = "", int Count = 0) {
            this.ItemId = ItemId; 
            this.Count = Count; 
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
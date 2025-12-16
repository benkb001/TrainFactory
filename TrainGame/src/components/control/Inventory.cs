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

public class Inventory {
    private string inventoryId; 

    private List<Item> items; 
    private int rows; 
    private int cols; 
    private int level = 1; 
    private HashSet<string> whitelist;
    private bool filtered = false; 

    public int Level => level; 

    public Inventory(string id, int r, int c) {
        inventoryId = id;
        rows = r; 
        cols = c; 
        items = new(); 
        whitelist = new(); 
        for (int i = 0; i < r; i++) {
            for (int j = 0; j < c; j++) {
                items.Add(new Item(Row: i, Column: j, Inv: this)); 
            }
        }
    }

    //todo: need to re-factor in other places because it might add a portion of the items but not all
    //returns the number of items it added to the inventory
    public int Add(Item i) {

        if (filtered && !whitelist.Contains(i.ItemId)) {
            return 0; 
        }

        int index = items.FindIndex(item => item.ItemId == i.ItemId && item.Count < stackSize(i.ItemId));

        if (index == -1) {
            index = items.FindIndex(item => item.ItemId == "");
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
        
        if (num_remaining > 0) {
            return num_adding + Add(new Item(ItemId: i.ItemId, Count: num_remaining)); 
        }

        return num_adding; 
    }

    public int Add(Item i, int row, int col) {
        if (filtered && !whitelist.Contains(i.ItemId)) {
            return 0; 
        }

        ensureValidIndices(row, col); 
        int idx = getIndex(row, col); 
        if (items[idx].ItemId == i.ItemId || items[idx].ItemId == "") {
            (int num_adding, int _) = getNumToAdd(i, idx); 
            i.Row = row; 
            i.Column = col; 
            i.Inv = this; 
            items[idx].ItemId = i.ItemId; 
            items[idx].Count += num_adding; 
            return num_adding; 
        }
        return 0; 
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
        return new Item(ItemId: itemId, Count: found); 
    }

    //TODO: Test
    public int ItemCount(string itemId) {
        int found = 0; 
        for (int i = 0; i < items.Count; i++) {
            if (items[i].ItemId == itemId) {
                found += items[i].Count; 
            }
        }
        return found; 
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
        return i; 
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
        foreach (string s in ItemID.Solids) {
            Whitelist(s); 
        }
    }

    public void SetLiquid() {
        foreach (string s in ItemID.Liquids) {
            Whitelist(s); 
        }
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

    private int stackSize(string itemId) {
        return Constants.ItemStackSize(itemId) * level; 
    }

    public class Item {
        //TODO: Make private idk 
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
            return ItemId == "" && Count == 0; 
        }
    }

    public static Inventory GetyByEntityOrId(World w, int entity = -1, string inventoryId = "") {
        if (w.ComponentContainsEntity<Inventory>(entity)) {
            return w.GetComponent<Inventory>(entity); 
        }
        return w.GetComponentArray<Inventory>().Where(pair => pair.Value.GetId() == inventoryId).FirstOrDefault().Value; 
    }
}
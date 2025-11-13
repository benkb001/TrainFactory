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

public class Inventory {
    private string inventoryId; 

    private List<Item> items; 
    private int rows; 
    private int cols; 

    public Inventory(string id, int r, int c) {
        inventoryId = id;
        rows = r; 
        cols = c; 
        items = new(); 
        for (int i = 0; i < r; i++) {
            for (int j = 0; j < c; j++) {
                items.Add(new Item(Row: i, Column: j, Inv: this)); 
            }
        }
    }

    public bool Add(Item i) {
        int index = items.FindIndex(item => item.ItemId == i.ItemId);

        if (index == -1) {
            index = items.FindIndex(item => item.ItemId == "");
        }
        
        if (index == -1) {
            return false; 
        }

        (int row, int col) = GetRowColIndex(index); 
        i.Row = row; 
        i.Column = col; 

        items[index].ItemId = i.ItemId; 
        items[index].Count += i.Count; 
        i.Inv = this;
        
        return true; 
    }

    public bool Add(Item i, int row, int col) {
        ensureValidInices(row, col); 
        int idx = (row * cols) + col; 
        if (items[idx].ItemId == i.ItemId || items[idx].ItemId == "") {
            i.Row = row; 
            i.Column = col; 
            i.Inv = this; 
            items[idx].ItemId = i.ItemId; 
            items[idx].Count += i.Count; 
            return true; 
        }
        return false; 
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

    public Item Take(int row, int col) {
        ensureValidInices(row, col); 
        int idx = (row * cols) + col; 
        Item i = items[idx]; 
        i.Row = -1; 
        i.Column = -1; 
        items[idx] = new Item(Row: row, Column: col, Inv: this); 
        return i; 
    }

    public Item Get(int row, int col) {
        ensureValidInices(row, col); 
        return items[(row * cols) + col];
    }

    public string GetId() {
        return inventoryId; 
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

    private (int, int) GetRowColIndex(int idx) {
        if (idx < 0 || idx >= (rows * cols)) {
            throw new InvalidOperationException($"Index {idx} out of bounds for {rows}x{cols} inventory"); 
        }
        return (idx / cols, idx % cols); 
    }

    private void ensureValidInices(int row, int col) {
        if (row < 0 || col < 0 || row >= rows || col >= cols) {
            throw new InvalidOperationException($"Inventory position {row}, {col} invalid for a {rows}x{cols} inventory"); 
        }
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
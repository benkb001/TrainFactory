namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

//TODO: TEST
public class Cart {
    private float mass; 

    public readonly Inventory Inv;
    public readonly string Id; 
    public float Mass => mass; 
    
    public Cart(string Id, int rows, int cols, float mass = 1f, bool solid = true) {
        Inv = new Inventory($"{Id}_inv", rows, cols); 
        this.Id = Id; 
        this.mass = mass; 
        if (solid) {
            Inv.SetSolid(); 
        } else {
            Inv.SetLiquid(); 
        }
    }
}
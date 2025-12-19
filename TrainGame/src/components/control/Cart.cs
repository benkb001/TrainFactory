namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Constants;

//TODO: TEST
public class Cart {
    private float mass; 

    public readonly Inventory Inv;
    public readonly string Id; 
    public float Mass => mass; 
    
    public Cart(string Id, CartType type) {
        Inv = new Inventory($"{Id}_inv", Constants.CartRows, Constants.CartCols); 
        this.Id = Id; 
        
        if (type == CartType.Freight) {
            Inv.SetSolid(); 
            mass = Constants.FreightCartBaseMass; 
        } else {
            Inv.SetLiquid(); 
            mass = Constants.LiquidCartBaseMass; 
        }
    }
}
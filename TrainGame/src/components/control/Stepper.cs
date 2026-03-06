namespace TrainGame.Components;

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

public class Stepper {
    public int Value => value; 
    private int value; 
    private int min; 
    private int max; 

    public Stepper(int v, int min = -Int32.MaxValue, int max = Int32.MaxValue) {
        this.value = v; 
        this.min = min; 
        this.max = max; 
    }

    public void Adjust(int step) {
        value += step; 
        value = Math.Max(min, value); 
        value = Math.Min(max, value); 
    }
}
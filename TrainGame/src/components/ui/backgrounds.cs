namespace TrainGame.Components;

using System.Collections.Generic;
using System; 
using System.Linq;
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Backgrounds {
    public List<(Background, Frame)> Ls = new(); 

    public void Add(Background b, Frame f) {
        Ls.Add((b, f)); 
    }
}
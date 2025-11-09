namespace TrainGame.Components;

using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

public class Sprite {
    private Texture2D texture; 
    private int depth; 

    public Sprite(Texture2D tx, int d) {
        texture = tx; 
        depth = d; 
    }

    public int GetDepth() {
        return depth; 
    }

    public Texture2D GetTexture() {
        return texture; 
    }
}
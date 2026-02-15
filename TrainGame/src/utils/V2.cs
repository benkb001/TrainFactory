namespace TrainGame.Utils; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class V2 {
    public Vector2 Vec; 

    public V2(Vector2 Vec) {
        this.Vec = Vec;
    }

    public V2(float x, float y) {
        this.Vec = new Vector2(x, y);
    }
    
    public float this[int i] {
        get {
            return i % 2 == 0 ? Vec.X : Vec.Y; 
        }
        set {
            if (i % 2 == 0) {
                Vec.X = value;
            } else { 
                Vec.Y = value; 
            }
        }
    }

    public static implicit operator Vector2(V2 v) {
        return v.Vec;
    }
}
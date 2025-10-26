namespace TrainGame.Components;

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 

public class LinearLayout {
    private List<int> children;
    private string direction;
    private string spacing; 
    public float Padding; 

    public LinearLayout(string d, string s) {
        SetDirection(d); 
        SetSpacing(s); 
        children = new List<int>(); 
    }

    //alignLow is left/top, alignHigh is right/bottom, shoves the children frames towards that direction
    public void SetDirection(string d) {
        d = d.ToLower(); 
        if (d != "horizontal" && d != "vertical") {
            throw new InvalidOperationException(
                $"LinearLayout direction must be 'vertical' or 'horziontal', {d} invalid"
            );
        }
        direction = d; 
    }

    public void SetSpacing(string s) {
        s = s.ToLower(); 
        if (s != "alignlow" && s != "alignhigh" && s != "spaceeven") {
            throw new InvalidOperationException(
                $"LinearLayout spacing must be 'alignlow', 'alignhigh', or 'spaceeven', {s} invalid"
            ); 
        }
        spacing = s; 
    }

    public string GetDirection() {
        return direction; 
    }

    public string GetSpacing() {
        return spacing; 
    }

    public bool IsVertical() {
        return direction == "vertical";
    }

    public bool IsHorizontal() {
        return direction == "horizontal"; 
    }

    public bool IsAlignLow() {
        return spacing == "alignlow"; 
    }

    public bool IsAlignHigh() {
        return spacing == "alignhigh"; 
    }

    public bool IsSpaceEven() {
        return spacing == "spaceeven"; 
    }

    public List<int> GetChildren() {
        return children; 
    }

    public bool AddChild(int e) {
        bool duplicateChild = children.Contains(e);
        if (!duplicateChild) {
            children.Add(e); 
        }
        return !duplicateChild; 
    }

    public bool RemoveChild(int e) {
        return children.Remove(e); 
    }
}
namespace TrainGame.Components;

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class Button {
    public bool Clicked => ClickType == Click.Left; 
    public bool ShiftClicked => ClickType == Click.Shift; 
    
    public bool Hovered = false; 
    public Click ClickType = Click.None; 
    public float Depth = 0f; 
    public int TicksHeld = 0; 
    public int RightTicksHeld = 0;

    public Button(bool Clicked = false, float Depth = 0f) {
        if (Clicked) {
            this.ClickType = Click.Left; 
        }

        this.Depth = Depth; 
    }

    public Button(Click type) {
        this.ClickType = type; 
    }
}
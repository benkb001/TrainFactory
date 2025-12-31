namespace TrainGame.Components;

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public enum Click {
    Left,
    Right,
    Shift,
    None
}

public class Button {
    public bool Clicked => ClickType == Click.Left; 
    public bool ShiftClicked => ClickType == Click.Shift; 
    
    public Click ClickType = Click.None; 
    public float Depth = 0f; 
    public int TicksHeld = 0; 
    private Action onClick;

    public Button(bool Clicked = false, float Depth = 0f, Action onClick = null) {
        if (Clicked) {
            this.ClickType = Click.Left; 
        }

        this.Depth = Depth; 
        this.onClick = onClick; 
    }

    public void OnClick() {
        if (onClick != null) {
            onClick(); 
        }
    }
}
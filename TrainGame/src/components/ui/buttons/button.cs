namespace TrainGame.Components;

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class Button {
    public bool Clicked = false; 
    public float Depth = 0f; 
    private Action onClick;

    public Button(bool Clicked = false, float Depth = 0f, Action onClick = null) {
        this.Clicked = Clicked; 
        this.Depth = Depth; 
        this.onClick = onClick; 
    }

    public void OnClick() {
        if (onClick != null) {
            onClick(); 
        }
    }
}
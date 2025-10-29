namespace TrainGame.Components;

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.ECS; 

public class Draggable {
    public Vector2 SnapPosition; 
    public Vector2 RelativeClickPosition; 
    private int dragState = DragState.Released; 

    public Draggable() {
        SnapPosition = Vector2.Zero; 
        RelativeClickPosition = Vector2.Zero; 
    }

    public Draggable(Vector2 v) {
        SnapPosition = v; 
        RelativeClickPosition = Vector2.Zero; 
    }

    public void PickUp() {
        dragState = DragState.Held; 
    }

    public void Drop() {
        if (IsHeld()) {
            dragState = DragState.BeingDropped; 
        }
    }

    public void Release() {
        dragState = DragState.Released; 
    }

    public bool IsHeld() {
        return dragState == DragState.Held; 
    }

    public bool IsReleased() {
        return dragState == DragState.Released; 
    }

    public bool IsBeingDropped() {
        return dragState == DragState.BeingDropped; 
    }
}

public static class DragState {
    public static int Released = 0; 
    public static int Held = 1;
    public static int BeingDropped = 2; 
}
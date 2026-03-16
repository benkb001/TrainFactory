namespace TrainGame.Components;

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class DrawMachineRequestMessage {
    private Machine machine; 
    public float Height; 
    public float Width; 
    public float Margin; 
    public Vector2 Position; 
    public bool SetMenu; 

    public DrawMachineRequestMessage(Machine machine, float Width, float Height, Vector2 Position, float Margin = 0f, 
        bool SetMenu = false) {

        this.machine = machine; 
        this.Width = Width; 
        this.Height = Height; 
        this.Margin = Margin; 
        this.Position = Position; 
        this.SetMenu = SetMenu; 
    }

    public Machine GetMachine() {
        return machine; 
    }
}
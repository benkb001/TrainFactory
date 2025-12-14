namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.ECS; 

public class DrawCallback {
    private Action cb; 

    public DrawCallback(Action cb) {
        this.cb = cb;
    }

    public void Run() {
        cb(); 
    }

    public Action GetCallback() {
        return cb; 
    }
}
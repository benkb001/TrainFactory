namespace TrainGame.ECS; 

using System.Collections.Generic;
using System; 
using System.Linq;
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color; 

using _Color = System.Drawing.Color; 
using _Rectangle = System.Drawing.Rectangle;

using TrainGame.Constants; 
using TrainGame.Components; 
using TrainGame.Utils; 

public static class EntityFactory {
    public static int Add(World w, bool setScene = true, bool setActive = true) {
        int e = w.AddEntity(); 

        if (setScene) {
            w.SetComponent<Scene>(e, new Scene(0));
        } 
        
        if (setActive) {
            w.SetComponent<Active>(e, Active.Get()); 
        }

        return e; 
    }

    public static int AddButton(World w, float x, float y, float width, float aspectRatio, int depth, Texture2D spr) {
        int e = Add(w); 
        w.SetComponent<Frame>(e, new Frame(x, y, width, width * aspectRatio)); 
        w.SetComponent<Sprite>(e, new Sprite(spr, depth)); 
        w.SetComponent<Button>(e, new Button()); 
        return e; 
    }

    public static int AddInventory(World w, Inventory inv) {
        int e = Add(w, setScene: false); 
        w.SetComponent<Inventory>(e, inv); 
        return e; 
    }
}   
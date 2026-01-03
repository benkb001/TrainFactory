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
using TrainGame.Systems;

public static class EntityFactory {
    public static int Add(World w, bool setScene = true, bool setActive = true, bool setData = false, int scene = -1,
        SceneType type = SceneType.None) {
        int e = w.AddEntity(); 

        //todo: test
        if (setData) {
            w.SetComponent<Data>(e, Data.Get()); 
            return e; 
        }

        if (type == SceneType.None) {
            type = SceneSystem.CurrentScene; 
        }

        w.SetComponent<Scene>(e, new Scene(type: type));
        if (type == SceneSystem.CurrentScene) {
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
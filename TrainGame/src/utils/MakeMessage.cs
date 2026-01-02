namespace TrainGame.Utils; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.ECS;
using TrainGame.Constants; 
using TrainGame.Callbacks; 

public class MakeMessage {

    public static void Add<T>(World w, T msg) {
        int e = EntityFactory.Add(w, setScene: false); 
        w.SetComponent<T>(e, msg); 
    }

    public static void DrawInventory(Inventory inv, World w, Vector2 Position, float Width, float Height) {
        DrawInventoryCallback.Create(w, inv, Position, Width, Height, Padding: 5f, SetMenu: true, DrawLabel: true); 
    }
}
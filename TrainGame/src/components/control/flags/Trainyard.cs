namespace TrainGame.Components; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TrainGame.ECS; 
using TrainGame.Constants;

public class TrainYard {
    private static TrainYard inst; 
    public static TrainYard Get() {
        if (inst is null) {
            inst = new TrainYard(); 
        }
        return inst; 
    }
}

public class TrainYardWrap {
    public static void Draw(World w, Vector2 pos) {
        int e = EntityFactory.AddUI(w, pos, Constants.TileWidth, Constants.TileWidth, 
            text: "Train Yard", setInteractable: true, setOutline: true, setCollidable: true); 
        w.SetComponent<TrainYard>(e, TrainYard.Get()); 
    }
}
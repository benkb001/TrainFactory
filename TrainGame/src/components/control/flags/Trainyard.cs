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
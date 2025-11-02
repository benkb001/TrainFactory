using Color = Microsoft.Xna.Framework.Color; 

//TODO: Test
namespace TrainGame.Components;
public class Background {
    public Color BackgroundColor; 
    public float Depth = 0f; 

    public Background(Color BackgroundColor, float Depth = 0f) {
        this.BackgroundColor = BackgroundColor; 
        this.Depth = Depth; 
    }
}
using TrainGame.Components; 

public class DrawCallbackTest {
    [Fact]
    public void DrawCallback_ShouldRespectConstructors() {
        Action cb = () => {}; 
        DrawCallback draw_callback = new DrawCallback(cb); 
        Assert.Equal(cb, draw_callback.GetCallback()); 
    }
}
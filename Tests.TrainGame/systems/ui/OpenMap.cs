
using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 
using TrainGame.Systems; 
using TrainGame.Utils; 
//sequential because global state (keyboard)
[Collection("Sequential")]
public class OpenMapSystemTest {
    [Fact]
    public void OpenMapSystem_ShouldCreateADrawMapMessageWhenOpenMapKeyIsPressed() {
        VirtualKeyboard.Reset(); 

        World w = new World(); 
        RegisterComponents.All(w); 
        OpenMapSystem.Register(w); 
        Assert.Empty(w.GetComponentArray<DrawMapMessage>()); 
        VirtualKeyboard.Press(KeyBinds.OpenMap);
        w.Update();
        Assert.Single(w.GetComponentArray<DrawMapMessage>()); 

        VirtualKeyboard.Reset(); 
    }   
}
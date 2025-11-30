
using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems;

public class CameraLockSystemTest {
    [Fact]
    public void CameraLockSystem_ShouldLockCameraWhenThereAreActiveMenuElements() {
        World w = WorldFactory.Build(); 
        CameraLockSystem.Register(w); 
        Assert.False(w.CameraLocked); 
        int e = EntityFactory.Add(w); 
        w.SetComponent<Menu>(e, Menu.Get()); 
        w.Update(); 
        Assert.True(w.CameraLocked); 
    }
}
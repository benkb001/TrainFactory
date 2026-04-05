namespace TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Constants; 
using TrainGame.Utils; 
public static class WorldFactory {
    public static World Build() {
        World w = new World(); 
        RegisterComponents.All(w); 
        RegisterSystems.All(w); 
        return w; 
    }
}
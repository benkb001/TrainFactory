
using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

//idrk what else to write so sanity check test
public class RegisterSystemsTest {
    [Fact]
    public void RegisterSystemsAll_ShouldRegisterMoreThanZeroSystems() {
        World w = new World(); 
        RegisterComponents.All(w); //necessary to avoid error on registering systems
        RegisterSystems.All(w); 
        Assert.True(w.SystemCount() > 0); 
    }
}
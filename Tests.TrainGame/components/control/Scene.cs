using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 

public class SceneTest {
    [Fact]
    public void Scene_ShouldRespectConstructorArguments() {
        Scene s = new Scene(10); 
        Assert.Equal(10, s.Value);
    }
}


using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

public class LayoutPositionSystemTest {
    [Fact]
    public void LayoutPositionSystem_ShouldSetLayoutToBeAboveTopLeftCornerOfBodyEntity() {
        World w = WorldFactory.Build();
        int bodyEntity = EntityFactory.Add(w); 

        Vector2 bodyPosition = new Vector2(15f, 25f); 
        w.SetComponent<Frame>(bodyEntity, new Frame(bodyPosition, 100f, 100f)); 

        int labelEntity = EntityFactory.Add(w); 
        float labelHeight = 5f; 
        w.SetComponent<Frame>(labelEntity, new Frame(0f, 0f, 5f, labelHeight)); 
        Label l = new Label(bodyEntity); 
        w.SetComponent<Label>(labelEntity, l); 
        w.Update(); 
        Assert.Equal(bodyPosition - new Vector2(0f, labelHeight), w.GetComponent<Frame>(labelEntity).Position); 
    }
}
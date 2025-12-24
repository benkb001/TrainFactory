using System.Collections.Generic;
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class ClearLLSystemTest {
    [Fact]
    public void ClearLLSystem_ShouldRemoveChildrenFromLL(){
        World w = WorldFactory.Build(); 

        LinearLayout llParent = new LinearLayout("Horizontal", "alignlow"); 
        LinearLayout llChild = new LinearLayout("Horizontal", "alignlow"); 
        
        int parentEnt = EntityFactory.Add(w); 
        int childEnt = EntityFactory.Add(w); 

        w.SetComponent<LinearLayout>(parentEnt, llParent); 
        w.SetComponent<LinearLayout>(childEnt, llChild); 

        w.SetComponent<Frame>(parentEnt, new Frame(0, 0, 100, 100)); 
        w.SetComponent<Frame>(childEnt, new Frame(0, 0, 50, 50)); 

        LinearLayoutWrap.AddChild(childEnt, parentEnt, llParent, w); 

        int grandChildEnt = EntityFactory.Add(w); 
        w.SetComponent<Frame>(grandChildEnt, new Frame(0, 0, 25, 25)); 
        LinearLayoutWrap.AddChild(grandChildEnt, childEnt, llChild, w); 

        Assert.True(w.EntityExists(parentEnt)); 
        Assert.True(w.EntityExists(childEnt)); 
        Assert.True(w.EntityExists(grandChildEnt)); 

        int dm = EntityFactory.Add(w); 
        w.SetComponent<ClearLLMessage>(dm, new ClearLLMessage(parentEnt)); 
        w.Update();

        Assert.False(w.EntityExists(childEnt)); 
        Assert.False(w.EntityExists(grandChildEnt)); 
    }
}
using TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

public class ResetPlayerStatsSystemTest {
    [Fact]
    public void ResetPlayerStatsSystem_ShouldResetPlayerHealthWhenTheyLeaveACity() {
        World w = WorldFactory.Build(); 
        int playerEnt = EntityFactory.Add(w, setData: true); 
        Health hp = new Health(10); 
        w.SetComponent<Player>(playerEnt, new Player()); 
        w.SetComponent<Health>(playerEnt, hp); 
        w.SetComponent<Armor>(playerEnt, new Armor(1)); 
        hp.AddHP(1); 
        Assert.Equal(11, hp.HP); 
        Train t = TrainWrap.GetTestTrain(); 
        t.HasPlayer = true; 
        MakeMessage.Add<EmbarkedMessage>(w, new EmbarkedMessage(t)); 
        w.Update(); 
        Assert.Equal(10, hp.HP); 
    }
}
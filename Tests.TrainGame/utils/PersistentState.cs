using TrainGame.Utils; 
using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Text.Json.Nodes; 
using System.Linq; 
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Callbacks; 
using TrainGame.Systems; 
using TrainGame.Constants;

public class PersistentStateTest {
    [Fact]
    public void PersistentState_SaveShouldWriteToSpecifiedFile() {
        World w = WorldFactory.Build(); 
        Bootstrap.InitWorld(w); 
        PersistentState.Save(w, "test"); 
    }
}
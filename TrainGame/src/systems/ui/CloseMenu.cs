namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils;
using TrainGame.Constants; 

public class CloseMenuSystem() {

    public static void Register(World world) {
        Action<World> update = (w) => {
            if (VirtualKeyboard.IsClicked(KeyBinds.Interact)) {
                List<int> menuEntities = w.GetMatchingEntities([typeof(Menu), typeof(Active)]); 

                if (menuEntities.Count >= 1) {
                    int popEntity = EntityFactory.Add(w, setScene: false);
                    w.SetComponent<PopSceneMessage>(popEntity, PopSceneMessage.Get());
                }
            }
        };
        world.AddSystem(update); 
    }
}
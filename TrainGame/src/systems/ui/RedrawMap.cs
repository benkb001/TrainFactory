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

//TODO: Test
public class RedrawMapSystem() {
    private static Action<World> update = (w) => {

        int num_pop_msgs = w.GetComponentArray<PopSceneMessage>().Count; 
        List<int> es = w.GetMatchingEntities([typeof(Scene), typeof(MapUIFlag)]); 
        if (num_pop_msgs > 0 && es.Any(e => {
            int scene = w.GetComponent<Scene>(e).Value; 
            Console.WriteLine($"cur entity scene: {scene}");
            return scene == num_pop_msgs; 
        })) {
            Console.WriteLine("Redraw map system activated");
            int dm = EntityFactory.Add(w, setScene: false); 
            int pushEnt = EntityFactory.Add(w, setScene: false); 
            int popEnt = EntityFactory.Add(w, setScene: false); 
            w.SetComponent<DrawMapMessage>(dm, DrawMapMessage.Get());
            w.SetComponent<PushSceneMessage>(pushEnt, PushSceneMessage.Get()); 
            w.SetComponent<PopSceneMessage>(popEnt, PopSceneMessage.Get()); 
        }
    }; 

    public static void Register(World world) {
        world.AddSystem(update); 
    }

}
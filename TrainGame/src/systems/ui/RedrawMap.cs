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

public class RedrawMapSystem() {
    private static Action<World> update = (w) => {
        bool should_redraw = false;

        if (!should_redraw) {
            List<int> tuiEnts = w.GetMatchingEntities([typeof(TrainUI), typeof(MapUIFlag), typeof(Active)]);
            if (tuiEnts.Any(e => {
                Train t = w.GetComponent<TrainUI>(e).GetTrain(); 
                return t.HasPlayer && t.IsArriving(); 
            })) {
                should_redraw = true; 
            }
        }

        if (!should_redraw) {
            List<int> trainDataEnts = w.GetMatchingEntities([typeof(Train), typeof(Data)]); 
            bool sceneIsMap = w.GetMatchingEntities([typeof(MapUIFlag), typeof(Active)]).Count > 0; 
            if (sceneIsMap && trainDataEnts.Any(e => {
                Train t = w.GetComponent<Train>(e); 
                return t.IsEmbarking; 
            })) {
                should_redraw = true; 
            }
        }

        if (should_redraw) {
            MakeMessage.Add<DrawMapMessage>(w, DrawMapMessage.Get()); 
        }
    }; 

    public static void Register(World world) {
        world.AddSystem(update); 
    }

}
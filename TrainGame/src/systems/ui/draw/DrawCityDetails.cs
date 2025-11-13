namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

//required order: 
// cityClick -> push -> drawCityDetail 
public class DrawCityDetailsSystem() {
    public static void Register(World world) {
        Type[] ts = [typeof(DrawCityDetailsMessage)]; 
        Action<World, int> tf = (w, e) => {
            //TODO: i think if we are doing a draw city detail message, 
            //the logic to choose the message should be in this file 
            //instead of whoever generates it. They should merely pass the city 
            //in the message
            DrawCityDetailsMessage detailMsg = w.GetComponent<DrawCityDetailsMessage>(e); 
            int drawnEntity = EntityFactory.Add(w); 
            w.SetComponent<Menu>(drawnEntity, Menu.Get()); 
            w.SetComponent<Frame>(drawnEntity, new Frame(detailMsg.Position, 100f, 200f));
            w.SetComponent<TextBox>(drawnEntity, new TextBox(detailMsg.Detail)); 
            w.SetComponent<Background>(drawnEntity, new Background(Colors.UIBG, Depth: Depth.MapCityDetail)); 
            w.SetComponent<Outline>(drawnEntity, new Outline(Colors.UIAccent, Depth: Depth.MapCityDetail)); 
            w.RemoveEntity(e); 
        }; 
        world.AddSystem(ts, tf); 
    }

}
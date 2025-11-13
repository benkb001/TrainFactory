namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 

//required order: 
// cityClick -> push -> drawCityDetail 
public class CityClickSystem() {
    public static void Register(World world) {
        Type[] ts = [typeof(CityUI), typeof(Button), typeof(Frame), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            if (w.GetComponent<Button>(e).Clicked) {
                int pushEntity = EntityFactory.Add(w); 
                w.SetComponent<PushSceneMessage>(pushEntity, PushSceneMessage.Get()); 
                int detailEntity = EntityFactory.Add(w); 
                Inventory inv = w.GetComponent<CityUI>(e).GetCity().Inv; 
                Frame f = w.GetComponent<Frame>(e);
                //TODO: just draw the inventory? 
                w.SetComponent<DrawCityDetailsMessage>(detailEntity, 
                    new DrawCityDetailsMessage(inv.GetContentsFormatted(), f.Position)); 
            }
        }; 

        world.AddSystem(ts, tf); 
    }

}
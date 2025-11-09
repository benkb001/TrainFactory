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

//needs to be trainInteract -> push -> drawMap
public class DrawMapSystem() {
    public static void Register(World world) {

        Type[] ts = [typeof(DrawMapMessage)]; 
        Action<World, int> tf = (w, e) => {
            List<int> cityDataEntities = w.GetMatchingEntities([typeof(City), typeof(Active)]); 
            foreach (int cityDataEntity in cityDataEntities) {
                City city = w.GetComponent<City>(cityDataEntity); 
                int cityDrawnEntity = EntityFactory.Add(w); 
                w.SetComponent<CityUI>(cityDrawnEntity, new CityUI(city));
                w.SetComponent<Frame>(cityDrawnEntity, new Frame(city.UiX, city.UiY, City.UIWidth, City.UIHeight)); 
                //todo put in cityUI width/height 
                w.SetComponent<Outline>(cityDrawnEntity, new Outline(Depth: Depth.MapCity)); 
                w.SetComponent<TextBox>(cityDrawnEntity, new TextBox(city.CityId, Depth: Depth.MapCity)); 
                w.SetComponent<Button>(cityDrawnEntity, new Button()); 
                w.SetComponent<Menu>(cityDrawnEntity, Menu.Get()); 
            }

            List<int> trainDataEntities = w.GetMatchingEntities([typeof(Train), typeof(Active)]); 
            foreach (int trainDataEntity in trainDataEntities) {
                Train train = w.GetComponent<Train>(trainDataEntity); 
                int trainDrawnEntity = EntityFactory.Add(w); 
                w.SetComponent<TrainUI>(trainDrawnEntity, new TrainUI(train)); 
                w.SetComponent<Frame>(trainDrawnEntity, new Frame(0, 0, TrainUI.Width, TrainUI.Height)); 
                w.SetComponent<TextBox>(trainDrawnEntity, new TextBox(train.Id));
                w.SetComponent<Outline>(trainDrawnEntity, new Outline(Depth: Depth.MapTrain));
                w.SetComponent<Button>(trainDrawnEntity, new Button()); 
            }
            w.RemoveEntity(e);
        }; 

        world.AddSystem(ts, tf); 
    }

}
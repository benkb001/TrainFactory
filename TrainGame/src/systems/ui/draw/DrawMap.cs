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
            Vector2 topleft = w.GetCameraTopLeft(); 
            int flagEnt = EntityFactory.Add(w); 
            w.SetComponent<MapUIFlag>(flagEnt, MapUIFlag.Get()); 
            List<int> cityDataEntities = w.GetMatchingEntities([typeof(City), typeof(Active)]); 
            int playerLocationEntity = -1; 
            foreach (int cityDataEntity in cityDataEntities) {
                City city = w.GetComponent<City>(cityDataEntity); 
                int cityDrawnEntity = EntityFactory.Add(w); 
                w.SetComponent<CityUI>(cityDrawnEntity, new CityUI(city));
                w.SetComponent<Frame>(cityDrawnEntity, new Frame(topleft + new Vector2(city.UiX, city.UiY), 
                    City.UIWidth, City.UIHeight)); 
                //todo put in cityUI width/height 
                w.SetComponent<Outline>(cityDrawnEntity, new Outline(Depth: Depth.MapCity)); 
                w.SetComponent<TextBox>(cityDrawnEntity, new TextBox(city.CityId, Depth: Depth.MapCity)); 
                w.SetComponent<Button>(cityDrawnEntity, new Button(Depth: Depth.MapCity)); 
                w.SetComponent<Menu>(cityDrawnEntity, Menu.Get()); 
                if (city.HasPlayer) {
                    playerLocationEntity = cityDrawnEntity; 
                }
            }

            if (playerLocationEntity != -1) {
                Frame f = w.GetComponent<Frame>(playerLocationEntity); 
                int labelEntity = EntityFactory.Add(w); 
                w.SetComponent<Frame>(labelEntity, new Frame(0, 0, f.GetWidth() / 2, f.GetWidth() / 4)); 
                w.SetComponent<Outline>(labelEntity, new Outline()); 
                w.SetComponent<TextBox>(labelEntity, new TextBox("Player"));
                w.SetComponent<Label>(labelEntity, new Label(playerLocationEntity)); 
            }

            List<int> trainDataEntities = w.GetMatchingEntities([typeof(Train), typeof(Active)]); 
            foreach (int trainDataEntity in trainDataEntities) {
                Train train = w.GetComponent<Train>(trainDataEntity); 
                int trainDrawnEntity = EntityFactory.Add(w); 
            
                w.SetComponent<TrainUI>(trainDrawnEntity, new TrainUI(train)); 
                w.SetComponent<MapUIFlag>(trainDrawnEntity, MapUIFlag.Get()); 
                Frame f = new Frame(0, 0, TrainUI.Width, TrainUI.Height); 
                w.SetComponent<Frame>(trainDrawnEntity, f); 
                w.SetComponent<TextBox>(trainDrawnEntity, new TextBox(train.Id));
                w.SetComponent<Outline>(trainDrawnEntity, new Outline(Depth: Depth.MapTrain));
                w.SetComponent<Button>(trainDrawnEntity, new Button(Depth: Depth.MapTrain)); 

                if (train.HasPlayer) {
                    int labelEntity = EntityFactory.Add(w); 
                    w.SetComponent<Frame>(labelEntity, new Frame(0, 0, f.GetWidth() / 2, f.GetWidth() / 4)); 
                    w.SetComponent<Outline>(labelEntity, new Outline()); 
                    w.SetComponent<TextBox>(labelEntity, new TextBox("Player"));
                    w.SetComponent<Label>(labelEntity, new Label(trainDrawnEntity)); 
                }
            }
            w.RemoveEntity(e);
        }; 

        world.AddSystem(ts, tf); 
    }
}
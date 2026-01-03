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
            SceneSystem.EnterScene(w, SceneType.Map); 
            Vector2 topleft = w.GetCameraTopLeft(); 
            int flagEnt = EntityFactory.Add(w); 
            w.SetComponent<MapUIFlag>(flagEnt, MapUIFlag.Get()); 
            w.SetComponent<Menu>(flagEnt, new Menu());
            List<int> cityDataEntities = w.GetMatchingEntities([typeof(City), typeof(Data)]); 
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

            List<int> trainDataEntities = w.GetMatchingEntities([typeof(Train), typeof(Data)]); 
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

            //add clock in top-right corner

            float clockWidth = w.ScreenWidth / 4f; 
            float clockHeight = clockWidth / 2f; 
            Vector2 clockPosition = topleft + new Vector2(w.ScreenWidth - clockWidth - 10f, 10f); 
            int clockEnt = EntityFactory.Add(w); 
            w.SetComponent<GameClockView>(clockEnt, GameClockView.Get()); 
            w.SetComponent<Frame>(clockEnt, new Frame(clockPosition, clockWidth, clockHeight)); 
            w.SetComponent<TextBox>(clockEnt, new TextBox("")); 
            w.SetComponent<Outline>(clockEnt, new Outline()); 

            w.RemoveEntity(e);

            //add speed buttons in top-left corner 
            float buttonWidth = clockWidth / 2f; 
            float buttonHeight = clockHeight / 2f; 
            int[] es = new int[3]; 
            for (int i = 0; i < 3; i++) {
                int buttonEnt = EntityFactory.Add(w); 
                es[i] = buttonEnt; 
                w.SetComponent<Frame>(buttonEnt, new Frame(topleft + new Vector2((10 + ((buttonWidth + 10) * i)), 10), 
                    buttonWidth, buttonHeight)); 
                w.SetComponent<Outline>(buttonEnt, new Outline()); 
                w.SetComponent<Button>(buttonEnt, new Button()); 
            }

            w.SetComponent<SlowTimeButton>(es[0], SlowTimeButton.Get()); 
            w.SetComponent<TextBox>(es[0], new TextBox("Slow Time")); 
            w.SetComponent<PauseButton>(es[1], PauseButton.Get()); 
            w.SetComponent<TextBox>(es[1], new TextBox("Pause Time")); 
            w.SetComponent<SpeedTimeButton>(es[2], SpeedTimeButton.Get()); 
            w.SetComponent<TextBox>(es[2], new TextBox("Fast Time")); 
        }; 

        world.AddSystem(ts, tf); 
    }
}
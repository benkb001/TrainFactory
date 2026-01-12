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

public static class DrawMapSystem {
    public static void Register(World world) {

        Type[] ts = [typeof(DrawMapMessage)]; 
        Action<World, int> tf = (w, e) => {
            SceneSystem.EnterScene(w, SceneType.Map); 
            w.UnlockCameraPan(); 
            w.LockCameraZoom(); 
            Vector2 topleft = w.GetCameraTopLeft(); 
            int flagEnt = EntityFactory.Add(w); 
            w.SetComponent<MapUIFlag>(flagEnt, MapUIFlag.Get()); 
            w.SetComponent<Menu>(flagEnt, new Menu());
            List<int> cityDataEntities = w.GetMatchingEntities([typeof(City), typeof(Data)]); 
            int playerLocationEntity = -1; 

            //draw cities
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

            //draw player label
            if (playerLocationEntity != -1) {
                Frame f = w.GetComponent<Frame>(playerLocationEntity); 
                int labelEntity = EntityFactory.Add(w); 
                w.SetComponent<Frame>(labelEntity, new Frame(0, 0, f.GetWidth() / 2, f.GetWidth() / 4)); 
                w.SetComponent<Outline>(labelEntity, new Outline()); 
                w.SetComponent<TextBox>(labelEntity, new TextBox("Player"));
                w.SetComponent<Label>(labelEntity, new Label(playerLocationEntity)); 
            }

            //draw moving trains
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

            //add container for hud on left
            float hudWidth = w.ScreenWidth / 3f; 
            float hudHeight = w.ScreenHeight - 20f; 
            LinearLayoutContainer hud = LinearLayoutWrap.Add(
                w, 
                new Vector2(10, 0),
                hudWidth, 
                hudHeight,
                direction: "vertical", 
                align: "alignlow", 
                outline: false,
                screenAnchor: true
            );

            //add clock to top of HUD

            int clockEnt = EntityFactory.Add(w); 
            w.SetComponent<GameClockView>(clockEnt, GameClockView.Get()); 
            w.SetComponent<TextBox>(clockEnt, new TextBox("")); 
            w.SetComponent<Outline>(clockEnt, new Outline()); 
            float clockWidth = hudWidth; 
            float clockHeight = hudWidth / 5f; 
            w.SetComponent<Frame>(clockEnt, new Frame(0, 0, clockWidth, clockHeight));
            hud.AddChild(clockEnt, w); 

            //add speed buttons to HUD

            float speedButtonRowWidth = hudWidth; 
            float speedButtonRowHeight = speedButtonRowWidth / 5f; 
            LinearLayoutContainer speedButtonRow = LinearLayoutWrap.Add(
                w, 
                Vector2.Zero, 
                speedButtonRowWidth, 
                speedButtonRowHeight, 
                direction: "horizontal",
                outline: false
            );

            int[] es = new int[3]; 
            for (int i = 0; i < 3; i++) {
                int buttonEnt = EntityFactory.Add(w); 
                es[i] = buttonEnt; 
                w.SetComponent<Outline>(buttonEnt, new Outline()); 
                w.SetComponent<Button>(buttonEnt, new Button()); 
                speedButtonRow.AddChild(buttonEnt, w); 
            }

            speedButtonRow.ResizeChildren(w); 
            hud.AddChild(speedButtonRow.GetParentEntity(), w); 

            w.SetComponent<SlowTimeButton>(es[0], SlowTimeButton.Get()); 
            w.SetComponent<TextBox>(es[0], new TextBox("Slow Time")); 
            w.SetComponent<PauseButton>(es[1], PauseButton.Get()); 
            w.SetComponent<TextBox>(es[1], new TextBox("Pause Time")); 
            w.SetComponent<SpeedTimeButton>(es[2], SpeedTimeButton.Get()); 
            w.SetComponent<TextBox>(es[2], new TextBox("Fast Time"));

            //add save button to HUD 

            float saveWidth = clockWidth; 
            float saveHeight = clockHeight; 
            int saveEnt = EntityFactory.AddUI(w, Vector2.Zero, saveWidth, saveHeight, setButton: true, 
                text: "Save", setOutline: true);
            w.SetComponent<SaveButton>(saveEnt, new SaveButton());
            hud.AddChild(saveEnt, w); 

            //add item summary to HUD 
            float itemSumWidth = hudWidth;
            float itemSumHeight = hudHeight / 1.6f; 
            LinearLayoutContainer llc = LinearLayoutWrap.Add(w, Vector2.Zero, itemSumWidth, 
                itemSumHeight, outline: true, usePaging: true, childrenPerPage: 4, direction: "vertical");
            
            List<Inventory> invs = w.GetMatchingEntities([typeof(Inventory), typeof(Data)]).Select(
                ent => w.GetComponent<Inventory>(ent)).ToList(); 
            
            List<string> itemStrings = ItemID.All.Select(
                s => new KeyValuePair<string, int>(s, invs.Aggregate(0, (acc, inv) => acc + inv.ItemCount(s)))
            ).Where(kvp => kvp.Value > 0).Aggregate(new List<string>(), (acc, kvp) => {
                acc.Add($"{kvp.Key}: {kvp.Value}\n");
                return acc; 
            }).ToList();

            foreach (string s in itemStrings) {
                int iEnt = EntityFactory.Add(w); 
                w.SetComponent<TextBox>(iEnt, new TextBox(s)); 

                llc.AddChild(iEnt, w);
            }

            llc.ResizeChildren(w); 
            hud.AddChild(llc.GetParentEntity(), w); 

            w.RemoveEntity(e);
        }; 

        world.AddSystem(ts, tf); 
    }
}
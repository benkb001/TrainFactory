
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
using TrainGame.Systems;
using TrainGame.Callbacks; 

public class TravelingInterfaceData : IInterfaceData {
    private Train train; 

    public Train GetTrain() => train; 

    public TravelingInterfaceData(Train train) {
        this.train = train; 
    }

    public Menu GetMenu() {
        return new Menu(train: train); 
    }

    public SceneType GetSceneType() {
        return SceneType.TravelingInterface; 
    }
}

public class DrawTravelingInterfaceSystem {
    public static void Register(World w) {

        DrawInterfaceSystem.Register<TravelingInterfaceData>(w, (w, e, data) => {
            Train t = data.GetTrain(); 

            Vector2 topleft = w.GetCameraTopLeft(); 
            Vector2 pos = topleft + new Vector2(10, 10); 
            float width = w.ScreenWidth - 20f; 
            float height = w.ScreenHeight - 20f; 

            int travelingInterfaceLayoutEnt = EntityFactory.Add(w); 
            LinearLayout travelingInterfaceLL = new LinearLayout("vertical", "alignlow");
            w.SetComponent<LinearLayout>(travelingInterfaceLayoutEnt, travelingInterfaceLL); 
            w.SetComponent<Frame>(travelingInterfaceLayoutEnt, new Frame(pos, width, height)); 
            
            int summaryEnt = EntityFactory.Add(w); 
            LinearLayoutWrap.AddChild(summaryEnt, travelingInterfaceLayoutEnt, travelingInterfaceLL, w);
            w.SetComponent<Frame>(summaryEnt, new Frame(0, 0, w.ScreenWidth - 40f, w.ScreenHeight / 4f)); 
            string s = $"{t.Id}\nArrival Time: {t.ArrivalTime}";
            w.SetComponent<TextBox>(summaryEnt, new TextBox(s)); 
            w.SetComponent<Outline>(summaryEnt, new Outline()); 
            
            int timeEnt = EntityFactory.Add(w); 
            LinearLayoutWrap.AddChild(timeEnt, travelingInterfaceLayoutEnt, travelingInterfaceLL, w); 
            w.SetComponent<Frame>(timeEnt, new Frame(0, 0, w.ScreenHeight / 4f, w.ScreenHeight / 4f)); 
            w.SetComponent<GameClockView>(timeEnt, GameClockView.Get());
            w.SetComponent<Outline>(timeEnt, new Outline()); 
            w.SetComponent<TextBox>(timeEnt, new TextBox("")); 

            DrawInventoryContainerMessage<Train> dm = new DrawInventoryContainerMessage<Train>(
                t, Vector2.Zero, w.ScreenWidth - 40f, w.ScreenHeight / 4f); 
            InventoryContainer<Train> container = DrawInventoryContainerSystem.Draw<Train>(dm, w);
            LinearLayoutWrap.AddChild(container.GetParentEntity(), travelingInterfaceLayoutEnt, travelingInterfaceLL, w); 
            w.RemoveEntity(e); 
        }); 
    }

    public static void AddMessage(World w, Train train) {
        MakeMessage.Add<DrawInterfaceMessage<TravelingInterfaceData>>(
            w, new DrawInterfaceMessage<TravelingInterfaceData>(new TravelingInterfaceData(train)));
    }
}
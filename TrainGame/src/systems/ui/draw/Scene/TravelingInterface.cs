
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
    public readonly int TrainEntity;

    public TravelingInterfaceData(Train train, int TrainEntity) {
        this.train = train; 
        this.TrainEntity = TrainEntity;
    }

    public Menu GetMenu() {
        return new Menu(train: train, TrainEntity : TrainEntity); 
    }

    public SceneType GetSceneType() {
        return SceneType.TravelingInterface; 
    }
}

public class DrawTravelingInterfaceSystem {
    public static void Register(World w) {

        DrawInterfaceSystem.Register<TravelingInterfaceData>(w, (w, e, data) => {
            
            Train t = data.GetTrain(); 
            int trainEnt = data.TrainEntity;

            Vector2 topleft = w.GetCameraTopLeft(); 
            Vector2 pos = topleft + new Vector2(10, 10); 
            float width = w.ScreenWidth - 20f; 
            float height = w.ScreenHeight - 20f; 

            LinearLayoutContainer outer = LinearLayoutContainer.AddOuter(w);
            
            int summaryEnt = EntityFactory.Add(w); 
            outer.AddChild(summaryEnt, w); 
            w.SetComponent<Frame>(summaryEnt, new Frame(0, 0, w.ScreenWidth - 40f, w.ScreenHeight / 4f)); 
            string s = $"{t.Id}\nArrival Time: {t.ArrivalTime}";
            w.SetComponent<TextBox>(summaryEnt, new TextBox(s)); 
            w.SetComponent<Outline>(summaryEnt, new Outline()); 
            
            int timeEnt = EntityFactory.Add(w); 
            outer.AddChild(timeEnt, w); 
            w.SetComponent<Frame>(timeEnt, new Frame(0, 0, w.ScreenHeight / 4f, w.ScreenHeight / 4f)); 
            w.SetComponent<GameClockView>(timeEnt, GameClockView.Get());
            w.SetComponent<Outline>(timeEnt, new Outline()); 
            w.SetComponent<TextBox>(timeEnt, new TextBox("")); 
            
            (float invWidth, float invHeight) = InventoryWrap.GetUI(t.Inv); 
            DrawInventoryContainerMessage<Train> dm = new DrawInventoryContainerMessage<Train>(
                t, Vector2.Zero, invWidth, invHeight); 
            InventoryContainer<Train> container = DrawInventoryContainerSystem.Draw<Train>(dm, w);

            LinearLayoutContainer row = LinearLayoutContainer.Add(w, Vector2.Zero, w.ScreenWidth - 40f, w.ScreenHeight / 4f);
            outer.AddChild(row.GetParentEntity(), w); 

            row.AddChild(container.GetParentEntity(), w);

            (TALBody<Train, City> exe, bool has_exe) = w.GetComponentSafe<TALBody<Train, City>>(trainEnt);
            if (has_exe) {
                int pauseProgramBtnEnt = PauseTrainProgramButtonWrap.Add(w, exe, w.ScreenHeight / 4f, w.ScreenHeight / 5f);
                row.AddChild(pauseProgramBtnEnt, w);
            }

            w.RemoveEntity(e); 
        }); 
    }

    public static void AddMessage(World w, Train train, int trainEnt) {
        MakeMessage.Add<DrawInterfaceMessage<TravelingInterfaceData>>(
            w, new DrawInterfaceMessage<TravelingInterfaceData>(new TravelingInterfaceData(train, trainEnt)));
    }
}
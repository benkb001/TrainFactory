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

public class CloseMenuMessage {}

public static class CloseMenuClickSystem {
    public static void Register(World w) {
        w.AddSystem((w) => {
            if (VirtualKeyboard.IsClicked(KeyBinds.Interact)) {
                CloseMenuSystem.AddMessage(w); 
            }
        });
    }
}

public static class CloseMenuSystem {

    private static void returnFrom(SceneType type, Menu menu, World w) {
        City city; 
        Train train; 
        Machine machine; 
        int trainEnt = menu.TrainEntity;
        //ICKY
        if (trainEnt == -1 && menu.GetTrain() != null) {
            trainEnt = ComponentID.GetEntity<Train>(menu.GetTrain().ID, w);
        }

        (TALBody<Train, City> exe, bool _) = w.GetComponentSafe<TALBody<Train, City>>(menu.TrainEntity);
        switch (type) {
            case SceneType.CartInterface: 
                MakeMessage.Add<DrawTrainInterfaceMessage>(w, new DrawTrainInterfaceMessage(menu.GetTrain(), trainEnt));
                break;
            case SceneType.Map: 
                train = TrainWrap.GetTrainWithPlayer(w); 
                if (train != null) {
                    DrawTravelingInterfaceSystem.AddMessage(w, train, trainEnt); 
                } else {
                    SceneSystem.EnterScene(w, SceneType.RPG, useOldScene: true); 
                }
                
                WorldTimeWrap.SetTimePassSlow(w); 
                break;
            case SceneType.TrainInterface: 
                train = menu.GetTrain(); 
                if (train != null) {
                    city = w.GetComponent<ComingFromCity>(trainEnt);
                    MakeMessage.Add<DrawCityInterfaceMessage>(w, new DrawCityInterfaceMessage(city)); 
                } else {
                    throw new InvalidOperationException("Found menu in train interface that did not specify train"); 
                }
                break;
            case SceneType.CityInterface: 
                MakeMessage.Add<DrawMapMessage>(w, DrawMapMessage.Get());
                break;
            case SceneType.MachineInterface: 
                machine = menu.GetMachine();
                city = menu.GetCity();
                if (!machine.PlayerAtMachine) {
                    if (city == null) {
                        throw new InvalidOperationException(@"Found a menu with no city 
                            specified when needing to return to city interface from machine interface");
                    }
                    MakeMessage.Add<DrawCityInterfaceMessage>(w, new DrawCityInterfaceMessage(city)); 
                } else {
                    SceneSystem.EnterScene(w, SceneType.RPG, useOldScene: true); 
                    machine.SetPlayerAtMachine(false); 
                }
                break;
            case SceneType.ProgramInterface: 
                MakeMessage.Add<DrawTrainInterfaceMessage>(w, new DrawTrainInterfaceMessage(menu.GetTrain(), trainEnt));
                break;
            case SceneType.ViewProgramInterface: 
                MakeMessage.Add<DrawTrainInterfaceMessage>(w, new DrawTrainInterfaceMessage(menu.GetTrain(), trainEnt));
                break;
            case SceneType.WriteProgramInterface: 
                MakeMessage.Add<DrawTrainInterfaceMessage>(w, new DrawTrainInterfaceMessage(menu.GetTrain(), trainEnt));
                break;
            case SceneType.VendorInterface: 
                SceneSystem.EnterScene(w, SceneType.RPG, useOldScene: true); 
                break;
            case SceneType.EquipmentInterface: 
                MakeMessage.Add<DrawMapMessage>(w, DrawMapMessage.Get());
                break;
            case SceneType.TravelingInterface: 
                MakeMessage.Add<DrawMapMessage>(w, DrawMapMessage.Get()); 
                break;
            case SceneType.UpgradeTrainInterface: 
                MakeMessage.Add<DrawTrainInterfaceMessage>(w, new DrawTrainInterfaceMessage(menu.GetTrain(), trainEnt));
                break;
            case SceneType.ElevatorInterface: 
                SceneSystem.EnterScene(w, SceneType.RPG, useOldScene: true); 
                break;
            default: 
                throw new InvalidOperationException("Not handled"); 
        }
    }

    public static void AddMessage(World w) {
        MakeMessage.Add<CloseMenuMessage>(w, new CloseMenuMessage()); 
    }

    public static void Register(World world) {
        world.AddSystem([typeof(CloseMenuMessage)], (w, e) => {
            if (SceneSystem.CanExitScene(w)) {
                List<int> menuEntities = SceneSystem.GetMenuEntities(w); 

                if (menuEntities.Count >= 1) {
                    int menuEnt = menuEntities[0]; 
                    Menu menu = w.GetComponent<Menu>(menuEnt); 
                    SceneType type = w.GetComponent<Scene>(menuEnt).Type; 
                    w.LockCamera(); 
                    returnFrom(type, menu, w); 
                }
            }
            w.RemoveEntity(e); 
        }); 
    }
}
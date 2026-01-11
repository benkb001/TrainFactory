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

public static class CloseMenuSystem {

    private static void returnFrom(SceneType type, Menu menu, World w) {
        City city; 
        Train train; 
        Machine machine; 
        switch (type) {
            case SceneType.CartInterface: 
                MakeMessage.Add<DrawTrainInterfaceMessage>(w, new DrawTrainInterfaceMessage(menu.GetTrain()));
                break;
            case SceneType.Map: 
                //TODO: this needs to be more dynamic, maybe we need to keep track of last scenes
                SceneSystem.EnterScene(w, SceneType.RPG, useOldScene: true); 
                WorldTimeWrap.SetTimePassSlow(w); 
                break;
            case SceneType.TrainInterface: 
                train = menu.GetTrain(); 
                if (train != null) {
                    city = train.ComingFrom; 
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
                city = machine.GetCity();
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
                MakeMessage.Add<DrawTrainInterfaceMessage>(w, new DrawTrainInterfaceMessage(menu.GetTrain()));
                break;
            case SceneType.ViewProgramInterface: 
                MakeMessage.Add<DrawTrainInterfaceMessage>(w, new DrawTrainInterfaceMessage(menu.GetTrain()));
                break;
            case SceneType.WriteProgramInterface: 
                MakeMessage.Add<DrawTrainInterfaceMessage>(w, new DrawTrainInterfaceMessage(menu.GetTrain()));
                break;
            default: 
                throw new InvalidOperationException("Not handled"); 
        }
    }

    public static void Register(World world) {
        Action<World> update = (w) => {
            if (VirtualKeyboard.IsClicked(KeyBinds.Interact) && SceneSystem.CanExitScene(w)) {
                List<int> menuEntities = w.GetMatchingEntities([typeof(Menu), typeof(Scene), typeof(Active)]); 

                if (menuEntities.Count >= 1) {
                    int e = menuEntities[0]; 
                    Menu menu = w.GetComponent<Menu>(e); 
                    SceneType type = w.GetComponent<Scene>(e).Type; 
                    returnFrom(type, menu, w); 
                }
            }
        };
        world.AddSystem(update); 
    }
}
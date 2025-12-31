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
using TrainGame.Callbacks; 


public class DrawCitySystem {
    private static float machineWidth = 50f; 
    private static float machineHeight = 50f; 
    
    private static int DrawPlayer(int maxScene, Vector2 topleft, Vector2 position, World w) {
        int playerEntity = EntityFactory.Add(w, scene: maxScene); 
        int playerInvDataEnt = InventoryWrap.GetEntity(Constants.PlayerInvID, w); 

        int playerInvEnt = EntityFactory.Add(w, scene: maxScene); 
        float playerInvHeight = 100f; 
        float playerInvWidth = w.ScreenWidth - 100f; 
        Vector2 playerInvPosition = topleft + new Vector2(50f, w.ScreenHeight - 120f); 
        Inventory playerInv = w.GetComponent<Inventory>(playerInvDataEnt); 
        DrawInventoryCallback.Draw(w, playerInv, playerInvPosition, playerInvWidth, 
            playerInvHeight, Entity: playerInvEnt, Padding: Constants.InventoryPadding, SetMenu: false, DrawLabel: false);

        w.SetComponent<Frame>(playerEntity, new Frame(position, Constants.PlayerWidth, Constants.PlayerHeight)); 
        w.SetComponent<Interactor>(playerEntity, Interactor.Get());
        w.SetComponent<CardinalMovement>(playerEntity, new CardinalMovement(Constants.PlayerSpeed)); 
        w.SetComponent<Collidable>(playerEntity, Collidable.Get()); 
        w.SetComponent<HeldItem>(playerEntity, new HeldItem(playerInv, playerInvEnt)); 
        w.SetComponent<Outline>(playerEntity, 
            new Outline(Colors.PlayerOutline, Constants.PlayerOutlineThickness, Depth.PlayerOutline)); 
        w.SetComponent<Background>(playerEntity, new Background(Colors.PlayerBackground, Depth.PlayerBackground));
        w.SetComponent<TextBox>(playerEntity, new TextBox(Constants.PlayerStr)); 
        return playerEntity; 
    }

    private static void DrawWalls(int maxScene, Vector2 cameraTopLeft, World w) {
        int[] ds = [0, 1]; 
        float margin = 100f; 

        Vector2 topLeft = cameraTopLeft + new Vector2(10, 10); 
        float wallWidth = w.ScreenWidth - 20f; 
        float wallHeight = w.ScreenHeight - 20f; 

        int wallOutlineEnt = EntityFactory.Add(w, scene: maxScene); 
        w.SetComponent<Frame>(wallOutlineEnt, new Frame(topLeft, wallWidth, wallHeight));
        w.SetComponent<Outline>(wallOutlineEnt, new Outline()); 

        foreach(int i in ds) {
            int e = EntityFactory.Add(w, scene: maxScene); 
            Vector2 wallTopLeft = topLeft - new Vector2(margin - i * (margin + wallWidth), 0);
            w.SetComponent<Frame>(e, new Frame(wallTopLeft, margin, wallHeight)); 
            w.SetComponent<Collidable>(e, Collidable.Get()); 
        }

        foreach (int i in ds) {
            int e = EntityFactory.Add(w, scene: maxScene); 
            
            Vector2 wallTopLeft = topLeft - new Vector2(0, margin - i * (margin + wallHeight));
            w.SetComponent<Frame>(e, new Frame(wallTopLeft, wallWidth, margin)); 
            w.SetComponent<Collidable>(e, Collidable.Get()); 
        }
    }

    private static int drawInteractable(int maxScene, Vector2 position, float width, float height, string s, World w) {
        int e = EntityFactory.Add(w, scene: maxScene); 
        w.SetComponent<Frame>(e, new Frame(position, width, height)); 
        w.SetComponent<Collidable>(e, Collidable.Get()); 
        w.SetComponent<TextBox>(e, new TextBox(s)); 
        w.SetComponent<Interactable>(e, new Interactable());
        w.SetComponent<Outline>(e, new Outline()); 
        return e; 
    }

    private static void DrawTrainYard(int maxScene, Vector2 position, float width, float height, World w) {
        int e = drawInteractable(maxScene, position, width, height, "Train Yard", w); 
        w.SetComponent<TrainYard>(e, TrainYard.Get()); 
    }

    private static void DrawMachine(int maxScene, Machine m, Vector2 position, float width, float height, World w) {
        int e = drawInteractable(maxScene, position, width, height, m.Id, w); 
        w.SetComponent<MachineUI>(e, new MachineUI(m)); 
    }

    private static void drawDefault(int maxScene, Vector2 topleft, Machine m, World w) {
        DrawWalls(maxScene, topleft, w); 
        DrawPlayer(maxScene, topleft, topleft + new Vector2(20, 20), w); 
        DrawTrainYard(maxScene, topleft + new Vector2(w.ScreenWidth - 130f, 20f), 100f, 100f, w);

        DrawMachine(
            maxScene, 
            m,
            topleft + new Vector2(w.ScreenWidth / 2, w.ScreenHeight / 2), 
            machineWidth, 
            machineHeight, 
            w
        ); 
    } 

    //entities should be drawn on the max scene
    private static void DrawLayout(City city, int maxScene, Vector2 topleft, World w) {
        
        w.LockCamera(); 
        
        string cityID = city.Id; 

        switch (cityID) {
            case CityID.Factory: 
                DrawWalls(maxScene, topleft, w); 
                DrawPlayer(maxScene, topleft, topleft + new Vector2(20, 20), w); 
                DrawTrainYard(maxScene, topleft + new Vector2(w.ScreenWidth - 130f, 20f), 100f, 100f, w);

                Machine[] ms = CityID.CityMap[CityID.Factory].Machines.Select(s => city.Machines[s]).ToArray(); 
                Vector2 msTopLeft = topleft + new Vector2(20, 130); 

                for (int i = 0; i < 2; i++) {
                    for (int j = 0; j < ms.Length / 2; j++) {

                        DrawMachine(
                            maxScene,
                            ms[(i * (ms.Length / 2)) + j], 
                            msTopLeft + new Vector2(120 * j, 120 * i), 
                            machineWidth, 
                            machineHeight, 
                            w
                        );
                    }
                }
                
                if ((ms.Length % 2) == 1) {
                    DrawMachine(
                        maxScene,
                        ms[ms.Length - 1], 
                        msTopLeft + new Vector2(120 * (ms.Length / 2), 120), 
                        machineWidth, 
                        machineHeight, 
                        w
                    );
                }


                break;
            case CityID.Greenhouse: 
                drawDefault(maxScene, topleft, city.Machines[MachineID.Greenhouse], w); 

                break;
            case CityID.Mine: 
                drawDefault(maxScene, topleft, city.Machines[MachineID.Drill], w); 
                
                break;
            case CityID.Coast: 
                drawDefault(maxScene, topleft, city.Machines[MachineID.Excavator], w); 
                DrawMachine(maxScene, city.Machines[MachineID.Pump], topleft + new Vector2(20, 200), 50f, 50f, w);

                break; 
            case CityID.Reservoir: 
                break;
            case CityID.Collisseum: 
                break; 
            default: 
                int e = EntityFactory.Add(w, scene: maxScene); 
                w.SetComponent<Frame>(e, new Frame(topleft, 100, 100));
                w.SetComponent<Outline>(e, new Outline()); 
                w.SetComponent<TextBox>(e, new TextBox("Test")); 
                break; 
        }
    }

    private static Type[] ts = [typeof(DrawCityMessage)];
    private static Action<World, int> tf = (w, e) => {
        City c = w.GetComponent<DrawCityMessage>(e).GetCity();
        (int maxScene, Vector2 topleft) = SceneSystem.RemoveMaxScene(w); 

        DrawLayout(c, maxScene, topleft, w);
        w.RemoveEntity(e); 
    }; 

    public static void Register(World world) {
        world.AddSystem(ts, tf); 
    }
}
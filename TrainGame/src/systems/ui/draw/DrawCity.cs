namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Callbacks; 
using TrainGame.Systems;

public class DrawCitySystem {
    private static float machineWidth = 50f; 
    private static float machineHeight = 50f; 
    
    private static int DrawPlayer(Vector2 topleft, Vector2 position, City c, World w) {
        int playerEntity = EntityFactory.Add(w); 
        int playerDataEnt = PlayerWrap.GetEntity(w); 
        int playerInvDataEnt = InventoryWrap.GetEntity(Constants.PlayerInvID, w); 
        Inventory playerInv = w.GetComponent<Inventory>(playerInvDataEnt); 

        (float playerInvWidth, float playerInvHeight) = InventoryWrap.GetUI(playerInv); 

        InventoryView playerInvView = DrawInventoryCallback.Draw(w, playerInv, Vector2.Zero, playerInvWidth, 
            playerInvHeight, Padding: Constants.InventoryPadding, SetMenu: false, DrawLabel: false);
        w.SetComponent<ScreenAnchor>(playerInvView.GetParentEntity(), 
            new ScreenAnchor(new Vector2((w.ScreenWidth - playerInvWidth) / 2f, w.ScreenHeight - playerInvHeight - 10)));
        w.SetComponent<PlayerInvFlag>(playerInvView.GetParentEntity(), new PlayerInvFlag());
        w.SetComponent<Outline>(playerInvView.GetParentEntity(), new Outline(Color.Red, 2));
        Console.WriteLine(LinearLayoutWrap.GetDepth(playerInvView.GetParentEntity(), w)); 
        Console.WriteLine(LinearLayoutWrap.GetDepth(playerInvView.GetInventoryEntity(), w)); 

        int playerInvEnt = playerInvView.GetInventoryEntity(); 
        w.SetComponent<Frame>(playerEntity, new Frame(position, Constants.PlayerWidth, Constants.PlayerHeight)); 
        w.SetComponent<Interactor>(playerEntity, Interactor.Get());
        w.SetComponent<CardinalMovement>(playerEntity, new CardinalMovement(Constants.PlayerSpeed)); 
        w.SetComponent<Collidable>(playerEntity, Collidable.Get()); 
        w.SetComponent<HeldItem>(playerEntity, new HeldItem(playerInv, playerInvEnt)); 
        w.SetComponent<Outline>(playerEntity, 
            new Outline(Colors.PlayerOutline, Constants.PlayerOutlineThickness, Depth.PlayerOutline)); 
        w.SetComponent<Background>(playerEntity, new Background(Colors.PlayerBackground, Depth.PlayerBackground));
        w.SetComponent<Player>(playerEntity, new Player()); 
        w.SetComponent<Health>(playerEntity, w.GetComponent<Health>(playerInvDataEnt));
        w.SetComponent<RespawnLocation>(playerEntity, w.GetComponent<RespawnLocation>(playerInvDataEnt));
        w.SetComponent<Inventory>(playerEntity, playerInv); 

        w.UnlockCameraPan(); 
        w.TrackEntity(playerEntity); 
        return playerEntity; 
    }

    private static void DrawWalls(Vector2 cameraTopLeft, World w) {
        int[] ds = [0, 1]; 
        float margin = 100f; 

        Vector2 topLeft = cameraTopLeft + new Vector2(10, 10); 
        float wallWidth = w.ScreenWidth - 20f; 
        float wallHeight = w.ScreenHeight - 20f; 

        int wallOutlineEnt = EntityFactory.Add(w); 
        w.SetComponent<Frame>(wallOutlineEnt, new Frame(topLeft, wallWidth, wallHeight));
        w.SetComponent<Outline>(wallOutlineEnt, new Outline()); 

        foreach(int i in ds) {
            int e = EntityFactory.Add(w); 
            Vector2 wallTopLeft = topLeft - new Vector2(margin - i * (margin + wallWidth), 0);
            w.SetComponent<Frame>(e, new Frame(wallTopLeft, margin, wallHeight)); 
            w.SetComponent<Collidable>(e, Collidable.Get()); 
        }

        foreach (int i in ds) {
            int e = EntityFactory.Add(w); 
            
            Vector2 wallTopLeft = topLeft - new Vector2(0, margin - i * (margin + wallHeight));
            w.SetComponent<Frame>(e, new Frame(wallTopLeft, wallWidth, margin)); 
            w.SetComponent<Collidable>(e, Collidable.Get()); 
        }
    }

    private static int drawInteractable(Vector2 position, float width, float height, string s, World w) {
        int e = EntityFactory.Add(w); 
        w.SetComponent<Frame>(e, new Frame(position, width, height)); 
        w.SetComponent<Collidable>(e, Collidable.Get()); 
        w.SetComponent<TextBox>(e, new TextBox(s)); 
        w.SetComponent<Interactable>(e, new Interactable());
        w.SetComponent<Outline>(e, new Outline()); 
        return e; 
    }

    private static void DrawTrainYard(Vector2 position, float width, float height, World w) {
        int e = drawInteractable(position, width, height, "Train Yard", w); 
        w.SetComponent<TrainYard>(e, TrainYard.Get()); 
    }

    private static void DrawMachine(Machine m, Vector2 position, float width, float height, World w) {
        int e = drawInteractable(position, width, height, m.Id, w); 
        w.SetComponent<MachineUI>(e, new MachineUI(m)); 
    }

    private static void drawDefault(Vector2 topleft, Machine m, City c, World w) {
        DrawWalls(topleft, w); 
        DrawPlayer(topleft, topleft + new Vector2(20, 20), c, w); 
        DrawTrainYard(topleft + new Vector2(w.ScreenWidth - 130f, 20f), 100f, 100f, w);

        DrawMachine(
            m,
            topleft + new Vector2(w.ScreenWidth / 2, w.ScreenHeight / 2), 
            machineWidth, 
            machineHeight, 
            w
        ); 
    } 

    //entities should be drawn on the max scene
    private static void DrawLayout(City city, Vector2 topleft, World w) {
        
        w.LockCamera(); 
        
        string cityID = city.Id; 

        switch (cityID) {
            case CityID.Factory: 
                DrawWalls(topleft, w); 
                DrawPlayer(topleft, topleft + new Vector2(20, 20), city, w); 
                DrawTrainYard(topleft + new Vector2(w.ScreenWidth - 130f, 20f), 100f, 100f, w);

                //start test

                for (int i = 0; i < 2; i++) {
                    int enemyEnt = EntityFactory.AddUI(w, topleft + new Vector2((i + 1) * 200, 200), 
                        50, 50, setOutline: true); 
                    w.SetComponent<Health>(enemyEnt, new Health(10)); 
                    w.SetComponent<Enemy>(enemyEnt, new Enemy()); 
                    w.SetComponent<Loot>(enemyEnt, new Loot(ItemID.TimeCrystal, 1, InventoryWrap.GetPlayerInv(w)));
                    w.SetComponent<Shooter>(enemyEnt, new Shooter());
                    w.SetComponent<Movement>(enemyEnt, new Movement()); 
                    w.SetComponent<Collidable>(enemyEnt, new Collidable()); 
                }


                //end test
                Machine[] ms = CityID.CityMap[CityID.Factory].Machines.Select(s => city.Machines[s]).ToArray(); 
                Vector2 msTopLeft = topleft + new Vector2(20, 130); 

                for (int i = 0; i < 2; i++) {
                    for (int j = 0; j < ms.Length / 2; j++) {

                        DrawMachine(
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
                        ms[ms.Length - 1], 
                        msTopLeft + new Vector2(120 * (ms.Length / 2), 120), 
                        machineWidth, 
                        machineHeight, 
                        w
                    );
                }


                break;
            case CityID.Greenhouse: 
                drawDefault(topleft, city.Machines[MachineID.Greenhouse], city, w); 

                break;
            case CityID.Mine: 
                drawDefault(topleft, city.Machines[MachineID.Drill], city, w); 
                
                break;
            case CityID.Coast: 
                drawDefault(topleft, city.Machines[MachineID.Excavator], city, w); 
                DrawMachine(city.Machines[MachineID.Pump], topleft + new Vector2(20, 200), 50f, 50f, w);

                break; 
            case CityID.Reservoir: 
                break;
            case CityID.Collisseum: 
                break; 
            default: 
                int e = EntityFactory.Add(w); 
                w.SetComponent<Frame>(e, new Frame(topleft, 100, 100));
                w.SetComponent<Outline>(e, new Outline()); 
                w.SetComponent<TextBox>(e, new TextBox("Test")); 
                break; 
        }
    }

    private static Type[] ts = [typeof(DrawCityMessage)];
    private static Action<World, int> tf = (w, e) => {
        foreach (int ent in w.GetMatchingEntities([typeof(City), typeof(Data)])) {
            w.GetComponent<City>(ent).HasPlayer = false; 
        }

        City c = w.GetComponent<DrawCityMessage>(e).GetCity();
        c.HasPlayer = true; 
        SceneSystem.EnterScene(w, SceneType.RPG); 
        Vector2 topleft = w.GetCameraTopLeft(); 

        DrawLayout(c, topleft, w);
        w.RemoveEntity(e); 
    }; 

    public static void Register(World world) {
        world.AddSystem(ts, tf); 
    }
}
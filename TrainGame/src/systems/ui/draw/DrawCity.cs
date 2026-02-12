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

public static class DrawHPSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(Health), typeof(TextBox), typeof(Active)], (w, e) => {
            Health h = w.GetComponent<Health>(e); 
            TextBox tb = w.GetComponent<TextBox>(e); 
            
            tb.Text = $"HP: {h.HP}";
        });
    }
}

public static class DrawCitySystem {
    private static float machineWidth = 50f; 
    private static float machineHeight = 50f; 
    
    private static int DrawPlayer(Vector2 topleft, Vector2 position, City c, World w) {
        return PlayerWrap.Draw(position, w);
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

        if (m != null) {
            DrawMachine(
                m,
                topleft + new Vector2(w.ScreenWidth / 2, w.ScreenHeight / 2), 
                machineWidth, 
                machineHeight, 
                w
            );
        }
 
    } 

    private static int drawVendor(World w, Vector2 pos, City city, string vendorID) {
        int vendorEnt = EntityFactory.AddUI(w, pos, 50, 50, 
        setOutline: true, setInteractable: true, setCollidable: true, text: vendorID);
        EnterInterfaceInteractable<VendorInterfaceData> interactable = 
            new EnterInterfaceInteractable<VendorInterfaceData>(
                new VendorInterfaceData(city, vendorID));
        w.SetComponent<EnterInterfaceInteractable<VendorInterfaceData>>(vendorEnt, interactable); 
        return vendorEnt; 
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
            case CityID.HauntedPowerPlant: 
                Layout.Draw(w, Layout.L1);
                /*
                w.SetCameraBounds(topleft.Y - 300f, topleft.X + 100f, topleft.Y - 150f, topleft.X - 100f); 
                DrawPlayer(topleft, topleft - new Vector2(50, 50), city, w); 
                DrawTrainYard(topleft - new Vector2(50, 50), 50, 50, w); 

                void drawWall(Vector2 pos, float width, float height, World w) {
                    int e = EntityFactory.AddUI(w, pos, width, height, setOutline: true); 
                    w.SetComponent<Collidable>(e, new Collidable()); 
                }

                float roomWidth = 850f; 
                float roomHeight = 500f; 

                float wallWidth = 50f; 

                Vector2 leftWallPos = topleft + new Vector2(-450, -450);
                drawWall(topleft + new Vector2(-400, 50), roomWidth, wallWidth, w);
                drawWall(leftWallPos, wallWidth, 500f, w); 
                drawWall(topleft + new Vector2(-400, -500), roomWidth, wallWidth, w); 
                drawWall(topleft + new Vector2(450, -450), wallWidth, roomHeight, w);

                float spawnWidth = roomWidth - 100f; 
                float spawnHeight = 150f; 

                int enemySpawnEnt = EntityFactory.AddUI(w, leftWallPos + new Vector2(60, 10), 
                    spawnWidth, spawnHeight, setOutline: true); 
                w.SetComponent<EnemySpawner>(enemySpawnEnt, new EnemySpawner()); 

                drawWall(leftWallPos + new Vector2(50f, 225f), 300f, 50f, w);
                */
                break;
            case CityID.Armory: 
                drawDefault(topleft, null, city, w); 
                drawVendor(w, topleft + new Vector2(50, 150), city, VendorID.ArmorCraftsman); 
                drawVendor(w, topleft + new Vector2(110, 150), city, VendorID.WeaponCraftsman); 
                break;
            case CityID.Reservoir: 
                //TODO: Write
                break;
            case CityID.Refinery: 
                //TODO: Write
                break; 
            case CityID.TrainYard: 
                //todo: Write
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
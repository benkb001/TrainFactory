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

public enum TileType {
    Enemy,
    Ground,
    Ladder,
    LadderDown,
    Player,
    Spawner,
    TrainYard,
    Vendor,
    Wall
}

public class Tile {
    private TileType type; 
    private EnemyType enemyType; 
    private string id; 
    public string ID => id; 

    public TileType Type => type; 
    public EnemyType EType => enemyType;

    public Tile(TileType type, EnemyType enemyType = EnemyType.Default, string id = "") {
        this.type = type; 
        this.enemyType = enemyType; 
        this.id = id; 
    }
}

public static class Layout {
    private static Tile w = new Tile(TileType.Wall);
    private static Tile g = new Tile(TileType.Ground);
    private static Tile p = new Tile(TileType.Player); 

    private static Tile sp = new Tile(TileType.Spawner);
    private static Tile ld = new Tile(TileType.Ladder);
    private static Tile ldDown = new Tile(TileType.LadderDown);
    private static Tile trainYard = new Tile(TileType.TrainYard); 
    private static Tile hppVendor = new Tile(TileType.Vendor, id: VendorID.HPPVendor);

    private static Tile artillery = new Tile(TileType.Enemy, EnemyType.Artillery);
    private static Tile dE = new Tile(TileType.Enemy); 
    private static Tile ninja = new Tile(TileType.Enemy, EnemyType.Ninja);
    private static Tile robot = new Tile(TileType.Enemy, EnemyType.Robot);
    private static Tile shotgun = new Tile(TileType.Enemy, EnemyType.Shotgun);

    public static List<List<Tile>> L0 = new() {
        new() {w, w, w, w, w, w, w, w, w, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, g, p, g, g, g, g, g, ld, w},
        new() {w, g, g, g, w, w, g, g, g, w},
        new() {w, g, g, g, w, w, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, sp, dE, g, g, g, g, dE, g, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, w, w, w, w, w, w, w, w, w},
    };

    public static List<List<Tile>> L1 = new() {
        new() {g, g, g, w, w, w, w, w, w, w, w, w, g, g, g},
        new() {g, g, g, w, g, g, g, g, g, g, ninja, w, g, g, g},
        new() {w, w, w, w, g, g, g, g, g, g, g, w, w, w, w},
        new() {w, p, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, ld, g, g, g, g, g, g, g, g, g, g, ninja, g, w},
        new() {w, w, w, w, g, g, g, g, g, g, g, w, w, w, w},
        new() {g, g, g, w, sp, g, g, g, g, g, g, w, g, g, g},
        new() {g, g, g, w, w, w, w, w, w, w, w, w},
    };

    public static List<List<Tile>> L2 = new() {
        new() {w, w, w, w, w, w, w, w, w, w, w, w, w, w},
        new() {w, g, g, g, g, g, g, p, g, g, g, g, ld, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, shotgun, g, g, g, w, w, w, g, g, g, g, shotgun, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, sp, g, g, g, g, g, g, g, w},
        new() {w, w, w, w, w, w, w, w, w, w, w, w, w, w},
    };

        public static List<List<Tile>> L3 = new() {
        new() {w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, ld, w},
        new() {w, g, g, g, g, g, g, g, g, g, p, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, w, w, g, g, sp, g, g, g, g, g, w, w, g, g, g, w},
        new() {w, g, g, g, w, w, g, g, g, g, g, g, g, g, w, w, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, artillery, g, g, g, g, g, g, g, g, g, g, g, g, g, artillery, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w},
    };

    public static List<List<Tile>> L4 = new() {
        new() {w, w, w, w, w, w, w, w, w},
        new() {w, robot, g, g, g, g, g, g, w},
        new() {w, g, w, g, w, g, w, g, w},
        new() {w, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, ld, w},
        new() {w, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, p, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, w},
        new() {w, sp, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, w},
        new() {w, w, g, w, g, w, g, w, w},
        new() {w, g, g, g, g, g, g, robot, w},
        new() {w, w, w, w, w, w, w, w, w},
    };

    public static List<List<Tile>> HauntedPowerPlant = new() {
        new() {w, w, w, w, w, w, w},
        new() {w, g, g, g, g, g, w},
        new() {w, g, trainYard, g, ldDown, g, w},
        new() {w, g, g, g, g, g, w},
        new() {w, g, p, g, hppVendor, g, w},
        new() {w, g, g, g, g, g, w},
        new() {w, w, w, w, w, w, w},
    };

    public static Dictionary<string, List<List<Tile>>> Cities = new() {
        [CityID.HauntedPowerPlant] = HauntedPowerPlant
    };

    public static List<List<List<List<Tile>>>> Levels = new() {
        new() { L0, L1, L2},
        new() { L3, L4}
    };

    public static void Draw(World w, List<List<Tile>> tss) {
        SceneSystem.EnterScene(w, SceneType.RPG);
        Vector2 topleft = w.GetCameraTopLeft(); 

        float tileSize = 50f; 
        int spawnerEnt = EntityFactory.Add(w); 
        EnemySpawner spawner = new EnemySpawner(); 
        w.SetComponent<EnemySpawner>(spawnerEnt, spawner);

        for (int i = 0; i < tss.Count; i++) {
            List<Tile> ts = tss[i]; 
            for (int j = 0; j < ts.Count; j++) {
                Vector2 tilePos = topleft + new Vector2(tileSize * j, tileSize * i);
                int e = EntityFactory.AddUI(w, tilePos, tileSize, tileSize);
                Tile t = ts[j]; 
                switch (t.Type) {
                    case TileType.Wall: 
                        w.SetComponent<Collidable>(e, new Collidable()); 
                        w.SetComponent<Outline>(e, new Outline()); 
                        break;
                    case TileType.Ground: 
                        break;
                    case TileType.Player: 
                        PlayerWrap.Draw(tilePos, w);
                        break; 
                    case TileType.Enemy: 
                        EnemyWrap ew = EnemyWrap.Draw(w, tilePos, t.EType);
                        spawner.Spawn(ew);
                        break; 
                    case TileType.Spawner: 
                        w.SetComponent<Frame>(spawnerEnt, new Frame(tilePos, tileSize, tileSize));
                        break;
                    case TileType.Ladder: 
                        LadderWrap.AddMessage(w, tilePos, 0);
                        break;
                    case TileType.LadderDown: 
                        LadderWrap.AddMessage(w, tilePos, 1);
                        break;
                    case TileType.Vendor: 
                        VendorWrap.Draw(w, tilePos, CityWrap.GetCityWithPlayer(w), t.ID);
                        break;
                    case TileType.TrainYard: 
                        TrainYardWrap.Draw(w, tilePos);
                        break;
                    default: 
                        throw new InvalidOperationException("Unhandled tile type in draw layout");
                }
            }
        }
    }

    public static void DrawRandom(World w, int floor) {
        int difficulty = Constants.FloorDifficulty(floor);
        List<List<List<Tile>>> options = Levels[difficulty];
        Draw(w, options[w.NextInt(options.Count)]);
    }
}
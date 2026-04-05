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
    Elevator,
    Ground,
    Ladder,
    LadderDown,
    Player,
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

    private static Tile ld = new Tile(TileType.Ladder);
    private static Tile ldDown = new Tile(TileType.LadderDown);
    private static Tile trainYard = new Tile(TileType.TrainYard); 

    private static Tile elevator = new Tile(TileType.Elevator); 

    private static Dictionary<string, Tile> v = VendorID.All
    .Select(s => new KeyValuePair<string, Tile>(s, new Tile(TileType.Vendor, id: s)))
    .ToDictionary();

    //Cities

    public static List<List<Tile>> Factory = new() {
        new() {w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, v[VendorID.ArmorCraftsman], g, v[VendorID.WeaponCraftsman], g, 
                     v[VendorID.MineralCollector], g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, p, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, ldDown, g, g, g, g, elevator, g, g, g, g, g, g, g, g, trainYard, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w},
    };

    private static List<List<Tile>> getCombat(){
        int width = 30; 
        List<List<Tile>> res = new(); 
        for (int i = 0; i < width; i++) {
            res.Add(new List<Tile>());
            for(int j = 0; j < width; j++) {
                res[i].Add(g);
            }
        }

        for (int i = 0; i < width; i++) {
            res[i][0] = w;
            res[i][width - 1] = w; 
            res[0][i] = w; 
            res[width - 1][i] = w; 
        }

        res[width/2][width/2] = p;

        return res; 
    }

    public static List<List<Tile>> Combat = getCombat();

    public static Dictionary<string, List<List<Tile>>> Cities = new() {
        [CityID.Factory] = Factory
    };

    public static void Draw(World w, List<List<Tile>> tss) {
        SceneSystem.EnterScene(w, SceneType.RPG);
        Vector2 topleft = w.GetCameraTopLeft(); 

        float tileSize = Constants.TileWidth; 

        //tile at (0, 0) is drawn at topleft

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
                    case TileType.Ladder: 
                        LadderWrap.AddMessage(w, tilePos, 0);
                        break;
                    case TileType.LadderDown: 
                        LadderWrap.AddMessage(w, tilePos, 1);
                        break;
                    case TileType.Vendor: 
                        VendorWrap.Draw(w, tilePos, t.ID);
                        break;
                    case TileType.TrainYard: 
                        TrainYardWrap.Draw(w, tilePos);
                        break;
                    case TileType.Elevator: 
                        w.SetComponent<EnterInterfaceInteractable<ElevatorInterfaceData>>(e, 
                            new EnterInterfaceInteractable<ElevatorInterfaceData>(new ElevatorInterfaceData()));
                        w.SetComponent<Collidable>(e, new Collidable()); 
                        w.SetComponent<Outline>(e, new Outline()); 
                        w.SetComponent<Interactable>(e, new Interactable()); 
                        w.SetComponent<TextBox>(e, new TextBox("Elevator")); 
                        break;
                    default: 
                        throw new InvalidOperationException("Unhandled tile type in draw layout");
                }
            }
        }
    }

    public static void DrawCombat(World w) {
        Draw(w, Combat);
        Vector2 topleft = w.GetCameraTopLeft();
        float width = (Combat.Count - 2) * Constants.TileWidth; 
        w.SetCameraBounds(
            topleft.Y + (2 * Constants.TileWidth), 
            topleft.X + width, 
            topleft.Y + width, 
            topleft.X + (2 * Constants.TileWidth)
        );
    }
}
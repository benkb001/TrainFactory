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

    private static Tile artillery = new Tile(TileType.Enemy, EnemyType.Artillery);
    private static Tile dE = new Tile(TileType.Enemy); 
    private static Tile ninja = new Tile(TileType.Enemy, EnemyType.Ninja);
    private static Tile robot = new Tile(TileType.Enemy, EnemyType.Robot);
    private static Tile shotgun = new Tile(TileType.Enemy, EnemyType.Shotgun);
    private static Tile barb = new Tile(TileType.Enemy, EnemyType.Barbarian); 
    private static Tile mach = new Tile(TileType.Enemy, EnemyType.MachineGun); 
    private static Tile voll = new Tile(TileType.Enemy, EnemyType.Volley);
    private static Tile snip = new Tile(TileType.Enemy, EnemyType.Sniper); 
    private static Tile warr = new Tile(TileType.Enemy, EnemyType.Warrior); 
    private static Tile wizz = new Tile(TileType.Enemy, EnemyType.Wizard);

    private static Tile elevator = new Tile(TileType.Elevator); 

    private static Dictionary<string, Tile> v = VendorID.All
    .Select(s => new KeyValuePair<string, Tile>(s, new Tile(TileType.Vendor, id: s)))
    .ToDictionary();

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

    public static List<List<Tile>> LB = new() {
        new() {w, w, w, w, w, w, w, w, w, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, g, p, g, g, g, g, g, ld, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, sp, barb, g, g, g, g, g, g, w},
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

    public static List<List<Tile>> L5 = new() {
        new() {w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, ld, w},
        new() {w, g, g, robot, g, g, g, g, g, g, p, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, sp, g, g, g, g, w},
        new() {w, g, g, g, w, w, g, g, g, g, g, g, g, g, w, w, g, g, g, w},
        new() {w, g, g, g, w, w, g, g, g, g, g, g, g, g, w, w, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, mach, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, dE, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, robot, g, g, g, g, g, g, g, g, g, dE, g, mach, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w},
    };

    public static List<List<Tile>> LM = new() {
        new() {w, w, w, w, w, w, w, w, w},
        new() {w, g, g, g, g, g, g, p, w},
        new() {w, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, ld, w},
        new() {w, g, g, w, w, w, w, w, w},
        new() {w, g, g, w, w, w, w, g, w},
        new() {w, g, g, w, w, w, w, w, w},
        new() {w, g, g, w, w, w, w, w, w},
        new() {w, sp, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, w},
        new() {w, mach, g, g, g, g, g, mach, w},
        new() {w, w, w, w, w, w, w, w, w},
    };

    public static List<List<Tile>> L6 = new() {
        new() {w, w, w, w, w, w, w, w, w, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, g, p, g, w, w, g, mach, ld, w},
        new() {w, g, g, g, w, w, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, sp, barb, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, w, w, w, w, w, w, w, w, w},
    };

    public static List<List<Tile>> LV = new() {
        new() {w, w, w, w, w, w, w, w, w, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, g, p, g, g, g, g, g, ld, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, sp, voll, g, g, g, g, voll, g, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, w, w, w, w, w, w, w, w, w},
    };

    public static List<List<Tile>> LS = new() {
        new() {w, w, w, w, w, w, w, w, w, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, g, p, g, g, g, g, g, ld, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, sp, snip, g, g, g, g, snip, g, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, w, w, w, w, w, w, w, w, w},
    };

    public static List<List<Tile>> LW = new() {
        new() {w, w, w, w, w, w, w, w, w, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, g, p, g, g, g, g, g, ld, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, sp, warr, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, w},
        new() {w, w, w, w, w, w, w, w, w, w},
    };

    public static List<List<Tile>> L7 = new() {
        new() {w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w},
        new() {w, w, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, ld, w},
        new() {w, w, w, g, g, g, g, g, g, g, p, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, sp, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, w, w, w, w, w, w, w, w, w, w, w, w, w, w, g, g, w},
        new() {w, g, g, w, w, w, w, w, w, w, w, w, w, w, w, w, w, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, w, w, g, voll, g, g, snip, g, g, snip, g, g, g, g, voll, g, w, w, w},
        new() {w, w, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w, w, w},
        new() {w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w},
    };

    public static List<List<Tile>> L8 = new() {
        new() {w, w, w, w, w, w, w, w, w},
        new() {w, g, g, g, g, g, g, g, w},
        new() {w, g, p, g, g, warr, g, g, w},
        new() {w, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, ld, w},
        new() {w, g, g, w, w, w, g, g, w},
        new() {w, g, g, w, w, w, g, g, w},
        new() {w, g, g, w, w, w, g, g, w},
        new() {w, g, g, w, w, w, g, g, w},
        new() {w, sp, g, g, g, g, g, g, w},
        new() {w, g, g, warr, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, w},
        new() {w, w, w, w, w, w, w, w, w},
    };

    public static List<List<Tile>> L9 = new() {
        new() {g, g, g, g, g, g, g, g, g, g, g, g, g, g, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w},
        new() {g, g, g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w, g, g},
        new() {g, g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, barb, g, snip, g, g, g, w, g, g, g},
        new() {g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g},
        new() {g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, barb, g, w, g, g, g, g, g},
        new() {g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g},
        new() {g, g, g, g, g, g, g, g, w, g, g, voll, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g},
        new() {g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g},
        new() {g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g},
        new() {g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g},
        new() {g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g},
        new() {g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g},
        new() {g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g},
        new() {g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, voll, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g},
        new() {w, g, g, g, g, g, g, g, p, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g},
        new() {w, g, g, g, g, g, g, sp, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g},
        new() {w, g, shotgun, g, g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g},
        new() {w, g, snip, g, shotgun, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g},
        new() {w, w, w, w, w, w, w, w, w, w, w, w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g},
    };

    public static List<List<Tile>> LWizz = new() {
        new() {w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, p, g, g, g, g, g, ld, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, sp, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, wizz, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, snip, g, g, g, g, g, g, g, mach, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w}
    };

    //Cities

    public static List<List<Tile>> Factory = new() {
        new() {w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, v[VendorID.ArmorCraftsman], g, v[VendorID.WeaponCraftsman], g, 
                     v[VendorID.HPPVendor], g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, p, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, g, ldDown, g, g, g, g, elevator, g, g, g, g, g, g, g, g, trainYard, g, w},
        new() {w, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, g, w},
        new() {w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w, w},
    };

    public static Dictionary<string, List<List<Tile>>> Cities = new() {
        [CityID.Factory] = Factory
    };

    //need 13 sets of floors 
    public static List<List<List<List<Tile>>>> Levels = new() {
        new() { L0, L1, L2 },
        new() { L3, L4, LB },
        new() { L5, L6, LM },
        new() { LV, LS, LW },
        new() { L7, L8, L9 },
        new() { LWizz }
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

    public static void DrawRandom(World w, int floor) {
        int difficulty = Constants.FloorDifficulty(floor);
        List<List<List<Tile>>> options = difficulty >= Levels.Count ? Levels[Levels.Count - 1] : Levels[difficulty];
        Draw(w, options[w.NextInt(options.Count)]);
    }
}
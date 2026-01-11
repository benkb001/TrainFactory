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
using TrainGame.Constants;
using System.ComponentModel.DataAnnotations;

public enum SceneType {
    CartInterface,
    CityInterface,
    MachineInterface,
    Map,
    OffScreen,
    ProgramInterface,
    RPG,
    TrainInterface,
    TravelingInterface,
    ViewProgramInterface,
    WriteProgramInterface,
    None
}

public class EnterSceneMessage {
    private SceneType type; 
    public SceneType Type => type; 

    public EnterSceneMessage(SceneType type) {
        this.type = type; 
    }
}

public static class SceneSystem {
    private static SceneType currentScene = SceneType.None; 
    public static SceneType CurrentScene => currentScene; 

    public static Dictionary<SceneType, Vector2> CameraPositions = new() {
        [SceneType.OffScreen] = new Vector2(-2000, -2000),
        [SceneType.CartInterface] = new Vector2(1000, 1000),
        [SceneType.CityInterface] = new Vector2(2000, 2000),
        [SceneType.MachineInterface] = new Vector2(3000, 3000),
        [SceneType.Map] = new Vector2(4000, 4000),
        [SceneType.ProgramInterface] = new Vector2(5000, 5000),
        [SceneType.RPG] = new Vector2(6000, 6000),
        [SceneType.TrainInterface] = new Vector2(7000, 7000),
        [SceneType.WriteProgramInterface] = new Vector2(8000, 8000),
        [SceneType.ViewProgramInterface] = new Vector2(9000, 9000)
    };

    public static void EnterScene(World w, SceneType type, bool useOldScene = false) {
        
        currentScene = type; 
        
        foreach (int e in w.GetMatchingEntities([typeof(Scene)]).Where(
            ent => w.GetComponent<Scene>(ent).Type == currentScene)) {
                if (useOldScene) {
                    w.SetComponent<Active>(e, Active.Get());
                } else {
                    w.RemoveEntity(e); 
                }
            }
        foreach (int e in w.GetMatchingEntities([typeof(Scene)]).Where(
            ent => w.GetComponent<Scene>(ent).Type != currentScene)) {
                w.RemoveComponent<Active>(e); 
            }

        w.SetCameraPosition(CameraPositions[type]);
    }
}
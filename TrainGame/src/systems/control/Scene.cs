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
    ProgramInterface,
    RPG,
    TrainInterface,
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
        [SceneType.CartInterface] = new Vector2(1000, 1000),
        [SceneType.CityInterface] = new Vector2(2000, 2000),
        [SceneType.MachineInterface] = new Vector2(3000, 3000),
        [SceneType.Map] = new Vector2(4000, 4000),
        [SceneType.ProgramInterface] = new Vector2(5000, 5000),
        [SceneType.RPG] = new Vector2(6000, 6000),
        [SceneType.TrainInterface] = new Vector2(7000, 7000)
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

    public static void RegisterPush(World world) {
        Type[] ts = [typeof(PushSceneMessage)]; 
        Action<World> update = (w) => {
            
            foreach (KeyValuePair<int, PushSceneMessage> messageEntry in w.GetComponentArray<PushSceneMessage>()) {
                foreach (KeyValuePair<int, Scene> sceneEntry in w.GetComponentArray<Scene>()) {
                    Scene s = sceneEntry.Value; 
                    int entity = sceneEntry.Key; 
                    s.Value++; 
                    if (s.Value > 0) {
                        w.RemoveComponent<Active>(entity); 
                    }
                }
                w.RemoveEntity(messageEntry.Key); 
            }
            
        };
        world.AddSystem(ts, update); 
    }

    //todo: clean
    public static void RegisterPop(World world) {
        Type[] ts = [typeof(PopSceneMessage)]; 
        Action<World> update = (w) => {
            
            foreach (KeyValuePair<int, PopSceneMessage> messageEntry in w.GetComponentArray<PopSceneMessage>()) {
                foreach (KeyValuePair<int, Scene> sceneEntry in w.GetComponentArray<Scene>()) {
                    Scene s = sceneEntry.Value; 
                    int entity = sceneEntry.Key; 
                    if (s.Value == 0) {
                        w.RemoveEntity(entity); 
                    } else {
                        s.Value--; 

                        if (s.Value == 0) {
                            w.SetComponent<Active>(entity, Active.Get());
                        }
                    }
                }
                w.RemoveEntity(messageEntry.Key); 
            }
        };
        
        world.AddSystem(ts, update); 
    }

    public static void RegisterPopLate(World world) {
        Type[] ts = [typeof(PopLateMessage)]; 
        Action<World> update = (w) => {
            foreach (KeyValuePair<int, PopLateMessage> messageEntry in w.GetComponentArray<PopLateMessage>()) {
                
                if (messageEntry.Value.FrameDelay <= 0) {
                    foreach (KeyValuePair<int, Scene> sceneEntry in w.GetComponentArray<Scene>()) {
                        Scene s = sceneEntry.Value; 
                        int entity = sceneEntry.Key; 
                        if (s.Value == messageEntry.Value.Scene) {
                            w.RemoveEntity(entity); 
                        } else {
                            if (s.Value > messageEntry.Value.Scene) {
                                s.Value--; 
                            }

                            if (s.Value == 0) {
                                w.SetComponent<Active>(entity, Active.Get());
                            }
                        }
                    }
                    w.RemoveEntity(messageEntry.Key); 
                }
                messageEntry.Value.FrameDelay--; 

            }
        }; 
        world.AddSystem(update); 
    }

    public static int GetMaxScene(World w) {
        return w.GetComponentArray<Scene>().Max(kvp => kvp.Value.Value); 
    }

    public static (int, Vector2) RemoveMaxScene(World w) {
        Dictionary<int, Scene> scenes = w.GetComponentArray<Scene>(); 
        int maxScene = 0; 
        if (scenes.Count > 0) {
            maxScene = w.GetComponentArray<Scene>().Max(kvp => kvp.Value.Value); 
        }
        
        Vector2 topleft = Vector2.Zero; 
        KeyValuePair<int, CameraReturn> cr = w.GetComponentArray<CameraReturn>().FirstOrDefault(kvp => {
            return w.GetComponent<Scene>(kvp.Key).Value == maxScene;
        }, new KeyValuePair<int, CameraReturn>(0, null)); 
        if (cr.Value != null) {
            topleft = cr.Value.Position - new Vector2(w.ScreenWidth / 2, w.ScreenHeight / 2); 
        }

        //remove all entities on the max scene except the camera return position
        foreach (int cur in w.GetComponentArray<Scene>().Where(kvp => {
            return kvp.Value.Value == maxScene && !w.ComponentContainsEntity<CameraReturn>(kvp.Key);
        }).Select(kvp => kvp.Key)) {
            w.RemoveEntity(cur); 
        }

        return (maxScene, topleft); 
    }
}
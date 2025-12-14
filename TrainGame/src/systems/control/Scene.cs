namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants;
using System.ComponentModel.DataAnnotations;

public static class SceneSystem {

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
}
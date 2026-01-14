namespace TrainGame.Systems; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public class Teleporter {
    private WorldTime canTP;
    private WorldTime cooldown;
    private float maxDistance; 
    public float MaxDistance => maxDistance; 

    public Teleporter() {
        canTP = new WorldTime(); 
        cooldown = new WorldTime(minutes: 4); 
        maxDistance = 100f; 
    }

    public bool CanTP(WorldTime now) {
        return now.IsAfterOrAt(canTP); 
    }

    public void TP(WorldTime now) {
        canTP = now + cooldown; 
    }
}

public static class TeleportSystem {
    public static void Register(World w) {
        w.AddSystem((w) => {
            if (VirtualMouse.RightPushed()) {
                List<int> collidableEnts = w.GetMatchingEntities([typeof(Frame), typeof(Collidable), typeof(Active)]);
                List<int> tpEnts = w.GetMatchingEntities([typeof(Player), typeof(Teleporter), typeof(Frame), typeof(Active)]);
                Vector2 mousePosition = w.GetWorldMouseCoordinates(); 

                foreach (int e in tpEnts) {
                    Frame tpFrame = w.GetComponent<Frame>(e); 
                    Teleporter tp = w.GetComponent<Teleporter>(e); 

                    if (tp.CanTP(w.Time)) {
                        float maxDistance = tp.MaxDistance; 
                        Vector2 delta = mousePosition - tpFrame.Position; 
                        float magnitude = Vector2.Distance(delta, Vector2.Zero); 
                        if (magnitude > maxDistance) {
                            delta = Vector2.Normalize(delta) * maxDistance; 
                        }

                        float dX = delta.X; 
                        float dY = delta.Y; 

                        bool collision = false; 
                        int i = 0; 

                        while (!collision && i < collidableEnts.Count) {
                            int cEnt = collidableEnts[i];

                            if (cEnt == e) {
                                i++;
                                continue; 
                            }

                            Frame cFrame = w.GetComponent<Frame>(cEnt); 

                            if (tpFrame.IntersectsWith(cFrame, dX, dY)) {
                                collision = true; 
                            }

                            i++; 
                        }

                        if (!collision) {
                            Vector2 targetPosition = tpFrame.Position + delta; 
                            tpFrame.SetCoordinates(targetPosition); 
                            tp.TP(w.Time); 
                        }
                    }
                }
            }
        });
    }
}
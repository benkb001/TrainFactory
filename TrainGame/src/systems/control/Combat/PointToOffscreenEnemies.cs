namespace TrainGame.Systems; 
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TrainGame.Components;
using TrainGame.Utils;
using TrainGame.ECS;
using TrainGame.Constants;

public class TrackingOffScreen {
    public int Entity; 

    public TrackingOffScreen(int e) {
        this.Entity = e; 
    }
}

public class PointToOffscreenEnemiesSystem {
    public static void RegisterDraw(World w) {
        w.AddSystem((w) => {
            if (w.Time.Ticks == 0) {
                Frame cameraFrame = new Frame(w.GetCameraTopLeft(), w.ScreenWidth, w.ScreenHeight);
                List<int> trackingEnts = w.GetMatchingEntities([typeof(TrackingOffScreen), typeof(Active)]);

                foreach (int e in 
                w
                .GetMatchingEntities([typeof(Enemy), typeof(Frame), typeof(Health), typeof(Active)])
                .Where(ent => !w.GetComponent<Frame>(ent).IntersectsWith(cameraFrame))
                .Where(ent => !trackingEnts.Any(trackingEnt => w.GetComponent<TrackingOffScreen>(trackingEnt).Entity == ent))) {
                    int ent = EntityFactory.AddUI(w, Vector2.Zero, Constants.TileWidth / 2, Constants.TileWidth / 2, setOutline: true);
                    w.SetComponent<TrackingOffScreen>(ent, new TrackingOffScreen(e));
                }
                
            }
        });
    }

    public static void RegisterPosition(World w) {
        w.AddSystem([typeof(TrackingOffScreen), typeof(Frame), typeof(Active)], (w, e) => {
            int trackedEnt = w.GetComponent<TrackingOffScreen>(e).Entity; 
            (Frame trackedFrame, bool hasFrame) = w.GetComponentSafe<Frame>(trackedEnt); 
            Vector2 topleft = w.GetCameraTopLeft(); 
            Frame f = w.GetComponent<Frame>(e);

            if (hasFrame) {
                float screenRight = topleft.X + w.ScreenWidth; 
                float screenBottom = topleft.Y + w.ScreenHeight; 

                float y = Math.Clamp(trackedFrame.Y, topleft.Y, screenBottom);
                float x = Math.Clamp(trackedFrame.X, topleft.X, screenRight);

                if (trackedFrame.X < topleft.X) {
                    f.SetCoordinates(topleft.X, y);
                } else if (trackedFrame.X > screenRight) {
                    f.SetCoordinates(screenRight - f.GetWidth(), y);
                } else if (trackedFrame.Y < topleft.Y){
                    f.SetCoordinates(x, topleft.Y);
                } else if (trackedFrame.Y > screenBottom) {
                    f.SetCoordinates(x, screenBottom - f.GetHeight());
                } else {
                    w.RemoveEntity(e);
                }
            } else {
                w.RemoveEntity(e);
            }
        });
    }
}
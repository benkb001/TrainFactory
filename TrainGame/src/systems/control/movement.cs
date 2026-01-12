namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 

public class MovementSystem {
    private static Type[] types = [typeof(Collidable), typeof(Frame), typeof(Velocity), typeof(Active)]; 

    public static void RegisterCollision(World world) {
        Action<World, int> tf = (w, e) => {
            Frame f = w.GetComponent<Frame>(e); 
            Vector2 velocity = w.GetComponent<Velocity>(e).Vector; 
            float dx = velocity.X; 
            float dy = velocity.Y; 

            float dx_og = dx; 
            float dy_og = dy; 

            float x = f.GetX(); 
            float y = f.GetY(); 
            float width = f.GetWidth(); 
            float height = f.GetHeight(); 

            List<int> es = w.GetMatchingEntities([typeof(Frame), typeof(Collidable), typeof(Active)]); 

            for (int i = 0; i < es.Count; i++) {
                if (es[i] == e) {
                    continue; 
                }
                
                Frame other = w.GetComponent<Frame>(es[i]);
                (Velocity otherVelocity, bool success) = w.GetComponentSafe<Velocity>(es[i]); 
                Vector2 otherVelocityVec = success ? otherVelocity.Vector : Vector2.Zero; 
                RectangleF oRect = other.GetRectangle(otherVelocityVec); 

                RectangleF expectedHorizontal = new RectangleF(x + dx, y, width, height); 
                if (expectedHorizontal.IntersectsWith(oRect)) {
                    if (dx > 0) {
                        dx = (oRect.Left - width) - x; 
                    } else {
                        dx = oRect.Right - x; 
                    }
                }

                RectangleF expectedVertical = new RectangleF(x + dx, y + dy, width, height);
                if (expectedVertical.IntersectsWith(oRect)) {
                    if (dy > 0) {
                        dy = (oRect.Top - height) - y; 
                    } else {
                        dy = (oRect.Bottom) - y; 
                    }
                }
                
            }

            w.SetComponent<Velocity>(e, new Velocity(dx, dy)); 

        }; 

        world.AddSystem(types, tf); 
    }

    public static void Register(World w) {
        w.AddSystem([typeof(Frame), typeof(Velocity), typeof(Active)], (w, e) => {
            Frame f = w.GetComponent<Frame>(e); 
            Velocity v = w.GetComponent<Velocity>(e); 
            f.SetCoordinates(f.Position + v.Vector);
        });
    }
}
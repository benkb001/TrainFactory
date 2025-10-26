namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 

public class MovementSystem() {
    private static Type[] types = [typeof(Collidable), typeof(Frame), typeof(Velocity), typeof(Active)]; 

    public static void Register(World world) {
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

            List<int> es = w.GetEntities<Collidable>(); 
            foreach(int i in es) {
                if (i == e) {
                    continue; 
                }
                
                Frame other = w.GetComponent<Frame>(es[i]);
                RectangleF oRect = other.GetRectangle(); 

                RectangleF expectedHorizontal = new RectangleF(x + dx, y, width, height); 
                if (expectedHorizontal.IntersectsWith(oRect)) {
                    if (dx > 0) {
                        dx = (other.GetX() - width) - x; 
                    } else {
                        dx = (other.GetX() + other.GetWidth()) - x; 
                    }
                }

                RectangleF expectedVertical = new RectangleF(x + dx, y + dy, width, height);
                if (expectedVertical.IntersectsWith(oRect)) {
                    if (dy > 0) {
                        dy = (other.GetY() - other.GetHeight()) - y; 
                    } else {
                        dy = (other.GetY() + height) - y; 
                    }
                }
                
            }

            f.SetCoordinates(x + dx, y + dy);

            float dx_new = dx_og; 
            float dy_new = dy_og; 

            if (dx_og != dx) {
                dx_new = 0; 
            }

            if (dy_og != dy) {
                dy_new = 0; 
            }

            w.SetComponent<Velocity>(e, new Velocity(dx_new, dy_new)); 

        }; 

        world.AddSystem(types, tf); 
    }
}
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

public class Partition {
    RectangleF bounds; 
    List<int> entities; 

    public RectangleF Bounds => bounds; 
    public List<int> Ents => entities; 

    public Partition(RectangleF bounds, List<int> entities) {
        this.bounds = bounds; 
        this.entities = entities; 
    }
}

public class MovementBounds {
    private RectangleF bounds; 
    public RectangleF Bounds => bounds; 

    public MovementBounds(RectangleF bounds) {
        this.bounds = bounds;
    }
}

public class MovementSystem {
    private static Type[] types = [typeof(Collidable), typeof(Frame), typeof(Velocity), typeof(Active)]; 

    private static QuadTree<Partition> partition = 
        new QuadTree<Partition>(new Partition(new RectangleF(0f, 0f, 0f, 0f), new List<int>()));

    public static void DrawBounds(World w) {
        partition
        .GetAll()
        .Select(p => p.Bounds)
        .ToList()
        .ForEach(b => EntityFactory.AddUI(w, new Vector2(b.X, b.Y), b.Width, b.Height, setOutline: true));
    }

    public static List<Partition> Partitions => partition.GetLeaves();

    private static float baseLen = 1000f; 
    private static float minSize = 50f; 

    private static void setPartition(World w, QuadTree<Partition> pTree, int targetNum) {
        Partition p = pTree.Data; 
        List<int> es = p.Ents; 
        RectangleF bounds = p.Bounds; 

        if (es.Count > targetNum && bounds.Width > minSize) {
            float halfWidth = bounds.Width / 2f;
            float halfHeight = bounds.Height / 2f;
            float centerX = bounds.X + halfWidth; 
            float centerY = bounds.Y + halfHeight; 
            float rightX = bounds.X + bounds.Width; 
            float bottomY = bounds.Y + bounds.Height; 

            List<RectangleF> rs = new(){
                new RectangleF(bounds.X, bounds.Y, halfWidth, halfHeight),
                new RectangleF(centerX, bounds.Y, halfWidth, halfHeight),
                new RectangleF(bounds.X, centerY, halfWidth, halfHeight),
                new RectangleF(centerX, centerY, halfWidth, halfHeight)
            };

            List<List<int>> splitEs = rs.Select(
                r => es.Where(
                    e => r.IntersectsWith(w.GetComponent<MovementBounds>(e).Bounds))
                .ToList())
            .ToList();
            
            pTree.SetChildren(
                new Partition(rs[0], splitEs[0]),
                new Partition(rs[1], splitEs[1]), 
                new Partition(rs[2], splitEs[2]), 
                new Partition(rs[3], splitEs[3])
            );

            (QuadTree<Partition> p1, QuadTree<Partition> p2, QuadTree<Partition> p3, QuadTree<Partition> p4) =
                pTree.Children; 
            
            setPartition(w, p1, targetNum); 
            setPartition(w, p2, targetNum); 
            setPartition(w, p3, targetNum); 
            setPartition(w, p4, targetNum); 
        }
    }

    public static void RegisterPartition(World w) {
        w.AddSystem((w) => {
            List<int> es = w.GetMatchingEntities([typeof(Collidable), typeof(Frame), typeof(Active)]);
            if (es.Count == 0) {
                return;
            }

            es.ForEach(e => {
                (Velocity v, bool success) = w.GetComponentSafe<Velocity>(e); 
                Frame f = w.GetComponent<Frame>(e); 
                if (!success) {
                    w.SetComponent<MovementBounds>(e, new MovementBounds(f.GetRectangle())); 
                } else {
                    Vector2 vVec = v.Vector; 
                    float dx = vVec.X; 
                    float dy = vVec.Y; 
                    Vector2 expected = f.Position + vVec; 
                    Vector2 curPos = f.Position; 

                    float width = f.GetWidth(); 
                    float height = f.GetHeight();

                    RectangleF bounds = new RectangleF(Math.Min(curPos.X, expected.X), Math.Min(curPos.Y, expected.Y), 
                        Math.Max(width + dx, width - dx), Math.Max(height + dy, height - dy));
                    w.SetComponent<MovementBounds>(e, new MovementBounds(bounds)); 
                }
            });

            Vector2 center = w.GetComponent<Frame>(es[0]).Position; 

            RectangleF bounds = new RectangleF(
                center.X - baseLen, center.Y - baseLen, baseLen * 2, baseLen * 2); 
            Partition p = new Partition(bounds, es); 

            partition = new QuadTree<Partition>(p);
            int targetNum = (int)Math.Sqrt((double)es.Count); 
            targetNum = Math.Max(1, targetNum);

            setPartition(w, partition, targetNum);
        });
    }

    public static void RegisterCollision(World world) {
        Action<World> tf = (w) => {
            partition
            .GetLeaves()
            .Select(p => p.Ents)
            .ToList()
            .ForEach(es => {
                foreach(int e in es) {
                    (Frame f, bool s0) = w.GetComponentSafe<Frame>(e); 
                    (Velocity v, bool s1) = w.GetComponentSafe<Velocity>(e); 

                    if (!s0 || !s1) {
                        continue;
                    }

                    Vector2 velocity = v.Vector;
                    float dx = velocity.X; 
                    float dy = velocity.Y; 

                    float dx_og = dx; 
                    float dy_og = dy; 

                    float x = f.GetX(); 
                    float y = f.GetY(); 
                    float width = f.GetWidth(); 
                    float height = f.GetHeight(); 

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
                }
            });
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

    public static List<int> GetIntersectingEntities(World w, int e) {
        List<int> es = new();
        Frame f = w.GetComponent<Frame>(e);

        partition
        .GetLeaves()
        .ForEach(p => {
            if (p.Bounds.IntersectsWith(f.GetRectangle())) {
                foreach (int otherEnt in p.Ents) {
                    (Frame otherF, bool success) = w.GetComponentSafe<Frame>(otherEnt); 
                    if (otherEnt != e && success && otherF.IntersectsWith(f)) {
                        es.Add(otherEnt);
                    }
                }
            }
        });
        
        return es; 
    }
}
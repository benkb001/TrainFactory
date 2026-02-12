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
    HashSet<int> entities; 

    public RectangleF Bounds => bounds; 
    public HashSet<int> Ents => entities; 

    public Partition(RectangleF bounds, HashSet<int> entities) {
        this.bounds = bounds; 
        this.entities = entities; 
    }
}

public class MovementBounds {
    private RectangleF bounds; 
    public readonly List<Partition> Partitions = new(); 

    public RectangleF Bounds => bounds; 

    public MovementBounds(RectangleF bounds) {
        SetBounds(bounds);
    }

    public void SetBounds(RectangleF bounds) {
        this.bounds = bounds;
    }

    public void AddPartition(Partition partition) {
        Partitions.Add(partition);
    }
}

public static class MovementSystem {
    private static Type[] types = [typeof(Collidable), typeof(Frame), typeof(Velocity), typeof(Active)]; 
    private static float baseLen = 500f; 
    private static float minSize = 50f; 
    private static QuadTree<Partition> setPartition(Vector2 center) {
        
        RectangleF bounds = new RectangleF(
            center.X - baseLen, center.Y - baseLen, baseLen * 2, baseLen * 2); 
        Partition p = new Partition(bounds, new HashSet<int>()); 
        partitions = new() { p };

        return new QuadTree<Partition>(p);
    }

    private static HashSet<Partition> partitions;
    private static QuadTree<Partition> partition = setPartition(Vector2.Zero);

    public static RectangleF Bounds => partition.Data.Bounds;
    private static List<int> drawnBoundEnts = new();

    public static void DrawBounds(World w) {
        drawnBoundEnts.ForEach(e => w.RemoveEntity(e));
        partition
        .GetAll()
        .Select(p => p.Bounds)
        .ToList()
        .ForEach(b => {
            int e = EntityFactory.AddUI(w, new Vector2(b.X, b.Y), b.Width, b.Height, setOutline: true);
            drawnBoundEnts.Add(e);
        });
    }

    public static void SetCollisionSpace(Vector2 center) {
        partition = setPartition(center);
    }

    public static HashSet<Partition> Partitions => partitions;

    private static List<Partition> getIntersectingPartitions(RectangleF rect) {
        List<QuadTree<Partition>> toProcess = new() { partition };
        List<Partition> ps = new(); 

        while (toProcess.Count > 0) {

            QuadTree<Partition> curTree = toProcess[0]; 
            Partition curPartition = curTree.Data; 

            if (rect.IntersectsWith(curPartition.Bounds)) {
                if (!curTree.HasChildren) {
                    ps.Add(curPartition);
                } else {
                    toProcess.Add(curTree.C1);
                    toProcess.Add(curTree.C2);
                    toProcess.Add(curTree.C3);
                    toProcess.Add(curTree.C4);
                }
            }

            toProcess.RemoveAt(0);
        }
        
        return ps; 
    }


    private static void addToPartition(World w, MovementBounds mb, int e, int maxEnts) {
        maxEnts = Math.Max(2, maxEnts);

        List<QuadTree<Partition>> toProcess = new() { partition };

        while (toProcess.Count > 0) {

            QuadTree<Partition> curTree = toProcess[0]; 
            Partition curPartition = curTree.Data; 

            if (mb.Bounds.IntersectsWith(curPartition.Bounds)) {
                if (curPartition.Ents.Count < maxEnts) {
                    curPartition.Ents.Add(e);
                    mb.AddPartition(curPartition);
                } else {

                    if (!curTree.HasChildren) {
                        RectangleF bounds = curPartition.Bounds;

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

                        curTree.SetChildren(
                            new Partition(rs[0], new()),
                            new Partition(rs[1], new()), 
                            new Partition(rs[2], new()), 
                            new Partition(rs[3], new())
                        );

                        partitions.Add(curTree.C1.Data);
                        partitions.Add(curTree.C2.Data);
                        partitions.Add(curTree.C3.Data);
                        partitions.Add(curTree.C4.Data);
                    }

                    toProcess.Add(curTree.C1);
                    toProcess.Add(curTree.C2);
                    toProcess.Add(curTree.C3);
                    toProcess.Add(curTree.C4);
                }
            }

            toProcess.RemoveAt(0);
        }
    }

    private static void updatePartition(World w, MovementBounds mb, int e, int maxEnts) {
        foreach (Partition p in mb.Partitions) {
            p.Ents.Remove(e);
        }

        addToPartition(w, mb, e, maxEnts);
    }

    private static void snipPartition(int maxEnts) {

        List<QuadTree<Partition>> toProcess = new() { partition };
        while (toProcess.Count > 0) {
            
            QuadTree<Partition> curTree = toProcess[0]; 

            if (curTree.HasChildren) {
                HashSet<int> e1 = curTree.C1.Data.Ents; 
                HashSet<int> e2 = curTree.C2.Data.Ents;
                HashSet<int> e3 = curTree.C3.Data.Ents; 
                HashSet<int> e4 = curTree.C4.Data.Ents; 
                HashSet<int> es = curTree.Data.Ents;

                if ((e1.Count + e2.Count + e3.Count + e4.Count + es.Count) < maxEnts) {
                    es.UnionWith(e1);
                    es.UnionWith(e2);
                    es.UnionWith(e3);
                    es.UnionWith(e4);

                    curTree.RemoveChildren();
                } else {
                    toProcess.Add(curTree.C1);
                    toProcess.Add(curTree.C2);
                    toProcess.Add(curTree.C3);
                    toProcess.Add(curTree.C4);
                }

            }

            toProcess.RemoveAt(0);
        }
    }

    public static void RegisterPartition(World w) {
        w.AddSystem((w) => {
            List<int> es = w.GetMatchingEntities([typeof(Collidable), typeof(Frame), typeof(Active)]);
            if (es.Count == 0) {
                return;
            }

            int maxEnts = (int)Math.Sqrt((double)es.Count); 

            es.ForEach(e => {
                (Velocity vel, bool s1) = w.GetComponentSafe<Velocity>(e); 
                Vector2 v = Vector2.Zero; 

                (MovementBounds mb, bool s2) = w.GetComponentSafe<MovementBounds>(e);

                Frame f = w.GetComponent<Frame>(e); 

                if (s1) {
                    v = vel.Vector; 
                }

                if (v.Length() > 0) {

                    float dx = v.X; 
                    float dy = v.Y; 
                    Vector2 expected = f.Position + v; 
                    Vector2 curPos = f.Position; 

                    float width = f.GetWidth(); 
                    float height = f.GetHeight();

                    RectangleF bounds = new RectangleF(Math.Min(curPos.X, expected.X), Math.Min(curPos.Y, expected.Y), 
                        Math.Max(width + dx, width - dx), Math.Max(height + dy, height - dy));

                    if (!s2) {
                        mb = new MovementBounds(bounds);
                        w.SetComponent<MovementBounds>(e, mb); 
                    } else {
                        mb.SetBounds(bounds);
                    }
                }

                if (!s2) {
                    mb = new MovementBounds(f.GetRectangle()); 
                }

                if (!s2 || v.Length() > 0) {
                    updatePartition(w, mb, e, maxEnts);
                }

            });

            if (VirtualKeyboard.IsClicked(Keys.B)) {
                DrawBounds(w);
            }
        });
    }

    public static void RegisterCollision(World world) {
        Action<World> tf = (w) => {

            foreach (Partition p in partitions) {
                HashSet<int> es = p.Ents; 
                List<int> esToRemove = new(); 

                foreach (int e in es) {
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

                    foreach (int otherEnt in es) {
                        (Frame other, bool fSuccess) = w.GetComponentSafe<Frame>(otherEnt);

                        if (!fSuccess) {
                            esToRemove.Add(otherEnt);
                            continue;
                        }

                        if (otherEnt == e) {
                            continue; 
                        }
                        
                        (Velocity otherVelocity, bool success) = w.GetComponentSafe<Velocity>(otherEnt); 
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

                foreach (int e in esToRemove) {
                    es.Remove(e);
                }
            }
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

    public static List<int> FillIntersectingEntities(World w, int e, List<int> es) {
        Frame f = w.GetComponent<Frame>(e);

        foreach (Partition p in getIntersectingPartitions(f.GetRectangle())) {
            foreach (int otherEnt in p.Ents) {
                (Frame fOther, bool success) = w.GetComponentSafe<Frame>(otherEnt);
                if (success && f.IntersectsWith(fOther)) {
                    es.Add(otherEnt);
                }
            }
        }
        
        return es; 
    }
}
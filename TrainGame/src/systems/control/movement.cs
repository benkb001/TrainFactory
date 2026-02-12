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

public class MovementBounds {
    private RectangleF bounds; 

    public RectangleF Bounds => bounds; 

    public MovementBounds(RectangleF bounds) {
        SetBounds(bounds);
    }

    public void SetBounds(RectangleF bounds) {
        this.bounds = bounds;
    }
}

public class SpatialHash {
    //Think of inner arrays as vertical
    private List<List<HashSet<int>>> partitions; 
    private List<HashSet<int>> psFlat;
    private List<HashSet<int>> psCur; 

    private RectangleF bounds;
    private float cellWidth; 
    private int cellsPerRow; 

    public SpatialHash(RectangleF bounds, int cellsPerRow) {
        partitions = new List<List<HashSet<int>>>(); 
        psCur = new();
        psFlat = new(); 
        cellWidth = bounds.Width / (float)cellsPerRow;
        this.bounds = bounds;
        this.cellsPerRow = cellsPerRow; 
        
        for (int i = 0; i < cellsPerRow; i++) {
            List<HashSet<int>> ps = new(); 
            partitions.Add(ps);
            for (int j = 0; j < cellsPerRow; j++) {
                HashSet<int> p = new();
                ps.Add(p);
                psFlat.Add(p);
            }
        }
    }

    private (float, float) getCoords(int x, int y) {
        return (bounds.X + (x * cellWidth), bounds.Y + (y * cellWidth));
    }

    private bool intersectsWith(int x, int y, RectangleF f) {
        (float cX, float cY) = getCoords(x, y); 
        
        float cRight = cX + cellWidth; 
        float cBot = cY + cellWidth; 
        bool intersects = f.Left <= cRight && f.Right >= cX && f.Top <= cBot && f.Bottom >= cY;
        return intersects;
    }

    private (int, int) getCell(RectangleF rect) {
        float xF = (rect.X - bounds.X) / cellWidth; 

        if (Util.FloatEqual(xF, (float)((int)xF))) {
            xF -= 1f; 
        }

        float yF = (rect.Y - bounds.Y) / cellWidth; 
        if (Util.FloatEqual(yF, (float)((int)yF))) {
            yF -= 1f;
        }

        int x = Math.Clamp((int)xF, 0, cellsPerRow - 1);
        int y = Math.Clamp((int)yF, 0, cellsPerRow - 1); 
        
        return (x, y);
    }

    private List<HashSet<int>> getIntersectingPartitions(RectangleF rect) {
        (int x, int y) = getCell(rect);
        int maxX = x; 
        int maxY = y; 
        List<HashSet<int>> ps = psCur;
        ps.Clear();
        
        bool found = false; 
        while (partitions.Count > (maxX + 1) && !found) {
            if (intersectsWith(maxX + 1, y, rect)) {
                maxX++;
            } else {
                found = true; 
            }
        }

        found = false; 
        while (partitions[x].Count > (maxY + 1) && !found) {
            if (intersectsWith(x, maxY + 1, rect)) {
                maxY++; 
            } else {
                found = true; 
            }
        }

        for (int i = x; i <= maxX; i++) {
            for (int j = y; j <= maxY; j++) {
                ps.Add(partitions[i][j]);
            }
        }

        return ps; 
    }
    
    public void AddEnt(RectangleF rect, int e) {
        List<HashSet<int>> ps = getIntersectingPartitions(rect);
        foreach (HashSet<int> p in ps) {
            p.Add(e);
        }
    }

    public void RemoveEnt(int e) {
        partitions.ForEach(ps => ps.ForEach(p => p.Remove(e)));
    }

    public HashSet<int> GetIntersectingEntities(World w, Frame f) {
        HashSet<int> es = new(); 

        FillIntersectingEnts(w, f, es);
        return es; 
    }

    public void FillIntersectingEnts(World w, Frame f, HashSet<int> es) {
        es.Clear();
        List<HashSet<int>> ps = getIntersectingPartitions(f.GetRectangle());
        foreach (HashSet<int> p in ps) {
            foreach (int e in p) {
                (Frame otherF, bool success) = w.GetComponentSafe<Frame>(e);
                if (success && otherF.IntersectsWith(f)) {
                    es.Add(e);
                }
            }
        }
    }

    public List<HashSet<int>> GetAll() {
        return psFlat;
    }

    public List<RectangleF> GetAllRects() {
        List<RectangleF> rs = new(); 

        for (int i = 0; i < cellsPerRow; i++) {
            for (int j = 0; j < cellsPerRow; j++) {
                (float x, float y) = getCoords(i, j);
                rs.Add(new RectangleF(x, y, cellWidth, cellWidth));
            }
        }
        return rs; 
    }
}

public static class MovementSystem {
    private static Type[] types = [typeof(Collidable), typeof(Frame), typeof(Velocity), typeof(Active)]; 
    private static float baseLen = 400f; 
    private static int cellsPerRow = 4; 
    private static SpatialHash setPartition(Vector2 center) {
        
        RectangleF bounds = new RectangleF(
            center.X - baseLen, center.Y - baseLen, baseLen * 2, baseLen * 2); 
        return new SpatialHash(bounds, cellsPerRow);
    }

    private static SpatialHash partition = setPartition(Vector2.Zero);

    private static List<int> drawnBoundEnts = new();

    public static List<HashSet<int>> Partitions => partition.GetAll();

    public static void DrawBounds(World w) {
        drawnBoundEnts.ForEach(e => w.RemoveEntity(e));
        partition
        .GetAllRects()
        .ForEach(b => {
            int e = EntityFactory.AddUI(w, new Vector2(b.X, b.Y), b.Width, b.Height, setOutline: true);
            drawnBoundEnts.Add(e);
        });
    }

    public static void SetCollisionSpace(Vector2 center) {
        partition = setPartition(center);
    }

    private static void addToPartition(World w, MovementBounds mb, int e) {
        partition.AddEnt(mb.Bounds, e);
    }

    private static void updatePartition(World w, MovementBounds mb, int e) {
        partition.RemoveEnt(e);
        addToPartition(w, mb, e);
    }

    public static void RegisterPartition(World w) {
        w.AddSystem((w) => {
            List<int> es = w.GetMatchingEntities([typeof(Collidable), typeof(Frame), typeof(Active)]);
            if (es.Count == 0) {
                return;
            }

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
                } else if (!s2) {
                    mb = new MovementBounds(f.GetRectangle());
                    w.SetComponent<MovementBounds>(e, mb);
                }

                if (!s2 || v.Length() > 0) {
                    updatePartition(w, mb, e);
                }

            });

            if (VirtualKeyboard.IsClicked(Keys.B)) {
                DrawBounds(w);
            }
        });
    }

    public static void RegisterCollision(World world) {
        Action<World> tf = (w) => {

            foreach (HashSet<int> es in partition.GetAll()) {
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

    public static void FillIntersectingEnts(World w, int e, HashSet<int> es) {
        partition.FillIntersectingEnts(w, w.GetComponent<Frame>(e), es);
    }

    public static HashSet<int> GetIntersectingEntities(World w, int e) {
        return GetIntersectingEntities(w, w.GetComponent<Frame>(e));
    }

    public static HashSet<int> GetIntersectingEntities(World w, Frame f) {
        return partition.GetIntersectingEntities(w, f);
    }
}
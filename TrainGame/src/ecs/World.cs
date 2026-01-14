namespace TrainGame.ECS; 

using System.Collections.Generic;
using System; 
using System.Linq;
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color; 

using _Color = System.Drawing.Color; 
using _Rectangle = System.Drawing.Rectangle;

using TrainGame.Constants; 
using TrainGame.Components; 
using TrainGame.Utils; 

public partial class World {
    private EntityManager em = new(); 
    private ComponentManager cm = new(); 
    private SystemManager sm = new(); 

    private GraphicsDeviceManager _graphics {get;}
    private GraphicsDevice graphicsDevice; 
    private SpriteBatch _spriteBatch {get;}
    private Dictionary<string, Texture2D> textures = new(); 
    private Camera camera; 

    private SpriteFont font; 
    private Vector2 targetCameraPosition; 
    private bool targetCameraPositionIsCurrent; 
    private int trackedEntity = -1;
    private GameClock gameClock = new GameClock(); 
    private Texture2D _pixel;
    private WorldTime wt = new WorldTime(); 
    public WorldTime Time => wt; 
    public readonly float ScreenHeight = 0f; 
    public readonly float ScreenWidth = 0f; 
    public int MiliticksPerUpdate => wt.MiliticksPerUpdate; 
    private Random random; 

    //TODO: consider replacing with checks for specific variables being null
    private bool isTest = false;  
    public int Frame = 0; 

    public World() { 
        isTest = true;
        random = new Random(); 
    }

    public World(GraphicsDeviceManager gm, GraphicsDevice gd, SpriteBatch s) {
        _graphics = gm; 
        graphicsDevice = gd; 
        _spriteBatch = s; 

        camera = new Camera(graphicsDevice.Viewport);
        ScreenWidth = gd.Viewport.Width; 
        ScreenHeight = gd.Viewport.Height; 
        camera.SetPosition(new Vector2(gd.Viewport.Width / 2, gd.Viewport.Height / 2)); 
        camera.UpdateCamera(graphicsDevice.Viewport); 
        random = new Random(); 
    }

    public void AddComponentType<T>() {
        cm.Register<T>(); 
    }

    public int AddEntity() {
        if (SystemCount() == 0) {
            throw new InvalidOperationException("Must add systems before entities");
        }
        int e = em.Add(); 
            
        return e; 
    }

    public _System AddSystem(Type[] ts, Action<World, int> transformer, Func<int, int> orderer = null) {
        bool[] signature = cm.GetSignature(ts); 
        return sm.Register(signature, transformer, orderer);
    }

    public _System AddSystem(Type[] ts, Action<World> update) {
        bool[] signature = cm.GetSignature(ts); 
        return sm.Register(signature, update);
    }

    public _System AddSystem(Action<World> update) {
        bool[] signature = cm.GetSignature([]); 
        return sm.Register(signature, update);
    }

    public void Clear() {
        foreach (int i in em.GetEntities()) {
            RemoveEntity(i); 
        }
    }

    public bool ComponentContainsEntity<T>(int e) {
        return cm.ComponentContainsEntity<T>(e); 
    }

    public int ComponentEntityCount<T>() {
        return cm.EntityCount<T>(); 
    }

    public bool ComponentTypeIsRegistered<T>() {
        return cm.ComponentTypeIsRegistered<T>(); 
    }

    public int EntityCount() {
        return em.EntityCount(); 
    }

    public bool EntityExists(int e) {
        return em.EntityExists(e); 
    }

    public T GetComponent<T>(int entity) {
        if (!EntityExists(entity)) {
            throw new InvalidOperationException($"Entity {entity} does not exist"); 
        }
        return cm.GetComponent<T>(entity); 
    }

    public (T, bool) GetComponentSafe<T>(int entity) {
        if (!EntityExists(entity) || !cm.ComponentContainsEntity<T>(entity)) {
            return (default(T), false); 
        }
        return (cm.GetComponent<T>(entity), true); 
    }

    public Dictionary<int, T> GetComponentArray<T>() {
        return cm.GetComponentArray<T>().GetEntities(); 
    }

    public int GetComponentIndex<T>() {
        return cm.GetComponentIndex<T>(); 
    }

    public int GetComponentTypeCount() {
        return cm.GetComponentTypeCount(); 
    }

    public List<int> GetEntities<T>() {
        return cm.GetComponentArray<T>().GetEntities().Keys.ToList(); 
    }

    public List<int> GetEntities() {
        return em.GetEntities(); 
    }

    public List<int> GetMatchingEntities(Type[] ts) {
        bool[] sig = cm.GetSignature(ts); 
        return em.GetMatchingEntities(sig); 
    }

    public double GetSecondsPassed() {
        return gameClock.TotalSeconds; 
    }

    public double NextDouble() {
        return (random.NextDouble() * 2) - 1; 
    }

    public float NextFloat() {
        float f = (float)random.NextDouble(); 
        if (f < 0f) {
            throw new InvalidOperationException("bug");
        }
        return f;
    }

    public void PassTime(double seconds = 0, double milliseconds = 0) {
        gameClock.PassTime(seconds, milliseconds); 
    }

    public void PassTime(WorldTime wt) {
        this.wt += wt; 
    }

    public void RemoveComponent<T>(int e) {
        bool[] signature = em.GetSignature(e); 
        
        signature = cm.RemoveComponent<T>(e, signature); 

        SetSignature(e, signature); 
    }

    public void RemoveEntity(int entity) {
        em.RemoveEntity(entity); 
        cm.RemoveEntity(entity); 
        sm.RemoveEntity(entity); 
    }

    public void SetSignature(int entity, bool[] signature) {
        em.SetEntitySignature(entity, signature); 
        sm.SetEntitySignature(entity, signature); 
    }

    public int SetComponent<T>(int e, T c) {
        bool[] signature = em.GetSignature(e); 
        signature = cm.AddComponent<T>(e, signature, c);
        SetSignature(e, signature); 

        return e; 
    }

    public void SetTargetCameraPosition(Vector2 v) {
        targetCameraPosition = v; 
        targetCameraPositionIsCurrent = true; 
    }

    public void SetTargetCameraPositionToScreenOrigin() {
        targetCameraPosition = new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2); 
        targetCameraPositionIsCurrent = true; 
    }

    public int SystemCount() {
        return sm.SystemCount(); 
    }

    public void Update() {
        VirtualMouse.UpdateStartFrame(); 
        
        bool tracked = false; 

        if (!isTest) {
            if (GetMatchingEntities([typeof(Frame), typeof(Active)]).Contains(trackedEntity)) {
                Frame f = GetComponent<Frame>(trackedEntity); 
                camera.SetPosition(new Vector2(f.GetX(), f.GetY())); 
                tracked = true; 
            }
        }

        sm.Update(this); 

        if (!isTest) {
            if (targetCameraPositionIsCurrent && !tracked) {
                camera.SetPosition(targetCameraPosition);
                targetCameraPositionIsCurrent = false; 
            }
            camera.UpdateCamera(graphicsDevice.Viewport); 
        }

        VirtualMouse.UpdateEndFrame(); 
        VirtualKeyboard.UpdatePrevFrame(); 
        wt.Update();
        Frame++;  
    }

    public void SetMiliticksPerUpdate(int t) {
        wt.SetMiliticksPerUpdate(t);
    }

    /*
    TODO: low-prio Write
    public string ToString() {
        string res = ""; 
        foreach (int e in em.GetEntities()) {
            res = $"{res}Entity {e}: "; 
            bool[] signature = em.GetSignature(e); 
            foreach (bool b in signature) {

            }
        }
    }
    */
}
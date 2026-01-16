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
using TrainGame.Systems;

public static class EntityFactory {

    public static int AddData<T>(World w, T component) {
        List<int> dataEnts = w.GetMatchingEntities([typeof(Data), typeof(T)]);

        foreach (int e in dataEnts) {
            T existing = w.GetComponent<T>(e); 
            if (existing.Equals(component)) {
                Console.WriteLine("Found existing data entity"); 
                return e;
            }
        }

        int dataEnt = Add(w, setData: true);
        w.SetComponent<T>(dataEnt, component); 
        return dataEnt; 
    }

    public static int AddData<T>(World w, T component, int e) {
        List<int> dataEnts = w.GetMatchingEntities([typeof(Data), typeof(T)]);

        bool exists = dataEnts.Any(ent => w.GetComponent<T>(ent).Equals(component));

        if (exists) {
            return -1;
        } else {
            w.SetComponent<T>(e, component); 
            return e; 
        }
    }

    public static int Add(World w, bool setScene = true, bool setActive = true, bool setData = false, 
        SceneType type = SceneType.None) {
        int e = w.AddEntity(); 

        //todo: test
        if (setData) {
            w.SetComponent<Data>(e, Data.Get()); 
            return e; 
        }

        if (setScene) {
            if (type == SceneType.None) {
                type = SceneSystem.CurrentScene; 
            }

            w.SetComponent<Scene>(e, new Scene(type: type));
        }
        
        if (setActive && type == SceneSystem.CurrentScene) {
            w.SetComponent<Active>(e, Active.Get()); 
        }

        return e; 
    }

    public static int AddUI(World w, Vector2 position, float width, float height, bool setButton = false, 
        string text = "", bool setOutline = false, bool screenAnchor = false, bool setInteractable = false, 
        bool setCollidable = false) {

        int e = Add(w); 
        w.SetComponent<Frame>(e, new Frame(position, width, height)); 
        if (screenAnchor) {
            w.SetComponent<ScreenAnchor>(e, new ScreenAnchor(position)); 
        }

        if (setButton) {
            w.SetComponent<Button>(e, new Button());
        }       
        if (setOutline) {
            w.SetComponent<Outline>(e, new Outline()); 
        }
        if (text != "") {
            w.SetComponent<TextBox>(e, new TextBox(text)); 
        }
        if (setCollidable) {
            w.SetComponent<Collidable>(e, new Collidable()); 
        }
        if (setInteractable) {
            w.SetComponent<Interactable>(e, new Interactable()); 
        }
        
        return e; 
    }

    public static int AddButton(World w, float x, float y, float width, float aspectRatio, int depth, Texture2D spr) {
        int e = Add(w); 
        w.SetComponent<Frame>(e, new Frame(x, y, width, width * aspectRatio)); 
        w.SetComponent<Sprite>(e, new Sprite(spr, depth)); 
        w.SetComponent<Button>(e, new Button()); 
        return e; 
    }

    public static int AddInventory(World w, Inventory inv) {
        int e = Add(w, setScene: false); 
        w.SetComponent<Inventory>(e, inv); 
        return e; 
    }

    public static int GetDataEntity<T>(World w, T component) {
        List<KeyValuePair<int, T>> es = w.GetComponentArray<T>().Where(kvp => Object.ReferenceEquals(kvp.Value, component)).ToList(); 
        if (es.Count == 0) {
            return -1; 
        } else if (es.Count == 1) {
            return es[0].Key; 
        } else {
            throw new InvalidOperationException($"Data for {typeof(T)} had more than 1 data entity set");
        }
    }

    public static int AddToast(World w, float width, float height, string t) {
        int e = AddUI(w, w.GetCameraTopLeft(), width, height, setOutline: true, text: t); 
        w.SetComponent<Toast>(e, new Toast()); 
        return e; 
    }

    public static int AddToast(World w, Vector2 pos, float width, float height, string t) {
        int e = AddUI(w, pos, width, height, setOutline: true, text: t); 
        w.SetComponent<Toast>(e, new Toast()); 
        return e; 
    }
}   
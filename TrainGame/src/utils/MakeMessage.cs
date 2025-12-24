namespace TrainGame.Utils; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.ECS;
using TrainGame.Constants; 
using TrainGame.Callbacks; 

public class MakeMessage {

    public static void Add<T>(World w, T msg) {
        int e = EntityFactory.Add(w, setScene: false); 
        w.SetComponent<T>(e, msg); 
    }

    public static void DrawInventory(Inventory inv, World w, Vector2 Position, float Width, float Height) {
        DrawInventoryCallback.Create(w, inv, Position, Width, Height, Padding: 5f, SetMenu: true, DrawLabel: true); 
    }

    public static void DrawMachineInterface(World w, Machine m, bool playerAtMachine = false) {
        View.EnterMenu(w); 
        //draw machine inventory
        Inventory inv = m.Inv; 

        float invWidth = w.ScreenWidth - 50f; 
        float invHeight = w.ScreenHeight / 4f; 
        float invY = w.ScreenHeight - invHeight - 10f; 
        float invX = 25f; 

        DrawInventory(inv, w, w.GetCameraTopLeft() + new Vector2(invX, invY), invWidth, invHeight); 

        //draw header
        
        Vector2 topLeft = w.GetCameraTopLeft(); 
        float headerWidth = w.ScreenWidth / 2.5f; 
        float headerHeight = w.ScreenHeight - invHeight - 10f; 
        Vector2 headerPosition = topLeft + new Vector2(10, 10); 

        Frame headerFrame = new Frame(headerPosition, headerWidth, headerHeight);
        
        string hStr = $"{m.Id}\n"; 
        hStr += $"Level: {m.Level}\n"; 
        hStr += $"Craft Speed: {m.GetCraftSpeedFormatted()}"; 
        hStr += $"Recipe: \n{m.GetRecipeFormatted()}"; 
        TextBox headerTextBox = new TextBox(hStr); 
        
        int drawHeaderEntity = EntityFactory.Add(w, setScene: false); 

        w.SetComponent<DrawCallback>(drawHeaderEntity, new DrawCallback(() => {
            int headerEntity = EntityFactory.Add(w); 
            w.SetComponent<TextBox>(headerEntity, new TextBox(hStr)); 
            w.SetComponent<Frame>(headerEntity, new Frame(headerPosition, headerWidth, headerHeight)); 
            w.SetComponent<Outline>(headerEntity, new Outline()); 
        }));

        //draw request stepper if not produce infinite (which means it produces as much as it can all the time)
        float requestWidth = w.ScreenWidth / 5f; 
        float requestHeight = headerHeight;
        Vector2 requestPosition = headerPosition + new Vector2(headerWidth + 10f, 0f); 

        if (!m.ProduceInfinite) {
            DrawMachineRequestCallback.Create(w, m, requestPosition, requestWidth, requestHeight); 
        }

        //Draw progress bar

        Vector2 pbPosition = requestPosition + new Vector2(requestWidth + 10f, 0f); 
        float pbWidth = w.ScreenWidth - requestWidth - headerWidth - 35f; 
        float pbHeight = pbWidth / 10f; 

        int drawPbEntity = EntityFactory.Add(w, setScene: false); 
        DrawProgressBarCallback.Create(w, pbPosition, pbWidth, pbHeight, m); 
        
        //draw upgrade button, clickable only if player inv was specified 
        Vector2 upgradePosition = pbPosition + new Vector2(0f, pbHeight + 10f); 
        float upgradeWidth = pbWidth; 
        float upgradeHeight = pbHeight * 2; 
        Action callback = DrawUpgradeMachineButtonCallback.Create(w, m, playerAtMachine, 
            upgradePosition, upgradeWidth, upgradeHeight);
        DrawCallback cb = new DrawCallback(callback); 

        int cbEntity = EntityFactory.Add(w, setScene: false); 
        w.SetComponent<DrawCallback>(cbEntity, cb); 

        
        //push
        PushFactory.Build(w); 
    }
}
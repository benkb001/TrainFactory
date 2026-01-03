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
using TrainGame.Constants;
using TrainGame.Callbacks; 

public static class DrawCityInterfaceSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(DrawCityInterfaceMessage)], (w, e) => {
            SceneSystem.EnterScene(w, SceneType.CityInterface); 

            Vector2 topleft = w.GetCameraTopLeft(); 
            City city = w.GetComponent<DrawCityInterfaceMessage>(e).GetCity();
            Inventory inv = city.Inv;

            int mFlagEnt = EntityFactory.Add(w); 
            w.SetComponent<Menu>(mFlagEnt, new Menu(city: city)); 

            (float invWidth, float invHeight) = InventoryWrap.GetUI(inv); 
            invWidth *= 0.75f; 
            invHeight *= 0.75f; 
            float invY = w.ScreenHeight - invHeight - 10f; 
            float invX = 25f; 

            DrawInventoryCallback.Draw(w, inv, topleft + new Vector2(invX, invY), invWidth, invHeight, 
                Padding: Constants.InventoryPadding, DrawLabel: true); 

            float viewWidth = w.ScreenWidth / 5; 
            float viewHeight = w.ScreenHeight - invHeight - 50f; 
            float trainsViewX = viewWidth; 
            float trainsViewY = 25f;
            Vector2 trainsPosition = topleft + new Vector2(trainsViewX, trainsViewY); 

            int drawTrainsMsgEntity = EntityFactory.Add(w); 
            DrawTrainsViewMessage tvMsg = new DrawTrainsViewMessage(
                city.Trains.Values.ToList(), 
                viewWidth, 
                viewHeight, 
                trainsPosition, 
                10f
            );  
            DrawTrainsViewSystem.Draw(tvMsg, w); 
            
            Vector2 machinesPosition = trainsPosition + new Vector2(2 * viewWidth, 0);
            
            int drawMachinesMsgEntity = EntityFactory.Add(w); 
            DrawMachinesViewMessage mvMsg = new DrawMachinesViewMessage(
                city.Machines.Values.ToList(), 
                viewWidth, 
                viewHeight, 
                machinesPosition, 
                10f
            ); 
            DrawMachinesViewSystem.Draw(mvMsg, w);

            w.RemoveEntity(e);
        }); 
    }
}
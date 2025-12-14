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



//required order: 
// cityClick -> push -> drawCityDetail 
public class CityClickSystem() {
    public static void Register(World world) {
        Type[] ts = [typeof(CityUI), typeof(Button), typeof(Frame), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            if (w.GetComponent<Button>(e).Clicked) {
                View.EnterMenu(w);
                Vector2 topleft = w.GetCameraTopLeft(); 
                
                City city = w.GetComponent<CityUI>(e).GetCity(); 
                int pushEntity = EntityFactory.Add(w); 
                w.SetComponent<PushSceneMessage>(pushEntity, PushSceneMessage.Get()); 
                
                Inventory inv = w.GetComponent<CityUI>(e).GetCity().Inv; 

                float invWidth = w.ScreenWidth - 50f; 
                float invHeight = w.ScreenHeight / 4f; 
                float invY = w.ScreenHeight - invHeight - 10f; 
                float invX = 25f; 

                MakeMessage.DrawInventory(inv, w, topleft + new Vector2(invX, invY), invWidth, invHeight); 

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
                w.SetComponent<DrawTrainsViewMessage>(drawTrainsMsgEntity, tvMsg); 
                
                Vector2 machinesPosition = trainsPosition + new Vector2(2 * viewWidth, 0);
                
                int drawMachinesMsgEntity = EntityFactory.Add(w); 
                DrawMachinesViewMessage mvMsg = new DrawMachinesViewMessage(
                    city.Machines.Values.ToList(), 
                    viewWidth, 
                    viewHeight, 
                    machinesPosition, 
                    10f
                ); 
                w.SetComponent<DrawMachinesViewMessage>(drawMachinesMsgEntity, mvMsg); 
            }
        }; 

        world.AddSystem(ts, tf); 
    }

}
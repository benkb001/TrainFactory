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

//required order: 
// cityClick -> push -> drawCityDetail 
public class CityClickSystem() {
    public static void Register(World world) {
        Type[] ts = [typeof(CityUI), typeof(Button), typeof(Frame), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            if (w.GetComponent<Button>(e).Clicked) {
                City city = w.GetComponent<CityUI>(e).GetCity(); 
                MakeMessage.Add<DrawCityInterfaceMessage>(w, new DrawCityInterfaceMessage(city));
                
            }
        }; 

        world.AddSystem(ts, tf); 
    }

}
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

public class EmbarkClickSystem() {
    public static void Register(World world) {
        Type[] ts = [typeof(EmbarkButton), typeof(Button), typeof(Frame), typeof(Active)]; 
        Action<World, int> tf = (w, e) => {
            if (w.GetComponent<Button>(e).Clicked) {
                EmbarkButton eb = w.GetComponent<EmbarkButton>(e); 
                Train t = eb.GetTrain();
                City c = eb.GetDestination(); 
                TrainWrap.Embark(t, c, w); 
                MakeMessage.Add<DrawCityInterfaceMessage>(w, new DrawCityInterfaceMessage(t.ComingFrom));
                //MakeMessage.Add<DrawTravelingInterfaceMessage>(w, new DrawTravelingInterfaceMessage(t));
            }
        }; 

        world.AddSystem(ts, tf); 
    }
}

public class TrainWrap {
    public static void Embark(Train t, City dest, World w) {
        t.Embark(dest, w.Time); 
    }
}
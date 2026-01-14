namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Callbacks; 

public class EnterInterfaceInteractSystem {
    public static void Register<T>(World w) where T : IInterfaceData {
        InteractSystem.Register<EnterInterfaceInteractable<T>>(w, (w, e) => {
            T data = w.GetComponent<EnterInterfaceInteractable<T>>(e).Data;
            DrawInterfaceMessage<T> dm = new DrawInterfaceMessage<T>(data);
            MakeMessage.Add<DrawInterfaceMessage<T>>(w, dm);
        }); 
    }
}
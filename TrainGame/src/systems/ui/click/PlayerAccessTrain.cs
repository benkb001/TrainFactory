namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Systems; 

//todo: test
public class PlayerAccessTrainClickSystem() {
    private static Type[] ts = [typeof(PlayerAccessTrainButton), typeof(Button), typeof(Frame), typeof(Active)]; 
    private static Action<World, int> tf = (w, e) => {
        if (w.GetComponent<Button>(e).Clicked) {
            PlayerAccessTrainButton acBtn = w.GetComponent<PlayerAccessTrainButton>(e); 
            Train t = acBtn.GetTrain();
            if (!t.HasPlayer) {
                t.HasPlayer = true; 
            } else {
                t.HasPlayer = false; 
            }
            w.SetComponent<TextBox>(e, new TextBox(acBtn.GetMessage())); 
        } 
    }; 
    public static void Register(World w) {
        w.AddSystem(ts, tf); 
    }
}
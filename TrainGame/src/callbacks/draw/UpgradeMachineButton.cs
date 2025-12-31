namespace TrainGame.Callbacks; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Constants; 
using TrainGame.Utils; 

public class DrawUpgradeMachineButtonCallback {
    public static Action Create(World w, Machine m, bool playerAtMachine, Vector2 position, float width, float height) {
        return () => {
            Color c; 
            string msg; 

            int upgradeEntity = EntityFactory.Add(w); 

            c = Colors.UIAccent; 
            msg = "Upgrade machine?"; 
            w.SetComponent<Button>(upgradeEntity, new Button(
                onClick: UpgradeMachineOnClick.Create(w, m)
            ));

            Background bg = new Background(c); 
            TextBox tb = new TextBox(msg); 
            Outline o = new Outline(); 
            Frame f = new Frame(position, width, height); 

            w.SetComponent<Background>(upgradeEntity, bg); 
            w.SetComponent<TextBox>(upgradeEntity, tb); 
            w.SetComponent<Outline>(upgradeEntity, o); 
            w.SetComponent<Frame>(upgradeEntity, f); 
        }; 
    }
}
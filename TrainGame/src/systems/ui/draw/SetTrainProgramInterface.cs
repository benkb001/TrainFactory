namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 
using TrainGame.Utils; 

public static class DrawSetTrainProgramInterfaceSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(DrawSetTrainProgramInterfaceMessage)], (w, e) => {
            SceneSystem.EnterScene(w, SceneType.ProgramInterface); 
            
            Train t = w.GetComponent<DrawSetTrainProgramInterfaceMessage>(e).GetTrain(); 

            int menuEnt = EntityFactory.Add(w); 
            w.SetComponent<Menu>(menuEnt, new Menu(train: t)); 

            Vector2 llPos = w.GetCameraTopLeft() + new Vector2(10, 10); 
            float llWidth = w.ScreenWidth - 20f; 
            float llHeight = w.ScreenHeight / 4f; 

            int llEnt = EntityFactory.Add(w); 
            LinearLayout ll = new LinearLayout("horizontal", "alignlow"); 
            ll.Padding = 5f; 

            w.SetComponent<LinearLayout>(llEnt, ll);
            w.SetComponent<Frame>(llEnt, new Frame(llPos, llWidth, llHeight)); 
            w.SetComponent<Outline>(llEnt, new Outline()); 

            foreach (string script in TAL.Scripts.Select(kvp => kvp.Key)) {
                int btnEnt = EntityFactory.Add(w); 
                LinearLayoutWrap.AddChild(btnEnt, llEnt, ll, w); 
                w.SetComponent<Button>(btnEnt, new Button()); 
                w.SetComponent<SetTrainProgramButton>(btnEnt, new SetTrainProgramButton(script, t));
                w.SetComponent<Outline>(btnEnt, new Outline()); 
                w.SetComponent<TextBox>(btnEnt, new TextBox($"Set to {script}? Requires 1 Motherboard"));
            }

            LinearLayoutWrap.ResizeChildren(llEnt, w); 
            w.RemoveEntity(e); 
        }); 
    }
}
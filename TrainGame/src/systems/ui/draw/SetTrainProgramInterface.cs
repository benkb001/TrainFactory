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
            float llHeight = w.ScreenHeight - 20f; 

            LinearLayoutContainer outerContainer = LinearLayoutWrap.Add(w, llPos, llWidth, llHeight, 
                direction: "vertical", outline: false);

            LinearLayoutContainer prewrittenRow = LinearLayoutWrap.Add(w, Vector2.Zero, 0, 0, 
                usePaging: true, childrenPerPage: 5, label: "Pre-Written Scripts", outline: true); 

            foreach (KeyValuePair<string, string> kvp in TAL.Scripts) {
                string programName = kvp.Key; 
                int btnEnt = EntityFactory.AddUI(w, Vector2.Zero, 0, 0, 
                    setButton: true, setOutline: true, text: programName);
                string program = kvp.Value; 
                string programExplanation = TAL.ScriptExplanations[programName]; 
                ViewProgramInterfaceData data = new ViewProgramInterfaceData(programName, program, programExplanation, t); 
                EnterInterfaceButton<ViewProgramInterfaceData> btn = new EnterInterfaceButton<ViewProgramInterfaceData>(data);
                w.SetComponent<EnterInterfaceButton<ViewProgramInterfaceData>>(btnEnt, btn); 
                prewrittenRow.AddChild(btnEnt, w); 
            }

            LinearLayoutContainer playerRow =  LinearLayoutWrap.Add(w, Vector2.Zero, 0, 0, 
                usePaging: true, childrenPerPage: 5, label: "Player-Written Scripts", outline: true); 
            
            List<KeyValuePair<string, string>> playerScripts = TAL.PlayerScripts.ToList(); 
            playerScripts.Add(new KeyValuePair<string, string>("new", "")); 

            foreach (KeyValuePair<string , string> kvp in playerScripts) {
                string programName = kvp.Key; 
                string program = kvp.Value; 
                int btnEnt = EntityFactory.Add(w); 
                w.SetComponent<Button>(btnEnt, new Button()); 
                WriteProgramInterfaceData d = new WriteProgramInterfaceData(t, program, programName); 
                w.SetComponent<EnterInterfaceButton<WriteProgramInterfaceData>>(btnEnt, 
                    new EnterInterfaceButton<WriteProgramInterfaceData>(d));
                w.SetComponent<Outline>(btnEnt, new Outline()); 
                w.SetComponent<TextBox>(btnEnt, new TextBox($"{programName}"));
                playerRow.AddChild(btnEnt, w); 
            }

            outerContainer.AddChild(prewrittenRow.GetParentEntity(), w); 
            outerContainer.AddChild(playerRow.GetParentEntity(), w);
            outerContainer.ResizeChildren(w, recurse: true); 
            w.RemoveEntity(e); 
        }); 
    }
}
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

            DrawSetTrainProgramInterfaceMessage dm = w.GetComponent<DrawSetTrainProgramInterfaceMessage>(e);
            Train t = dm.GetTrain(); 
            int trainEnt = dm.TrainEntity;

            int menuEnt = EntityFactory.Add(w); 
            w.SetComponent<Menu>(menuEnt, new Menu(train: t)); 

            Vector2 llPos = w.GetCameraTopLeft() + new Vector2(10, 10); 
            float llWidth = w.ScreenWidth - 20f; 
            float llHeight = w.ScreenHeight - 20f; 

            LinearLayoutContainer outerContainer = LinearLayoutContainer.Add(w, llPos, llWidth, llHeight, 
                direction: "vertical", outline: false);

            LinearLayoutContainer prewrittenRow = LinearLayoutContainer.Add(w, Vector2.Zero, 0, 0, 
                usePaging: true, childrenPerPage: 5, label: "Pre-Written Scripts", outline: true); 

            foreach (KeyValuePair<string, string> kvp in TAL.Scripts) {
                string programName = kvp.Key; 
                int btnEnt = EntityFactory.AddUI(w, Vector2.Zero, 0, 0, 
                    setButton: true, setOutline: true, text: programName);
                string program = kvp.Value; 
                string programExplanation = TAL.ScriptExplanations[programName]; 
                ViewProgramInterfaceData data = new ViewProgramInterfaceData(programName, program, programExplanation, t, trainEnt); 
                EnterInterfaceButton<ViewProgramInterfaceData> btn = new EnterInterfaceButton<ViewProgramInterfaceData>(data);
                w.SetComponent<EnterInterfaceButton<ViewProgramInterfaceData>>(btnEnt, btn); 
                prewrittenRow.AddChild(btnEnt, w); 
            }

            outerContainer.AddChild(prewrittenRow.GetParentEntity(), w); 
            outerContainer.ResizeChildren(w, recurse: true); 
            w.RemoveEntity(e); 
        }); 
    }
}

/*
(TALBody<Train, City> exe, bool hasExe) = w.GetComponentSafe<TALBody<Train, City>>(trainEnt);

if (hasExe) {
    pauseRow.AddChild(PauseTrainProgramButtonWrap.Add(w, exe));
}
*/
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

public static class DrawViewProgramInterfaceSystem {
    public static void Register(World w) {
        DrawInterfaceSystem.Register<ViewProgramInterfaceData>(w, (w, e) => {
            ViewProgramInterfaceData data = w.GetComponent<DrawInterfaceMessage<ViewProgramInterfaceData>>(e).Data; 
            LinearLayoutContainer outer = LinearLayoutWrap.AddOuter(w, data.ProgramName); 

            int explanationEnt = EntityFactory.Add(w); 
            float eHeight = w.ScreenHeight / 2; 
            float eWidth = eHeight * 2; 
            w.SetComponent<Frame>(explanationEnt, new Frame(0, 0, eWidth, eHeight)); 
            w.SetComponent<TextBox>(explanationEnt, new TextBox(data.ProgramExplanation)); 
            outer.AddChild(explanationEnt, w); 

            int btnEnt = EntityFactory.AddUI(w, Vector2.Zero, 160, 80, setButton: true, setOutline: true, 
                text: $"Set to {data.ProgramName}? Requires 1 Motherboard"); 
            w.SetComponent<SetTrainProgramButton>(btnEnt, new SetTrainProgramButton(data.ProgramName, data.GetTrain(), 
                data.Program));
            outer.AddChild(btnEnt, w); 
        
        });
    }
}
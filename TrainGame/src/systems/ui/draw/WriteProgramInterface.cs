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

public static class DrawWriteProgramInterfaceSystem {
    public static void Register(World w) {
        DrawInterfaceSystem.Register<WriteProgramInterfaceData>(w, (w, e) => {
            WriteProgramInterfaceData data = w.GetComponent<DrawInterfaceMessage<WriteProgramInterfaceData>>(e).Data; 
            
            LinearLayoutContainer llc = LinearLayoutWrap.Add(w, w.GetCameraTopLeft() + new Vector2(10, 10),
                w.ScreenWidth - 20, w.ScreenHeight - 20, direction: "vertical", align: "alignlow");
            
            TextInputContainer inputContainer = TextInputWrap.Add(w, Vector2.Zero, 
                w.ScreenWidth - 30, w.ScreenHeight - 100, data.ProgramName, data.Program, editableLabel: true); 

            llc.AddChild(inputContainer.GetParentEntity(), w); 
            
            int btnEnt = EntityFactory.AddUI(
                w, 
                Vector2.Zero, 
                160f, 
                80f, 
                setButton: true, 
                setOutline: true,
                text: $"Set to {data.ProgramName}? Requires 1 Motherboard"
            );
            w.SetComponent<SetPlayerProgramButton>(btnEnt, 
                new SetPlayerProgramButton(inputContainer.GetLabelInput(), inputContainer.GetTextInput())); 
            
            w.SetComponent<SetTrainProgramButton>(btnEnt, new SetTrainProgramButton(data.ProgramName, data.GetTrain(), 
                data.Program));
            

            llc.AddChild(btnEnt, w); 
        });
    }
}
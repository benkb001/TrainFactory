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

public static class SetTrainProgramClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<SetTrainProgramButton>(w, (w, e) => {
            SetTrainProgramButton btn = w.GetComponent<SetTrainProgramButton>(e); 

            Train t = btn.GetTrain(); 
            Inventory inv = t.ComingFrom.Inv; 
            string program = btn.Program;

            TAL.BuyTrainProgram(program, t, w);
        });
    }
}

public static class DrawSystem {
    public static void Register<T>(World w, Action<World, int> tf) {
        w.AddSystem([typeof(T)], (w, e) => {
            tf(w, e); 
            w.RemoveEntity(e);
        });
    }
}

public class SetPlayerProgramButton {
    public readonly TextInput ProgramNameInput; 
    public readonly TextInput ProgramInput;

    public SetPlayerProgramButton(TextInput ProgramNameInput, TextInput ProgramInput) {
        this.ProgramNameInput = ProgramNameInput; 
        this.ProgramInput = ProgramInput; 
    }
}

public static class SetPlayerProgramClickSystem {
    public static void Register(World w) {
        ClickSystem.Register([typeof(SetPlayerProgramButton), typeof(SetTrainProgramButton)], w, (w, e) => {
            SetPlayerProgramButton playerBtn = w.GetComponent<SetPlayerProgramButton>(e); 
            SetTrainProgramButton programBtn = w.GetComponent<SetTrainProgramButton>(e); 

            string programName = playerBtn.ProgramNameInput.Text; 
            string program = playerBtn.ProgramInput.Text; 
            TAL.PlayerScripts[programName] = program; 

            programBtn.SetProgram(programName, program);
        });
    }
}

public static class DrawWriteProgramInterfaceSystem {
    public static void Register(World w) {
        DrawSystem.Register<DrawInterfaceMessage<WriteProgramInterfaceData>>(w, (w, e) => {
            SceneSystem.EnterScene(w, SceneType.WriteProgramInterface); 
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

public class ViewProgramInterfaceData {
    private string programName; 
    private string program; 
    private string programExplanation; 
    private Train train; 
    public Train GetTrain() => train; 
    public string ProgramName => programName; 
    public string Program => program; 
    public string ProgramExplanation => programExplanation; 
    
    public ViewProgramInterfaceData(string programName, string program, string programExplanation, Train train) {
        this.train = train; 
        this.program = program; 
        this.programName = programName; 
        this.programExplanation = programExplanation; 
    }
}


public class WriteProgramInterfaceData {
    private string programName; 
    private string program; 
    private Train train; 
    public Train GetTrain() => train; 
    public string ProgramName => programName; 
    public string Program => program; 
    
    public WriteProgramInterfaceData(Train train, string program, string programName) {
        this.train = train; 
        this.program = program; 
        this.programName = programName; 
    }
}

public class EnterInterfaceButton<T> {
    public readonly T Data; 
    public EnterInterfaceButton(T Data) {
        this.Data = Data;
    }
}

public class DrawInterfaceMessage<T> {
    public readonly T Data; 
    public DrawInterfaceMessage(T Data) {
        this.Data = Data; 
    }
}

public static class EnterInterfaceClickSystem {
    public static void Register<T>(World w) {
        ClickSystem.Register<EnterInterfaceButton<T>>(w, (w, e) => {
            T data = w.GetComponent<EnterInterfaceButton<T>>(e).Data; 
            DrawInterfaceMessage<T> dm = new DrawInterfaceMessage<T>(data); 
            MakeMessage.Add<DrawInterfaceMessage<T>>(w, dm); 
        });
    }
}

public static class DrawViewProgramInterfaceSystem {
    public static void Register(World w) {
        DrawSystem.Register<DrawInterfaceMessage<ViewProgramInterfaceData>>(w, (w, e) => {
            SceneSystem.EnterScene(w, SceneType.ViewProgramInterface); 

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
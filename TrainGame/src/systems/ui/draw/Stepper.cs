namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Components; 
using TrainGame.Constants; 

public class StepperContainer {
    public int StepUpEnt; 
    public int StepDownEnt; 
    public int SubmitEnt; 
    public int StepperEnt; 
    public int ContainerEnt; 
    public Frame Container; 
    public Stepper Step;

    public StepperContainer(int upEnt, int downEnt, int subEnt, int stepEnt, int contEnt, Frame cont, Stepper step) {
        this.StepUpEnt = upEnt; 
        this.StepDownEnt = downEnt; 
        this.SubmitEnt = subEnt; 
        this.StepperEnt = stepEnt; 
        this.ContainerEnt = contEnt; 
        this.Container = cont; 
        this.Step = step; 
    }
}

public class StepperWrap {

    public static StepperContainer Draw(float width, float viewHeight, 
        string submitStr, World w, int defaultVal = 0) {
        
        int viewEntity = EntityFactory.Add(w); 
        LinearLayout ll = new LinearLayout("vertical", "alignLow"); 
        ll.Padding = 5f; 

        Frame container = new Frame(Vector2.Zero, width, viewHeight);
        w.SetComponent<Frame>(viewEntity, container); 
        w.SetComponent<Outline>(viewEntity, new Outline()); 
        w.SetComponent<LinearLayout>(viewEntity, ll); 
        
        float margin = 5f;
        float elementHeight = (viewHeight - (4 * margin)) / 4; 
        float elementWidth = width - (2 * margin); 

        int stepperEntity = EntityFactory.Add(w); 
        Stepper step = new Stepper(defaultVal);
        w.SetComponent<Stepper>(stepperEntity, step);
        w.SetComponent<Frame>(stepperEntity, new Frame(Vector2.Zero, elementWidth, elementHeight));
        w.SetComponent<Outline>(stepperEntity, new Outline()); 
        w.SetComponent<TextBox>(stepperEntity, new TextBox($"{defaultVal}"));

        int stepUpEntity = EntityFactory.Add(w); 
        List<Vector2> triangleUp = new(); 
        triangleUp.Add(new Vector2(0, 0));
        triangleUp.Add(new Vector2(elementWidth, 0));
        triangleUp.Add(new Vector2(elementWidth / 2, -elementHeight));
        w.SetComponent<Button>(stepUpEntity, new Button());
        w.SetComponent<StepperButton>(stepUpEntity, new StepperButton(stepperEntity, 1));
        w.SetComponent<Frame>(stepUpEntity, new Frame(triangleUp)); 
        w.SetComponent<Outline>(stepUpEntity, new Outline()); 

        int stepDownEntity = EntityFactory.Add(w); 
        List<Vector2> triangleDown = new(); 
        triangleDown.Add(new Vector2(0, 0));
        triangleDown.Add(new Vector2(elementWidth, 0));
        triangleDown.Add(new Vector2(elementWidth / 2, elementHeight));
        w.SetComponent<Button>(stepDownEntity, new Button()); 
        w.SetComponent<StepperButton>(stepDownEntity, new StepperButton(stepperEntity, -1));
        w.SetComponent<Frame>(stepDownEntity, new Frame(triangleDown)); 
        w.SetComponent<Outline>(stepDownEntity, new Outline()); 

        int submitEntity = EntityFactory.Add(w); 
        w.SetComponent<Button>(submitEntity, new Button()); 
        w.SetComponent<Frame>(submitEntity, new Frame(Vector2.Zero, elementWidth, elementHeight)); 
        w.SetComponent<Outline>(submitEntity, new Outline()); 
        w.SetComponent<TextBox>(submitEntity, new TextBox(submitStr));

        ll.AddChild(stepUpEntity); 
        ll.AddChild(stepperEntity); 
        ll.AddChild(stepDownEntity); 
        ll.AddChild(submitEntity); 

        return new StepperContainer(stepUpEntity, stepDownEntity, submitEntity, stepperEntity,
            viewEntity, container, step); 
    }   
}
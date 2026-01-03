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

//must be make -> push -> drawMachineRequest

public class DrawMachineRequestSystem() {
    private static Type[] ts = [typeof(DrawMachineRequestMessage)]; 
    private static Action<World, int> tf = (w, e) => {
        DrawMachineRequestMessage dm = w.GetComponent<DrawMachineRequestMessage>(e); 
        Machine m = dm.GetMachine(); 
        float width = dm.Width; 
        float height = dm.Height; 
        int viewEntity = EntityFactory.Add(w); 
        
        float viewHeight = height;
        Vector2 viewPosition = dm.Position;

        LinearLayout ll = new LinearLayout("vertical", "alignLow"); 

        w.SetComponent<Frame>(viewEntity, new Frame(viewPosition, width, viewHeight)); 
        w.SetComponent<Outline>(viewEntity, new Outline()); 
        w.SetComponent<LinearLayout>(viewEntity, ll); 
        
        float margin = dm.Margin; 
        float elementHeight = (viewHeight - (4 * margin)) / 4; 
        float elementWidth = width - (2 * margin); 

        int stepperEntity = EntityFactory.Add(w); 
        w.SetComponent<Stepper>(stepperEntity, new Stepper(0));
        w.SetComponent<Frame>(stepperEntity, new Frame(Vector2.Zero, elementWidth, elementHeight));
        w.SetComponent<Outline>(stepperEntity, new Outline()); 

        int stepUpEntity = EntityFactory.Add(w); 
        List<Vector2> triangleUp = new(); 
        triangleUp.Add(new Vector2(0, 0));
        triangleUp.Add(new Vector2(elementWidth, 0));
        triangleUp.Add(new Vector2(elementWidth / 2, -elementHeight));
        w.SetComponent<Button>(stepUpEntity, new Button());
        w.SetComponent<StepperButton>(stepUpEntity, new StepperButton(stepperEntity, m.ProductCount));
        w.SetComponent<Frame>(stepUpEntity, new Frame(triangleUp)); 
        w.SetComponent<Outline>(stepUpEntity, new Outline()); 

        int stepDownEntity = EntityFactory.Add(w); 
        List<Vector2> triangleDown = new(); 
        triangleDown.Add(new Vector2(0, 0));
        triangleDown.Add(new Vector2(elementWidth, 0));
        triangleDown.Add(new Vector2(elementWidth / 2, elementHeight));
        w.SetComponent<Button>(stepDownEntity, new Button()); 
        w.SetComponent<StepperButton>(stepDownEntity, new StepperButton(stepperEntity, -m.ProductCount));
        w.SetComponent<Frame>(stepDownEntity, new Frame(triangleDown)); 
        w.SetComponent<Outline>(stepDownEntity, new Outline()); 

        int submitEntity = EntityFactory.Add(w); 
        w.SetComponent<Button>(submitEntity, new Button()); 
        w.SetComponent<MachineRequestButton>(submitEntity, new MachineRequestButton(m, stepperEntity)); 
        w.SetComponent<Frame>(submitEntity, new Frame(Vector2.Zero, elementWidth, elementHeight)); 
        w.SetComponent<Outline>(submitEntity, new Outline()); 
        w.SetComponent<TextBox>(submitEntity, new TextBox("Craft"));

        ll.AddChild(stepUpEntity); 
        ll.AddChild(stepperEntity); 
        ll.AddChild(stepDownEntity); 
        ll.AddChild(submitEntity); 

        w.RemoveEntity(e); 
    }; 

    public static void Register(World w) {
        w.AddSystem(ts, tf); 
    }
}
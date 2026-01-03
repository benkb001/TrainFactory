namespace TrainGame.Callbacks; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 
using TrainGame.Utils; 

public class DrawMachineRequestCallback {
    public static void Draw(World w, Machine m, Vector2 Position, float width, float height,
        float margin = 5f, bool SetMenu = true) {
        int viewEntity = EntityFactory.Add(w); 
            
            float viewHeight = height;
            Vector2 viewPosition = Position;

            LinearLayout ll = new LinearLayout("vertical", "alignLow"); 

            w.SetComponent<Frame>(viewEntity, new Frame(viewPosition, width, viewHeight)); 
            w.SetComponent<Outline>(viewEntity, new Outline()); 
            w.SetComponent<LinearLayout>(viewEntity, ll); 
            
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
    }

    public static void Create(World w, Machine m, Vector2 Position, float width, float height, 
        float margin = 5f, bool SetMenu = true) {
        int e = EntityFactory.Add(w); 
        w.SetComponent<DrawCallback>(e, new DrawCallback(() => {
            Draw(w, m, Position, width, height, margin, SetMenu); 
        })); 
    }
}
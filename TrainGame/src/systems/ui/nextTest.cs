namespace TrainGame.Systems; 

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

public static class NextDrawTestUISystem {
    private static Type[] ts = [typeof(NextDrawTestControl)]; 
    private static int nextTestButtonX = 50; 
    private static int nextTestButtonY = 400; 

    private static void AddNextTestButton(World w, int test) {
        int nextTestButton = w.AddButton(
            nextTestButtonX, 
            nextTestButtonY, 
            100, 
            AspectRatio.Button, 
            Depth.NextTestButton, 
            w.GetTexture(Textures.Button)
        ); 
        
        w.SetComponent<NextDrawTestButton>(nextTestButton, new NextDrawTestButton(test)); 
    }

    private static Dictionary<int, Action<World>> actions = new() {
        [1] = (w) => { 
            AddNextTestButton(w, 1); 

            int label = w.AddEntity(); 
            w.SetComponent<Frame>(label, new Frame(300, 300, 100, 50)); 
            w.SetComponent<Outline>(label, new Outline(10, Color.White)); 
            w.SetComponent<Message>(label, new Message("This should stick out of a white box above the button. Click to skip to last test"));
            w.SetComponent<Button>(label, new Button());
            w.SetComponent<NextDrawTestButton>(label, new NextDrawTestButton(13)); 

        }, 
        [2] = (w) => {
            AddNextTestButton(w, 2); 

            int spr1 = w.AddEntity(); 
            w.SetComponent<Sprite>(spr1, new Sprite(w.GetTexture(Textures.Button), 0));
            w.SetComponent<Frame>(spr1, new Frame(100, 100, 100, 100 * AspectRatio.Button));
            
            int spr2 = w.AddEntity(); 
            w.SetComponent<Sprite>(spr2, new Sprite(w.GetTexture(Textures.Button), 1));
            w.SetComponent<Frame>(spr2, new Frame(150, 125, 100, 100 * AspectRatio.Button));

            int label = w.AddEntity(); 
            w.SetComponent<Frame>(label, new Frame(300, 300, 100, 50)); 
            w.SetComponent<Message>(label, new Message("The lower button should be drawn ontop of the higher one"));
        }, 
        [3] = (w) => {
            AddNextTestButton(w, 3); 

            int spr1 = w.AddEntity(); 
            w.SetComponent<Sprite>(spr1, new Sprite(w.GetTexture(Textures.Button), 0));
            w.SetComponent<Frame>(spr1, new Frame(50, 100, 100, 100 * AspectRatio.Button));
            
            int spr2 = w.AddEntity(); 
            w.SetComponent<Sprite>(spr2, new Sprite(w.GetTexture(Textures.Button), 1));
            w.SetComponent<Frame>(spr2, new Frame(200, 100, 50, 50 * AspectRatio.Button));

            int label = w.AddEntity(); 
            w.SetComponent<Frame>(label, new Frame(300, 300, 100, 50)); 
            w.SetComponent<Message>(label, new Message("The left button should be twice as wide as the right"));
        }, 
        [4] = (w) => {
            AddNextTestButton(w, 4);

            int spr1 = w.AddEntity(); 
            w.SetComponent<Sprite>(spr1, new Sprite(w.GetTexture(Textures.Button), 0));

            Frame f1 = new Frame(50, 100, 100, 100 * AspectRatio.Button); 
            f1.SetRotation(30f * (3.14f / 180f)); 
            w.SetComponent<Frame>(spr1, f1);
            
            int spr2 = w.AddEntity(); 
            w.SetComponent<Sprite>(spr2, new Sprite(w.GetTexture(Textures.Button), 1));
            
            Frame f2 = new Frame(200, 100, 100, 100 * AspectRatio.Button); 
            f2.SetRotation(90f * (3.14f / 180f));
            w.SetComponent<Frame>(spr2, f2);

            int label = w.AddEntity(); 
            w.SetComponent<Frame>(label, new Frame(300, 300, 100, 50)); 
            w.SetComponent<Message>(label, new Message("The left button should be angled 30, right 90"));
        }, 
        [5] = (w) => {
            AddNextTestButton(w, 5); 
            int box = w.AddEntity(); 
            w.SetComponent<Frame>(box, new Frame(0, 0, 100, 100)); 
            w.SetComponent<TextBox>(box, new TextBox("Test init"));
            w.SetComponent<Outline>(box, new Outline()); 

            int box2 = w.AddEntity(); 
            w.SetComponent<Frame>(box2, new Frame(0, 100, 100, 100)); 
            w.SetComponent<Outline>(box2, new Outline()); 
            string test = "This long long long long long long string should be squeezed"; 
            w.SetComponent<TextBox>(box2, new TextBox(test));

            int box3 = w.AddEntity(); 
            w.SetComponent<Frame>(box3, new Frame(0, 200, 50, 200)); 
            TextBox tb = new TextBox("This box should have some padding and red text"); 
            tb.Padding = 10f;
            tb.TextColor = Color.Red; 
            w.SetComponent<TextBox>(box3, tb);
            w.SetComponent<Outline>(box3, new Outline()); 
        }, 
        [6] = (w) => {
            AddNextTestButton(w, 6); 
            LinearLayout linearLayout = new LinearLayout("horizontal", "alignLow"); 
            linearLayout.Padding = 5f; 
            int e = w.AddEntity(); 
            w.SetComponent<Frame>(e, new Frame(0, 0, 600, 150)); 
            w.SetComponent<Outline>(e, new Outline()); 
            w.SetComponent<LinearLayout>(e, linearLayout);

            int c1 = w.AddEntity(); 
            w.SetComponent<Frame>(c1, new Frame(0, 0, 100, 100)); 
            w.SetComponent<Outline>(c1, new Outline()); 
            w.SetComponent<TextBox>(c1, new TextBox("This box should be at left of a horziontal container")); 

            int c2 = w.AddEntity(); 
            w.SetComponent<Frame>(c2, new Frame(0, 0, 100, 100)); 
            w.SetComponent<Outline>(c2, new Outline()); 
            w.SetComponent<TextBox>(c2, new TextBox("This box should be to the right, with some padding between")); 
            linearLayout.AddChild(c1); 
            linearLayout.AddChild(c2); 

            LinearLayout ll2 = new LinearLayout("horizontal", "alignHigh"); 
            ll2.Padding = 5f; 
            int e2 = w.AddEntity(); 
            w.SetComponent<Frame>(e2, new Frame(0, 160, 600, 150)); 
            w.SetComponent<Outline>(e2, new Outline()); 
            w.SetComponent<LinearLayout>(e2, ll2);

            int c3 = w.AddEntity(); 
            w.SetComponent<Frame>(c3, new Frame(0, 0, 100, 100)); 
            w.SetComponent<Outline>(c3, new Outline()); 
            w.SetComponent<TextBox>(c3, new TextBox("This box should be at right of a horizontal container")); 

            int c4 = w.AddEntity(); 
            w.SetComponent<Frame>(c4, new Frame(0, 0, 100, 100)); 
            w.SetComponent<Outline>(c4, new Outline()); 
            w.SetComponent<TextBox>(c4, new TextBox("This box should be to the left, with some padding between")); 
            ll2.AddChild(c3); 
            ll2.AddChild(c4); 

            LinearLayout ll3 = new LinearLayout("vertical", "alignLow"); 
            ll3.Padding = 5f; 
            int e3 = w.AddEntity(); 
            w.SetComponent<Frame>(e3, new Frame(610, 10, 100, 400)); 
            w.SetComponent<Outline>(e3, new Outline()); 
            w.SetComponent<LinearLayout>(e3, ll3);

            int c5 = w.AddEntity(); 
            w.SetComponent<Frame>(c5, new Frame(0, 0, 100, 100)); 
            w.SetComponent<Outline>(c5, new Outline()); 
            w.SetComponent<TextBox>(c5, new TextBox("This box should be at top of a vertical container")); 

            int c6 = w.AddEntity(); 
            w.SetComponent<Frame>(c6, new Frame(0, 0, 100, 100)); 
            w.SetComponent<Outline>(c6, new Outline()); 
            w.SetComponent<TextBox>(c6, new TextBox("This box should be below, with some padding between")); 
            ll3.AddChild(c5); 
            ll3.AddChild(c6); 

            LinearLayout ll4 = new LinearLayout("vertical", "alignHigh"); 
            ll4.Padding = 5f; 
            int e4 = w.AddEntity(); 
            w.SetComponent<Frame>(e4, new Frame(720, 10, 100, 400)); 
            w.SetComponent<Outline>(e4, new Outline()); 
            w.SetComponent<LinearLayout>(e4, ll4);

            int c7 = w.AddEntity(); 
            w.SetComponent<Frame>(c7, new Frame(0, 0, 100, 100)); 
            w.SetComponent<Outline>(c7, new Outline()); 
            w.SetComponent<TextBox>(c7, new TextBox("This box should be at bottom of a vertical container")); 

            int c8 = w.AddEntity(); 
            w.SetComponent<Frame>(c8, new Frame(0, 0, 100, 100)); 
            w.SetComponent<Outline>(c8, new Outline()); 
            w.SetComponent<TextBox>(c8, new TextBox("This box should be above, with some padding between")); 
            ll4.AddChild(c7); 
            ll4.AddChild(c8); 
        }, 
        [7] = (w) => {
            int entity_follow = w.AddEntity(); 
            w.SetComponent<Frame>(entity_follow, new Frame(0, 0, 100, 100)); 
            w.SetComponent<Outline>(entity_follow, new Outline()); 
            w.SetComponent<TextBox>(entity_follow, new TextBox("This box should stay at the center. Click to continue")); 
            w.SetComponent<Velocity>(entity_follow, new Velocity(5f, 0f)); 
            w.SetComponent<Button>(entity_follow, new Button()); 
            w.SetComponent<NextDrawTestButton>(entity_follow, new NextDrawTestButton(7)); 
            
            //necessary to match movement system signature, consider changing? 
            w.SetComponent<Collidable>(entity_follow, Collidable.Get()); 
            w.TrackEntity(entity_follow); 
            
            int e_other = w.AddEntity(); 
            w.SetComponent<Frame>(e_other, new Frame(0, 100, 100, 100)); 
            w.SetComponent<TextBox>(e_other, new TextBox("This box should be moving to the left")); 
        }, 
        [8] = (w) => {
            w.SetTargetCameraPositionToScreenOrigin();
            AddNextTestButton(w, 8); 
            int entity_clock = w.AddEntity(); 
            w.SetComponent<Frame>(entity_clock, new Frame(0, 0, 100, 100));
            w.SetComponent<Outline>(entity_clock, new Outline()); 
            w.SetComponent<TextBox>(entity_clock, new TextBox("")); 
            w.SetComponent<GameClockView>(entity_clock, GameClockView.Get()); 

            int label = w.AddEntity(); 
            w.SetComponent<Frame>(label, new Frame(0, 100, 100, 100)); 
            w.SetComponent<TextBox>(label, new TextBox("The above should be counting up, it's pretty slow tho give it a sec")); 
        }, 
        [9] = (w) => {
            AddNextTestButton(w, 9); 
            int toast = w.AddEntity(); 
            w.SetComponent<Frame>(toast, new Frame(0, 0, 100, 100)); 
            w.SetComponent<Outline>(toast, new Outline()); 
            w.SetComponent<TextBox>(toast, new TextBox("This text should be fading awayy")); 
            w.SetComponent<Toast>(toast, new Toast()); 
        }, 
        [10] = (w) => {
            AddNextTestButton(w, 10); 
            int player = w.AddEntity(); 
            w.SetComponent<Frame>(player, new Frame(0, 0, 100, 100)); 
            w.SetComponent<Outline>(player, new Outline()); 
            w.SetComponent<TextBox>(player, new TextBox("This should move around with WASD or configured movement keys")); 
            w.SetComponent<CardinalMovement>(player, new CardinalMovement(2f)); 
            w.SetComponent<Collidable>(player, Collidable.Get());
            
            int label = w.AddEntity(); 
            w.SetComponent<Frame>(label, new Frame(300, 0, 100, 100)); 
            w.SetComponent<TextBox>(label, new TextBox("This box should be staying still")); 
        }, 
        [11] = (w) => {
            int label = w.AddEntity(); 
            w.SetComponent<Frame>(label, new Frame(200, 200, 100, 100)); 
            w.SetComponent<Outline>(label, new Outline()); 
            w.SetComponent<TextBox>(label, new TextBox("Click the triangle above this box to continue")); 
            
            int triangle = w.AddEntity(); 

            List<Vector2> points = new(); 
            points.Add(new Vector2(200, 190)); 
            points.Add(new Vector2(300, 190)); 
            points.Add(new Vector2(250, 90)); 

            w.SetComponent<Frame>(triangle, new Frame(points)); 
            w.SetComponent<Outline>(triangle, new Outline()); 
            w.SetComponent<Button>(triangle, new Button()); 
            w.SetComponent<NextDrawTestButton>(triangle, new NextDrawTestButton(11)); 
        }, 
        [12] = (w) => {
            int label = w.AddEntity(); 
            w.SetComponent<Frame>(label, new Frame(0, 0, 180, 180)); 
            w.SetComponent<Outline>(label, new Outline()); 
            w.SetComponent<TextBox>(label, new TextBox("Clicking the above arrow should make it count up by 1 and vice-versa for down"));

            int up = w.AddEntity(); 

            int stepper = w.AddEntity(); 
            w.SetComponent<Stepper>(stepper, new Stepper(0)); 
            w.SetComponent<Frame>(stepper, new Frame(200, 200, 100, 100)); 
            w.SetComponent<TextBox>(stepper, new TextBox("0")); 
            w.SetComponent<Outline>(stepper, new Outline());

            List<Vector2> points = new(); 
            points.Add(new Vector2(200, 190)); 
            points.Add(new Vector2(300, 190)); 
            points.Add(new Vector2(250, 90)); 
            w.SetComponent<Frame>(up, new Frame(points));
            w.SetComponent<StepperButton>(up, new StepperButton(stepper, 1)); 
            w.SetComponent<Button>(up, new Button()); 
            w.SetComponent<Outline>(up, new Outline()); 

            List<Vector2> ps2 = new(); 
            ps2.Add(new Vector2(200, 310)); 
            ps2.Add(new Vector2(300, 310)); 
            ps2.Add(new Vector2(250, 410)); 

            int down = w.AddEntity(); 

            w.SetComponent<Frame>(down, new Frame(ps2));
            w.SetComponent<StepperButton>(down, new StepperButton(stepper, -1)); 
            w.SetComponent<Button>(down, new Button()); 
            w.SetComponent<Outline>(down, new Outline()); 

            AddNextTestButton(w, 12); 
        }, 
        [13] = (w) => {
            AddNextTestButton(w, 13); 
            int drag = w.AddEntity(); 
            Frame f = new Frame(0, 0, 200, 200); 
            w.SetComponent<Frame>(drag, f); 
            w.SetComponent<Draggable>(drag, new Draggable(f.Position)); 
            w.SetComponent<Button>(drag, new Button()); 
            w.SetComponent<Outline>(drag, new Outline()); 
            w.SetComponent<TextBox>(drag, new TextBox("This box should be able to be clicked and dragged, and snap back to top left corner"));
        }, 
        [14] = (w) => {
            int outerButton = w.AddEntity(); 
            w.SetComponent<Frame>(outerButton, new Frame(0, 0, 400, 400)); 
            w.SetComponent<Outline>(outerButton, new Outline()); 
            w.SetComponent<TextBox>(outerButton, new TextBox("Clicking on this box, outside the inner box, should advance to the next step"));
            w.SetComponent<Button>(outerButton, new Button(Depth: 0)); 
            w.SetComponent<NextDrawTestButton>(outerButton, new NextDrawTestButton(14)); 

            int innerButton = w.AddEntity(); 
            w.SetComponent<Frame>(innerButton, new Frame(100, 100, 100, 100)); 
            w.SetComponent<Outline>(innerButton, new Outline()); 
            w.SetComponent<Button>(innerButton, new Button(Depth: -1)); 
            w.SetComponent<TextBox>(innerButton, new TextBox("Clicking inside the inner box should do nothing")); 

            AddNextTestButton(w, 14); 
        }, 
        [15] = (w) => {
            int label = w.AddEntity(); 
            w.SetComponent<Frame>(label, new Frame(0, 0, 100, 100)); 
            w.SetComponent<Draggable>(label, new Draggable(Vector2.Zero)); 
            w.SetComponent<TextBox>(label, new TextBox("This should be draggable only before the pause button is clicked")); 
            w.SetComponent<Button>(label, new Button()); 

            int pauseButton = w.AddEntity(); 
            w.SetComponent<Frame>(pauseButton, new Frame(0, 200, 100, 100)); 
            w.SetComponent<Button>(pauseButton, new Button()); 
            w.SetComponent<PauseButton>(pauseButton, PauseButton.Get()); 
            w.SetComponent<TextBox>(pauseButton, new TextBox("Pause")); 
            AddNextTestButton(w, 15); 
        }, 
        [16] = (w) => {
            int inv = w.AddEntity(); 
            Inventory i = new Inventory("Test", 2, 2);
            i.Add(new Inventory.Item(ItemId: "Apple", Count: 1)); 
            i.Add(new Inventory.Item(ItemId: "Apple", Count: 1), 1, 0); 
            w.SetComponent<Inventory>(inv, i); 

            int msg = w.AddEntity(); 
            w.SetComponent<DrawInventoryMessage>(msg, new DrawInventoryMessage(100, 100, Vector2.Zero, i, inv, 5f));

            int other_inv_entity = w.AddEntity(); 
            Inventory inv_other = new Inventory("Other", 2, 2); 
            int other_msg = w.AddEntity(); 
            w.SetComponent<Inventory>(other_inv_entity, inv_other); 
            w.SetComponent<DrawInventoryMessage>(other_msg, new DrawInventoryMessage(100, 100, new Vector2(0, 100), inv_other, other_inv_entity, 5f)); 
            
        }
    };

    public static void Register(World world) {
        Action<World, int> tf = (w, e) => {
            int curTest = w.GetComponent<NextDrawTestControl>(e).GetCurTest();
             
            w.Clear(); 
            actions[curTest](w); 
        };

        world.AddSystem(ts, tf);
    }
}
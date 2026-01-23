namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.Components; 
using TrainGame.ECS;
using TrainGame.Utils;
using TrainGame.Constants;

public static class TextInputSystem {

    public static void RegisterActivate(World w) {
        ClickSystem.Register<TextInput>(w, (w, e) => {
            w.GetComponent<TextInput>(e).Active = true; 
        }); 
    }

    public static void RegisterDeactivate(World w) {
        w.AddSystem((w) => {
            if (VirtualMouse.LeftClicked()) {
                List<int> es = w.GetMatchingEntities([typeof(TextInput), typeof(Frame), typeof(Active)]); 
                foreach (int e in es) {
                    Frame f = w.GetComponent<Frame>(e); 
                    TextInput tInput = w.GetComponent<TextInput>(e); 
                    if (!f.Contains(w.GetWorldMouseCoordinates())) {
                        tInput.Active = false;
                    }
                }
            }
        });  
    }

    public static void RegisterType(World w) {
        w.AddSystem([typeof(TextInput), typeof(Active)], (w, e) => {
            TextInput tIn = w.GetComponent<TextInput>(e); 
            Outline cursorOutline = w.GetComponent<Outline>(tIn.CursorEnt);

            if (tIn.Active) {
                tIn.IncrementCursorVisibility(); 

                if (tIn.CursorIsVisible()) {
                    cursorOutline.SetColor(Color.White); 
                } else {
                    cursorOutline.SetColor(Color.Transparent); 
                }

                bool changed = false; 
                bool movedCursor = false; 
                bool shift = VirtualKeyboard.IsPressed(Keys.LeftShift);

                foreach (Keys k in KeyBinds.AlphaList) {
                    if (VirtualKeyboard.IsClicked(k)) {
                        string s = k.ToString(); 
                        if (!shift) {
                            s = s.ToLower(); 
                        }
                        tIn.AddChar(s); 
                        changed = true; 
                    }
                }

                foreach (KeyValuePair<Keys, string> kvp in KeyBinds.StringMap) {
                    if (VirtualKeyboard.IsClicked(kvp.Key)) {
                        tIn.AddChar(kvp.Value); 
                        changed = true; 
                    }
                }

                foreach (KeyValuePair<(Keys, bool), string> kvp in KeyBinds.StringMapShift.Where(
                    kvp => kvp.Key.Item2 == shift
                )) {
                    if (VirtualKeyboard.IsClicked(kvp.Key.Item1)) {
                        tIn.AddChar(kvp.Value); 
                        changed = true; 
                    }
                }

                if (VirtualKeyboard.IsClicked(Keys.Back)) {
                    tIn.DeleteChar(); 
                    changed = true; 
                }

                if (VirtualKeyboard.IsClicked(Keys.Left)) {
                    tIn.CursorLeft(); 
                    movedCursor = true; 
                }

                if (VirtualKeyboard.IsClicked(Keys.Right)) {
                    tIn.CursorRight(); 
                    movedCursor = true; 
                }

                if (VirtualKeyboard.IsClicked(Keys.Down)) {
                    tIn.CursorDown(); 
                    movedCursor = true; 
                }

                if (VirtualKeyboard.IsClicked(Keys.Up)) {
                    tIn.CursorUp(); 
                    movedCursor = true; 
                }

                tIn.Changed = changed; 
                tIn.MovedCursor = movedCursor; 
            } else {
                cursorOutline.SetColor(Color.Transparent); 
            }
        }); 
    }
    
    public static void RegisterFormat(World w) {
        w.AddSystem([typeof(TextInput), typeof(LinearLayout), typeof(Frame), typeof(Active)], (w, e) => {
            TextInput tIn = w.GetComponent<TextInput>(e); 
            bool changed = tIn.Changed; 
            
            if (changed || tIn.MovedCursor) {
                
                tIn.SetLinesFromText(); 
                List<string> lines = tIn.Lines; 

                LinearLayout ll = w.GetComponent<LinearLayout>(e); 
                Frame frame = w.GetComponent<Frame>(e); 
                float lineWidth = frame.GetWidth(); 
                float lineHeight = frame.GetHeight() / ll.ChildrenPerPage;

                while (ll.PagedChildren.Count < lines.Count + 1) {
                    int curLineEnt = EntityFactory.Add(w); 
                    w.SetComponent<Frame>(curLineEnt, new Frame(Vector2.Zero, lineWidth, lineHeight)); 
                    w.SetComponent<TextBox>(curLineEnt, new TextBox("")); 
                    LinearLayoutWrap.AddChild(curLineEnt, e, ll, w);
                }

                for (int j = 0; j < lines.Count; j++) {
                    w.GetComponent<TextBox>(ll.PagedChildren[j]).Text = lines[j]; 
                }

                int lineIndex = lines.Count; 

                while (lineIndex < ll.PagedChildren.Count) {
                    w.GetComponent<TextBox>(ll.PagedChildren[lineIndex]).Text = ""; 
                    lineIndex++; 
                }

                (int cursorCol, int cursorRow) = tIn.CursorCoordinates;

                //TODO: the manual scaling here is actually a bug resulting 
                //from not using the textbox scale on measure string, but 
                //for now this is fine. It would be fixed if we ensured 
                //paging didnt resize row height to scale down text, then 
                //could use regular measuring like in RegisterCopy below

                if (cursorRow >= 0 && cursorRow < ll.PagedChildren.Count) {
                    int cursorLineEnt = ll.PagedChildren[cursorRow];
                    Vector2 linePosition = w.GetComponent<Frame>(cursorLineEnt).Position; 
                    string cursorLineStr = w.GetComponent<TextBox>(cursorLineEnt).Text; 
                    int col = Math.Min(cursorCol, cursorLineStr.Length); 
                    col = Math.Max(0, col); 
                    Vector2 baseChar = w.MeasureString("A");
                    float charWidth = baseChar.X - 2.75f;
                    float charHeight = baseChar.Y; 
                    float strWidth = charWidth * (cursorLineStr.Substring(0, col).Length); 
                    Vector2 cursorPosition = linePosition + new Vector2(strWidth, charHeight / 8f); 
                    w.GetComponent<Frame>(tIn.CursorEnt).SetCoordinates(cursorPosition);  
                }
            }

            tIn.Changed = false; 
        }); 
    }

    public static void RegisterCopy(World w) {
        w.AddSystem([typeof(TextBox), typeof(Frame), typeof(TextInput), typeof(Active)], (w, e) => {
            TextBox tb = w.GetComponent<TextBox>(e); 
            TextInput input = w.GetComponent<TextInput>(e); 
            tb.Text = input.Text; 
            Frame cursorFrame = w.GetComponent<Frame>(input.CursorEnt); 
            Frame tbFrame = w.GetComponent<Frame>(e); 
            int cIndex = Math.Min(input.CursorIndex, tb.Text.Length);
            Vector2 measure = w.MeasureString(tb.Text.Substring(0, cIndex));
            cursorFrame.SetCoordinates(tbFrame.Position + new Vector2(measure.X, measure.Y / 8f)); 
        });
    }
}
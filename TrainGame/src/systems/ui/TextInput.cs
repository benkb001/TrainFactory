namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.Components; 
using TrainGame.ECS;
using TrainGame.Utils;
using TrainGame.Constants;

public static class TextInputSystem {
    private static List<string> format(string word) {
        int i = 0; 
        List<string> words = new(); 
        string cur = ""; 
        while (i < word.Length) {
            if (word[i] == '\n') {
                if (cur != "") {
                    words.Add(cur); 
                }
                words.Add("\n");
                cur = ""; 
            } else {
                cur += word[i]; 
            }
            i++;
        }

        if (cur != "") {
            words.Add(cur); 
        }

        return words;
    }

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
        w.AddSystem([typeof(TextInput), typeof(LinearLayout), typeof(Frame), typeof(Active)], (w, e) => {
            TextInput tIn = w.GetComponent<TextInput>(e); 
            tIn.IncrementVisibility(); 
            if (tIn.Active) {
                bool changed = false; 
                foreach (Keys k in KeyBinds.AlphaList) {
                    if (VirtualKeyboard.IsClicked(k)) {
                        string s = k.ToString(); 
                        if (!VirtualKeyboard.IsPressed(Keys.LeftShift)) {
                            s = s.ToLower(); 
                        }
                        tIn.AddChar(s); 
                        changed = true; 
                    }
                }

                if (VirtualKeyboard.IsClicked(Keys.Back)) {
                    tIn.DeleteChar(); 
                    changed = true; 
                }

                if (VirtualKeyboard.IsClicked(Keys.Enter)) {
                    tIn.AddChar("\n");
                    changed = true; 
                }

                if (VirtualKeyboard.IsClicked(Keys.Space)) {
                    tIn.AddChar(" ");
                    changed = true; 
                }

                if (VirtualKeyboard.IsClicked(Keys.Left)) {
                    tIn.CursorLeft(); 
                }

                if (VirtualKeyboard.IsClicked(Keys.Right)) {
                    tIn.CursorRight(); 
                }

                if (VirtualKeyboard.IsClicked(Keys.Down)) {
                    tIn.CursorDown(); 
                }

                if (VirtualKeyboard.IsClicked(Keys.Up)) {
                    tIn.CursorUp(); 
                }

                if (changed) {
                    string text = tIn.Text; 
                    string[] words = text.Split(" "); 
                    words = words.Aggregate(
                        seed: new List<string>(),
                        func: (acc, word) => {
                            foreach (string cur in format(word)) {
                                acc.Add(cur); 
                            }
                            return acc; 
                        },
                        resultSelector: acc => acc.ToArray()
                    );

                    for (int j = 0; j < words.Length; j++) {
                        words[j] = words[j].Substring(0, Math.Min(words[j].Length, tIn.CharsPerRow)); 
                    }

                    LinearLayout ll = w.GetComponent<LinearLayout>(e); 
                    Frame frame = w.GetComponent<Frame>(e); 
                    float lineWidth = frame.GetWidth(); 
                    float lineHeight = frame.GetHeight() / ll.ChildrenPerPage;
                    int i = 0; 
                    int lineIndex = 0; 

                    while (i < words.Length) {
                        string line = ""; 
                        if (words[i] == "\n") {
                            lineIndex++; 
                            i++;
                            continue; 
                        }

                        while (i < words.Length && (line + words[i]).Length <= tIn.CharsPerRow && words[i] != "\n") {
                            string word = words[i]; 
                            line += word;
                            line += " "; 
                            i++;
                        }
                        
                        while (ll.PagedChildren.Count <= lineIndex) {
                            int curLineEnt = EntityFactory.Add(w); 
                            w.SetComponent<Frame>(curLineEnt, new Frame(Vector2.Zero, lineWidth, lineHeight)); 
                            w.SetComponent<TextBox>(curLineEnt, new TextBox("")); 
                            LinearLayoutWrap.AddChild(curLineEnt, e, ll, w);
                        }

                        int lineEnt = ll.PagedChildren[lineIndex]; 
                        w.GetComponent<TextBox>(lineEnt).Text = line; 
                        lineIndex++; 
                    }
                }
            }
        });
    }
}
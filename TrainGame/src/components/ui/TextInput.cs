namespace TrainGame.Components;

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

public class TextInput {
    private string text; 
    private int cursorIndex; 
    private int cursorVisibilityFrame = 0; 
    private int charsPerRow; 
    public string Text => text; 
    public int CharsPerRow => charsPerRow; 
    public int CursorIndex => cursorIndex; 

    private static int cursorFramesVisible = 30; 

    public bool Active = false;

    public TextInput(int charsPerRow) {
        text = ""; 
        cursorIndex = 0; 
        this.charsPerRow = charsPerRow; 
    }

    public void AddChar(string s) {
        text = text.Insert(cursorIndex, s); 
        cursorIndex++; 
    }

    public void DeleteChar() {
        if (cursorIndex > 0 && text.Length > 0) {
            text = text.Remove(cursorIndex - 1, 1);
            cursorIndex--; 
        }
    }

    public bool IsVisible() {
        return (cursorVisibilityFrame / cursorFramesVisible) > 0; 
    }

    public void IncrementVisibility() {
        cursorVisibilityFrame = (cursorVisibilityFrame + 1) % (cursorFramesVisible * 2); 
    }

    private void changeCursor(int delta) {
        cursorIndex += delta; 
        cursorIndex = Math.Max(0, cursorIndex); 
        cursorIndex = Math.Min(cursorIndex, text.Length); 
    }

    public void CursorLeft() {
        changeCursor(-1); 
    }

    public void CursorRight() {
        changeCursor(1); 
    }

    public void CursorUp() {
        changeCursor(-charsPerRow); 
    }

    public void CursorDown() {
        changeCursor(charsPerRow); 
    }
}

public static class TextInputWrap {

    public static void AddTextInput(World w, Vector2 position, float width, float height) {
        int e = EntityFactory.Add(w); 
        int childrenPerPage = GetChildrenPerPage(w, height); 
        LinearLayoutContainer llc = LinearLayoutWrap.Add(w, position, width, height, usePaging: true, 
            childrenPerPage: childrenPerPage, direction: "vertical", align: "alignlow", padding: 0f, 
            outline: true);
        TextInput input = new TextInput(GetCharsPerRow(w, width)); 
        int llEnt = llc.LLEnt; 
        w.SetComponent<TextInput>(llEnt, input); 
        w.SetComponent<Button>(llEnt, new Button()); 
    }

    public static int GetCharsPerRow(World w, float width) {
        string s = ""; 
        width = Math.Min(width, w.ScreenWidth); 
        while (w.MeasureString(s).X < width) {
            s += "A"; 
        }
        return s.Length; 
    }

    public static int GetChildrenPerPage(World w, float height) {
        float oneCharHeight = w.MeasureString("A").Y; 
        return (int)(height / oneCharHeight); 
    }
}
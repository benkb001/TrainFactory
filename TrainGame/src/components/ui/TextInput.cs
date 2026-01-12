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
    private (int, int) cursorCoordinates = (0, 0);  
    private int cursorVisibilityFrame = 0; 
    private int charsPerRow; 
    private int cursorEnt = -1; 
    public string Text => text; 
    public int CharsPerRow => charsPerRow; 
    public int CursorIndex => cursorIndex; 
    public bool Changed = true; 
    public bool MovedCursor = false; 
    public (int, int) CursorCoordinates => cursorCoordinates; 
    public List<string> Lines = new(); 
    public int CursorEnt => cursorEnt; 

    private static int cursorFramesVisible = 30; 

    public bool Active = false;

    public TextInput(int charsPerRow = 0, string defaultText = "", int cursorEnt = -1) {
        text = defaultText;
        cursorIndex = defaultText.Length; 
        this.charsPerRow = charsPerRow;
        this.cursorEnt = cursorEnt; 
    }

    public void AddChar(string s) {
        text = text.Insert(cursorIndex, s); 
        cursorIndex++; 
        /*
        if (cursorCoordinates.Item1 < Lines.Count) {

            Lines[cursorCoordinates.Item1].Insert(cursorCoordinates.Item2, s); 
        }
        
        setCursorCoordinatesFromIndex(); 
        */
    }

    public void DeleteChar() {
        if (cursorIndex > 0 && text.Length > 0) {
            text = text.Remove(cursorIndex - 1, 1);
            cursorIndex--; 
            setCursorCoordinatesFromIndex();
        }
    }

    public bool IsVisible() {
        return (cursorVisibilityFrame / cursorFramesVisible) > 0; 
    }

    public void IncrementVisibility() {
        cursorVisibilityFrame = (cursorVisibilityFrame + 1) % (cursorFramesVisible * 2); 
    }

    public void SynchronizeCursor() {
        setCursorCoordinatesFromIndex();
    }

    private void changeCursor(int delta) {
        cursorIndex += delta; 
        cursorIndex = Math.Max(0, cursorIndex); 
        cursorIndex = Math.Min(cursorIndex, text.Length); 
        setCursorCoordinatesFromIndex(); 
    }

    public void CursorLeft() {
        changeCursor(-1); 
    }

    public void CursorRight() {
        changeCursor(1); 
    }

    public void CursorUp() {
        cursorCoordinates.Item2 = Math.Max(0, cursorCoordinates.Item2 - 1); 
        if (cursorCoordinates.Item2 < Lines.Count && cursorCoordinates.Item2 > -1) {
            cursorCoordinates.Item1 = Math.Min(cursorCoordinates.Item1, Lines[cursorCoordinates.Item2].Length); 
        }
        setCursorIndexFromCoordinates();
    }

    public void CursorDown() {
        if (cursorCoordinates.Item2 + 1 < Lines.Count) {
            cursorCoordinates.Item2 = cursorCoordinates.Item2 + 1;
            cursorCoordinates.Item1 = Math.Min(cursorCoordinates.Item1, Lines[cursorCoordinates.Item2].Length); 
            setCursorIndexFromCoordinates(); 
        }
    }

    private void setCursorCoordinatesFromIndex() {
        int i = 0; 
        int charsConsumed = 0; 
        while (i < Lines.Count && (charsConsumed + Lines[i].Length) < cursorIndex) {
            charsConsumed += Lines[i].Length + 1; 
            i++; 
        }
        cursorCoordinates = (cursorIndex - charsConsumed, i);
    }

    private void setCursorIndexFromCoordinates() {
        (int column,  int row) = cursorCoordinates;
        int index = 0; 
        for (int i = 0; i < row && i < Lines.Count; i++) {
            index += Lines[i].Length + 1; 
        }
        index += column; 
        cursorIndex = index; 
    }
}

public class TextInputContainer {
    private TextInput input; 
    private TextInput labelInput; 
    private int textInputEntity; 
    private int parentEntity; 
    private int labelEntity; 

    public int GetTextInputEntity() => textInputEntity; 
    public int GetParentEntity() => parentEntity; 
    public string GetText() => input.Text;
    public int LabelEntity => labelEntity; 
    public TextInput GetTextInput() => input; 
    public TextInput GetLabelInput() => labelInput; 

    public TextInputContainer(TextInput input, TextInput labelInput, int entity, int parentEntity, int labelEntity) {
        this.textInputEntity = entity; 
        this.parentEntity = parentEntity; 
        this.input = input; 
        this.labelEntity = labelEntity;
        this.labelInput = labelInput; 
    }
}

public static class TextInputWrap {

    public static TextInputContainer Add(World w, Vector2 position, float width, float height, 
        string label = "", string defaultText = "", bool editableLabel = false) {

        int childrenPerPage = GetChildrenPerPage(w, height); 
        LinearLayoutContainer llc = LinearLayoutWrap.Add(w, position, width, height, usePaging: true, 
            childrenPerPage: childrenPerPage, direction: "vertical", align: "alignlow", padding: 0f, 
            outline: true, label: label);
        int cursorEnt = EntityFactory.AddUI(w, Vector2.Zero, 2, 20, setOutline: true); 
        TextInput input = new TextInput(GetCharsPerRow(w, width), defaultText, cursorEnt); 

        int llEnt = llc.LLEnt; 
        w.SetComponent<TextInput>(llEnt, input); 
        w.SetComponent<Button>(llEnt, new Button()); 

        TextInput labelInput = null; 

        if (editableLabel && label != "") {
            int labelEnt = llc.LabelEntity; 
            w.SetComponent<Button>(labelEnt, new Button());
            labelInput = new TextInput(defaultText: label);
            w.SetComponent<TextInput>(labelEnt, labelInput); 
        }

        return new TextInputContainer(input, labelInput, llEnt, llc.GetParentEntity(), llc.LabelEntity);  
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
namespace TrainGame.Systems;

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Constants;
using TrainGame.Utils;

public static class TextInputWrap {

    public static TextInputContainer Add(World w, Vector2 position, float width, float height, 
        string label = "", string defaultText = "", bool editableLabel = false) {

        int childrenPerPage = GetChildrenPerPage(w, height); 
        LinearLayoutContainer llc = LinearLayoutContainer.Add(w, position, width, height, usePaging: true, 
            childrenPerPage: childrenPerPage, direction: "vertical", align: "alignlow", padding: 0f, 
            outline: true, label: label);
        int cursorEnt = addCursor(w); 
        TextInput input = new TextInput(GetCharsPerRow(w, width), defaultText, cursorEnt); 

        int llEnt = llc.LLEnt; 
        w.SetComponent<TextInput>(llEnt, input); 
        w.SetComponent<Button>(llEnt, new Button()); 

        TextInput labelInput = null; 

        if (editableLabel && label != "") {
            int labelEnt = llc.LabelEntity; 
            int cEnt = addCursor(w); 
            w.SetComponent<Button>(labelEnt, new Button());
            labelInput = new TextInput(defaultText: label, cursorEnt: cEnt);
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

    private static int addCursor(World w) {
        return EntityFactory.AddUI(w, Vector2.Zero, 2, 20, setOutline: true); 
    }
}
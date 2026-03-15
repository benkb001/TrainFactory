namespace TrainGame.Components;

using System;

public class TextInputContainer {
    private TextInput input; 
    private TextInput labelInput; 
    private int textInputEntity; 
    private int parentEntity; 
    private int labelEntity; 

    public int GetTextInputEntity() => textInputEntity; 
    public int GetParentEntity() => parentEntity; 
    public string GetText() => input.Text;
    public string Text => input.Text; 
    public int LabelEntity => labelEntity; 
    public TextInput GetTextInput() => input; 
    public TextInput GetLabelInput() => labelInput;
    public bool Active => input.Active; 

    public TextInputContainer(TextInput input, TextInput labelInput, int entity, int parentEntity, int labelEntity) {
        this.textInputEntity = entity; 
        this.parentEntity = parentEntity; 
        this.input = input; 
        this.labelEntity = labelEntity;
        this.labelInput = labelInput; 
    }

    public void Activate() {
        input.Active = true; 
    }

    public void Deactivate() {
        input.Active = false; 
    }
}
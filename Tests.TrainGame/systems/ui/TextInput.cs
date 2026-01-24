
using System.Collections.Generic;
using System; 
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Systems; 
using TrainGame.Utils; 

[Collection("Sequential")]
public class TextInputSystemTest {
    [Fact]
    public void TextInputSystem_ClickingOnTextInputShouldActivateIt() {
        World w = WorldFactory.Build(); 
        TextInputContainer tIn = TextInputWrap.Add(w, Vector2.Zero, 100, 100);
        int tEnt = tIn.GetTextInputEntity();
        w.GetComponent<Button>(tEnt).ClickType = Click.Left; 
        w.Update(); 
        Assert.True(w.GetComponent<TextInput>(tEnt).Active); 
    }

    [Fact]
    public void TextInputSystem_ClickingAwayFromTextShouldDeactivateIt() {
        World w = WorldFactory.Build(); 
        TextInputContainer tIn = TextInputWrap.Add(w, Vector2.Zero, 100, 100);
        int tEnt = tIn.GetTextInputEntity();
        w.GetComponent<Button>(tEnt).ClickType = Click.Left; 
        w.Update(); 
        TextInput input = w.GetComponent<TextInput>(tEnt);
        Assert.True(input.Active); 
        VirtualMouse.LeftClick(new Vector2(-100, -100));
        w.Update(); 
        Assert.False(input.Active); 
    }

    [Fact]
    public void TextInputSystem_TypingShouldAddCharacterToTextIfActive() {
        World w = WorldFactory.Build(); 
        TextInputContainer tIn = TextInputWrap.Add(w, Vector2.Zero, 100, 100);
        tIn.Activate(); 
        w.Update(); 
        VirtualKeyboard.Press(Keys.A); 
        w.Update(); 
        Assert.Equal("a", tIn.Text); 
    }

    [Fact]
    public void TextInputSystem_TypingShouldDoNothingIfNotActive() {
        VirtualKeyboard.Reset(); 

        VirtualKeyboard.UseVirtualKeyboard(); 
        World w = WorldFactory.Build(); 
        TextInputContainer tIn = TextInputWrap.Add(w, Vector2.Zero, 100, 100);
        tIn.Deactivate(); 
        VirtualKeyboard.Click(Keys.A); 
        w.Update(); 
        Assert.Equal("", tIn.Text); 

        VirtualKeyboard.Reset(); 
    }
}
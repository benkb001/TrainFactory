
using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.Components; 

public class TextInputTest {
    private TextInput init() {
        return new TextInput(charsPerRow: 10);
    }

    [Fact]
    public void TextInput_AddCharShouldIncrementCursor() {
        TextInput inp = init(); 
        Assert.Equal(0, inp.CursorIndex); 
        inp.AddChar("A"); 
        inp.AddChar("B");
        Assert.Equal(2, inp.CursorIndex); 
        Assert.Equal((2, 0), inp.CursorCoordinates); 
    }

    [Fact]
    public void TextInput_DeleteCharShouldDecrementCursor() {
        TextInput inp = init(); 
        Assert.Equal(0, inp.CursorIndex); 
        inp.AddChar("A"); 
        Assert.Equal(1, inp.CursorIndex); 
        inp.DeleteChar(); 
        Assert.Equal(0, inp.CursorIndex); 
    }

    [Fact]
    public void TextInput_CursorLeftShouldMoveCursorLeft() {
        TextInput inp = init(); 
        inp.AddChar("A"); 
        Assert.Equal(1, inp.CursorIndex); 
        inp.CursorLeft(); 
        Assert.Equal(0, inp.CursorIndex); 
    }

    [Fact]
    public void TextInput_AddToLinesShouldMakeANewLineForEachNewLineCharacter() {
        TextInput inp = init(); 
        inp.AddToLines("1234567890\n");
        Assert.Equal(2, inp.Lines.Count); 
    }

    [Fact]
    public void TextInput_WhenCursorIsOnTheLastCharInRowItShouldBeMovedDown() {
        TextInput inp = init(); 
        inp.AddToLines("1234567890\n");
        Assert.Equal((0, 1), inp.CursorCoordinates); 
    }
    
    [Fact]
    public void TextInput_CursorUpShouldMoveCursorUp() {
        TextInput inp = init(); 
        inp.AddToLines("1234567890\n");
        Assert.Equal((0, 1), inp.CursorCoordinates); 
        inp.CursorUp(); 
        Assert.Equal((0, 0), inp.CursorCoordinates); 
    }

    [Fact]
    public void TextInput_CursorDownShouldMoveCursorDown() {
        TextInput inp = init(); 
        inp.AddToLines("1234567890\n");
        Assert.Equal((0, 1), inp.CursorCoordinates); 
        inp.CursorUp(); 
        Assert.Equal((0, 0), inp.CursorCoordinates); 
        inp.CursorDown(); 
        Assert.Equal((0, 1), inp.CursorCoordinates); 
    }

    [Fact]
    public void TextInput_AddToLinesShouldSetEachLineBasedOnCharsPerRow() {
        TextInput inp = init(); 
        inp.AddToLines("\nLine1\nLine2\n\nLine4");

        //
        //Line1
        //Line2
        //
        //Line4
        Assert.Equal("", inp.Lines[0]); 
        Assert.Equal("Line1", inp.Lines[1]); 
        Assert.Equal("Line2", inp.Lines[2]); 
        Assert.Equal("Line4", inp.Lines[4]);
    }
}
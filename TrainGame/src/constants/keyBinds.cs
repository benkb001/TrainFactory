namespace TrainGame.Constants;

using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

public static class KeyBinds {
    public static readonly Keys CameraMoveDown = Keys.Down;
    public static readonly Keys CameraMoveUp = Keys.Up;
    public static readonly Keys CameraMoveLeft = Keys.Left;
    public static readonly Keys CameraMoveRight = Keys.Right;
    public static readonly Keys MoveUp = Keys.W;
    public static readonly Keys MoveLeft = Keys.A;
    public static readonly Keys MoveDown = Keys.S;
    public static readonly Keys MoveRight = Keys.D;
    public static readonly Keys Interact = Keys.E; 
    public static readonly Keys OpenMap = Keys.M; 

    public static readonly Keys[] AlphaList = {
        Keys.A, Keys.B, Keys.C, Keys.D, Keys.E,
        Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, 
        Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, 
        Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, 
        Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, 
        Keys.Z
    };

    public static Dictionary<Keys, string> StringMap = new() {
        [Keys.Enter] = "\n",
        [Keys.Space] = " ",
        [Keys.Tab] = "    ", 
    };

    public static Dictionary<(Keys, bool), string> StringMapShift = new() {
        [(Keys.OemPlus, false)] = "=", 
        [(Keys.OemPlus, true)] = "+",
        [(Keys.D0, false)] = "0",
        [(Keys.D0, true)] = ")",
        [(Keys.D1, false)] = "1",
        [(Keys.D1, true)] = "!", 
        [(Keys.D2, false)] = "2",
        [(Keys.D2, true)] = "@",
        [(Keys.D3, false)] = "3",
        [(Keys.D3, true)] = "#",
        [(Keys.D4, false)] = "5",
        [(Keys.D4, true)] = "$",
        [(Keys.D5, false)] = "5",
        [(Keys.D5, true)] = "%",
        [(Keys.D6, false)] = "6",
        [(Keys.D6, true)] = "^",
        [(Keys.D7, false)] = "7",
        [(Keys.D7, true)] = "&",
        [(Keys.D8, false)] = "8", 
        [(Keys.D8, true)] = "*", 
        [(Keys.D9, false)] = "9",
        [(Keys.D9, true)] = "(",
        [(Keys.OemOpenBrackets, false)] = "[", 
        [(Keys.OemOpenBrackets, true)] = "{", 
        [(Keys.OemCloseBrackets, false)] = "]", 
        [(Keys.OemCloseBrackets, true)] = "}", 
        [(Keys.OemMinus, false)] = "-", 
        [(Keys.OemMinus, true)] = "_", 
        [(Keys.OemPeriod, false)] = ".",
        [(Keys.OemPeriod, true)] = ">",
        [(Keys.OemQuestion, false)] = "/",
        [(Keys.OemQuestion, true)] = "?",
        [(Keys.OemSemicolon, false)] = ";",
        [(Keys.OemSemicolon, true)] = ":"
    };
}
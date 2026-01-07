namespace TrainGame.Systems; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 
using System.Text.RegularExpressions; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Constants;

public enum TokenType {
    True, 
    False, 
    Plus, 
    Minus, 
    Divide, 
    Multiply, 
    Go, 
    City, 
    Train, 
    OpenCurly, 
    CloseCurly, 
    Wait,
    While, 
    Access, 
    Load, 
    Unload, 
    ItemID, 
    Semicolon, 
    Int, 
    And,
    Or,
    Equal, 
    Not, 
    NotEqual, 
    Less, 
    Greater, 
    LessEqual, 
    GreaterEqual,
    Error,
    End,
    Self
}

public class TALToken {
    public TokenType Type => type; 
    private TokenType type; 
    private int i; 
    private string id; 

    public int IntVal => i; 
    public string ID => id; 

    public TALToken(TokenType type) {
        this.type = type; 
    }

    public static TALToken Int(int i) {
        TALToken t = new TALToken(TokenType.Int); 
        t.i = i; 
        return t; 
    }

    public static TALToken ItemID(string id) {
        TALToken t = new TALToken(TokenType.ItemID); 
        t.id = id; 
        return t; 
    }

    public static TALToken City(string id) {
        TALToken t = new TALToken(TokenType.City); 
        t.id = id; 
        return t; 
    }

    public static TALToken Train(string id) {
        TALToken t = new TALToken(TokenType.Train); 
        t.id = id; 
        return t; 
    }

}

public class TALLexer {

    public static Regex InList(List<string> ls) {
        string rxStr = ls.Aggregate("", (acc, cur) => $@"{acc}\G{cur}|"); 
        return new Regex(rxStr.Remove(rxStr.Length - 1)); 
    }

    public static List<Train> Trains(World w) {
        return w.GetMatchingEntities([typeof(Train), typeof(Data)]).Select(e => w.GetComponent<Train>(e)).ToList();
    }

    public static Regex rxCity = InList(CityID.All); 
    public static Regex rxItem = InList(ItemID.All); 
    public static Regex rxTrue = new Regex(@"\Gtrue"); 
    public static Regex rxFalse = new Regex(@"\Gfalse"); 
    public static Regex rxInt = new Regex(@"\G[0-9]+"); 
    public static Regex rxLoad = new Regex(@"\GLOAD"); 
    public static Regex rxUnload = new Regex(@"\GUNLOAD"); 
    public static Regex rxGo = new Regex(@"\GGO TO"); 
    public static Regex rxWait = new Regex(@"\GWAIT"); 
    public static Regex rxWhile = new Regex(@"\GWHILE");
    public static Regex rxPlus = new Regex(@"\G\+"); 
    public static Regex rxMinus = new Regex(@"\G-"); 
    public static Regex rxMultiply = new Regex(@"\G\*"); 
    public static Regex rxDivide = new Regex(@"\G/"); 
    public static Regex rxOpenCurly = new Regex(@"\G{"); 
    public static Regex rxCloseCurly = new Regex(@"\G}"); 
    public static Regex rxSemi = new Regex(@"\G;");
    public static Regex rxAccess = new Regex(@"\G\."); 
    public static Regex rxEqual = new Regex(@"\G=="); 
    public static Regex rxNotEqual = new Regex(@"\G!=");
    public static Regex rxNot = new Regex(@"\G!");
    public static Regex rxGreaterEqual = new Regex(@"\G>=");
    public static Regex rxLessEqual = new Regex(@"\G<=");
    public static Regex rxGreater = new Regex(@"\G>");
    public static Regex rxLess = new Regex(@"\G<");
    public static Regex rxTrain = new Regex(@"\G[A-Za-z]([A-Za-z0-9]*)");
    public static Regex rxAnd = new Regex(@"\GAND");
    public static Regex rxOr = new Regex(@"\GOR");
    public static Regex rxSelf = new Regex(@"\GSELF");

    public static List<Regex> rxs = [
        rxCity, rxItem, rxTrue, rxFalse, rxAnd, rxOr, rxSelf, rxInt, 
        rxLoad, rxUnload, rxGo, rxWait, rxWhile, rxPlus, rxMinus, 
        rxMultiply, rxDivide, rxOpenCurly, rxCloseCurly,
        rxSemi, rxAccess, rxEqual, rxNotEqual, rxNot, 
        rxGreaterEqual, rxLessEqual, rxGreater, rxLess, 
        rxTrain
    ];

    public static Dictionary<Regex, Func<Match, TALToken>> matchToTok = new() {
        [rxCity] = (m) => TALToken.City(m.Value),
        [rxItem] = (m) => TALToken.ItemID(m.Value),
        [rxInt] = (m) => TALToken.Int(int.Parse(m.Value)),
        [rxTrain] = (m) => TALToken.Train(m.Value),
        [rxAnd] = (m) => new TALToken(TokenType.And),
        [rxTrue] = (m) => new TALToken(TokenType.True), 
        [rxFalse] = (m) => new TALToken(TokenType.False), 
        [rxLoad] = (m) => new TALToken(TokenType.Load), 
        [rxUnload] = (m) => new TALToken(TokenType.Unload), 
        [rxGo] = (m) => new TALToken(TokenType.Go), 
        [rxPlus] = (m) => new TALToken(TokenType.Plus), 
        [rxMinus] = (m) => new TALToken(TokenType.Minus),
        [rxMultiply] = (m) => new TALToken(TokenType.Multiply), 
        [rxOr] = (m) => new TALToken(TokenType.Or),
        [rxDivide] = (m) => new TALToken(TokenType.Divide), 
        [rxOpenCurly] = (m) => new TALToken(TokenType.OpenCurly), 
        [rxCloseCurly] = (m) => new TALToken(TokenType.CloseCurly), 
        [rxSemi] = (m) => new TALToken(TokenType.Semicolon),
        [rxAccess] = (m) => new TALToken(TokenType.Access), 
        [rxEqual] = (m) => new TALToken(TokenType.Equal), 
        [rxNotEqual] = (m) => new TALToken(TokenType.NotEqual), 
        [rxGreaterEqual] = (m) => new TALToken(TokenType.GreaterEqual), 
        [rxLessEqual] = (m) => new TALToken(TokenType.LessEqual), 
        [rxGreater] = (m) => new TALToken(TokenType.Greater), 
        [rxLess] = (m) => new TALToken(TokenType.Less),
        [rxWait] = (m) => new TALToken(TokenType.Wait), 
        [rxWhile] = (m) => new TALToken(TokenType.While),
        [rxSelf] = (m) => new TALToken(TokenType.Self)
    };

    public static TALToken MatchToToken(Match m, Regex r) {
        return matchToTok[r](m); 
    }

    public static List<TALToken> Tokenize(string program) {

        List<TALToken> ts = new(); 

        while (program.Length > 0) {
            program = program.Trim(); 
            bool found = false; 
            int i = 0; 
            while (!found && i < rxs.Count) {
                Regex rx = rxs[i]; 
                Match m = rx.Match(program); 
                if (m.Success) {
                    if (m.Value.Length == 0) {
                        throw new InvalidOperationException($"{program} tried to lex a zero-char token"); 
                    }
                    ts.Add(MatchToToken(m, rx));
                    program = program.Remove(0, m.Value.Length);
                    found = true; 
                }
                i++; 
            }

            if (!found) {
                throw new InvalidOperationException($"{program} next word is not lexable"); 
            }
        }
        
        return ts; 
    }

    public static string ToksToString(List<TALToken> ts) {
        string s = "";
        foreach (TALToken t in ts) {
            s += t.Type.ToString(); 
        }
        return s;
    }
}
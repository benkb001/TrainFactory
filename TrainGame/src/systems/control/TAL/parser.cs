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
using TrainGame.Utils; 


public static class TALParser {
    public static TALToken MatchOne(TokenType type, List<TALToken> ts) {
        TALToken t = ts[0]; 
        if (t.Type != type) {
            throw new InvalidOperationException($"TAL parser expected {type}, got {t.Type}");
        }
        ts.RemoveAt(0); 
        return t; 
    }

    public static TALToken Lookahead(List<TALToken> ts, int look = 0) {
        if (ts.Count > look) {
            return ts[look]; 
        } 
        return new TALToken(TokenType.End); 
    }

    public static List<TALInstruction> ParseBody(List<TALToken> ts) {
        return new List<TALInstruction>(); 
    }

    public static TALExpression ParseIntExp(List<TALToken> ts, World w, Train train) {
        TALToken t1 = Lookahead(ts); 
        TALExpression e1; 
        TALExpression e2; 
        TALToken tokItem; 
        
        switch (t1.Type) {
            case TokenType.Int: 
                MatchOne(TokenType.Int, ts); 
                e1 = TALExpression.Int(t1.IntVal); 
                break; 
            case TokenType.Self: 
                MatchOne(TokenType.Self, ts); 
                MatchOne(TokenType.Access, ts); 
                tokItem = MatchOne(TokenType.ItemID, ts); 
                e1 = TALExpression.AccessTrain(tokItem.ID, train); 
                break;
            case TokenType.Train: 
                MatchOne(TokenType.Train, ts); 
                MatchOne(TokenType.Access, ts); 
                tokItem = MatchOne(TokenType.ItemID, ts);
                Train specifiedTrain = ID.GetComponent<Train>(t1.ID, w); 
                e1 = TALExpression.AccessTrain(tokItem.ID, specifiedTrain);
                break; 
            case TokenType.City: 
                MatchOne(TokenType.City, ts); 
                MatchOne(TokenType.Access, ts); 
                tokItem = MatchOne(TokenType.ItemID, ts);
                City city = ID.GetComponent<City>(t1.ID, w); 
                e1 = TALExpression.AccessCity(tokItem.ID, city);
                break; 
            default: 
                throw new InvalidOperationException("Did not received an expected token type when parsing integer expression"); 
        }

        TALToken t2 = Lookahead(ts); 
        switch (t2.Type) {
            case TokenType.Plus: 
                MatchOne(TokenType.Plus, ts);
                e2 = ParseIntExp(ts, w, train); 
                return TALExpression.Add(e1, e2); 
            case TokenType.Minus: 
                MatchOne(TokenType.Minus, ts);
                e2 = ParseIntExp(ts, w, train); 
                return TALExpression.Subtract(e1, e2); 
            case TokenType.Multiply: 
                MatchOne(TokenType.Multiply, ts);
                e2 = ParseIntExp(ts, w, train); 
                return TALExpression.Multiply(e1, e2); 
            case TokenType.Divide: 
                MatchOne(TokenType.Divide, ts);
                e2 = ParseIntExp(ts, w, train); 
                return TALExpression.Divide(e1, e2); 
            default: 
                return e1; 
        }
    }

    public static TALExpression ParseConditionalExp(List<TALToken> ts, World w, Train train) {
        TALToken t1 = Lookahead(ts); 
        TALToken t2; 
        TALExpression e1; 
        TALExpression e2; 

        if (t1.Type == TokenType.Not) {
            MatchOne(TokenType.Not, ts); 
            e1 = ParseConditionalExp(ts, w, train); 
            return TALExpression.Not(e1);
        }

        e1 = ParseBooleanExp(ts, w, train); 
        t2 = Lookahead(ts); 

        switch (t2.Type) {
            case TokenType.And: 
                MatchOne(TokenType.And, ts); 
                e2 = ParseConditionalExp(ts, w, train); 
                return TALExpression.And(e1, e2); 
            case TokenType.Or: 
                MatchOne(TokenType.Or, ts); 
                e2 = ParseConditionalExp(ts, w, train); 
                return TALExpression.Or(e1, e2); 
            case TokenType.Equal: 
                MatchOne(TokenType.Equal, ts); 
                e2 = ParseConditionalExp(ts, w, train); 
                return TALExpression.Equal(e1, e2); 
            case TokenType.NotEqual: 
                MatchOne(TokenType.NotEqual, ts); 
                e2 = ParseConditionalExp(ts, w, train); 
                return TALExpression.NotEqual(e1, e2); 
            default: 
                return e1; 
        }
    }

    public static TALExpression ParseBooleanExp(List<TALToken> ts, World w, Train train) {
        TALToken t1 = Lookahead(ts); 
        TALToken t2; 
        TALExpression e1; 
        TALExpression e2; 

        switch (t1.Type) {
            case TokenType.True: 
                MatchOne(TokenType.True, ts); 
                return TALExpression.Bool(true); 
            case TokenType.False: 
                MatchOne(TokenType.False, ts); 
                return TALExpression.Bool(false); 
            default: 
                e1 = ParseIntExp(ts, w, train); 
                t2 = Lookahead(ts); 
                switch (t2.Type) {
                    case TokenType.Greater: 
                        MatchOne(TokenType.Greater, ts); 
                        e2 = ParseIntExp(ts, w, train);
                        return TALExpression.Greater(e1, e2); 
                    case TokenType.GreaterEqual: 
                        MatchOne(TokenType.GreaterEqual, ts); 
                        e2 = ParseIntExp(ts, w, train);
                        return TALExpression.GreaterEqual(e1, e2); 
                    case TokenType.Less: 
                        MatchOne(TokenType.Less, ts); 
                        e2 = ParseIntExp(ts, w, train);
                        return TALExpression.Less(e1, e2); 
                    case TokenType.LessEqual: 
                        MatchOne(TokenType.LessEqual, ts); 
                        e2 = ParseIntExp(ts, w, train);
                        return TALExpression.LessEqual(e1, e2); 
                    case TokenType.Equal: 
                        MatchOne(TokenType.Equal, ts); 
                        e2 = ParseIntExp(ts, w, train);
                        return TALExpression.Equal(e1, e2); 
                    case TokenType.NotEqual: 
                        MatchOne(TokenType.NotEqual, ts); 
                        e2 = ParseIntExp(ts, w, train);
                        return TALExpression.NotEqual(e1, e2); 
                    default: 
                        throw new InvalidOperationException("Unexpected token when parsing an integer conditional");
                }
        }
    }

    public static TALInstruction ParseStatement(List<TALToken> ts, World w, Train train) {
        TALToken t1 = Lookahead(ts);
        TALToken t2; 
        TALExpression e1; 
        TALExpression e2; 

        switch (t1.Type) {
            case TokenType.Go: 
                MatchOne(TokenType.Go, ts); 
                t2 = MatchOne(TokenType.City, ts); 
                MatchOne(TokenType.Semicolon, ts); 
                return TALInstruction.Go(ID.GetComponent<City>(t2.ID, w));
            case TokenType.Load: 
                MatchOne(TokenType.Load, ts); 
                e1 = ParseIntExp(ts, w, train); 
                t2 = MatchOne(TokenType.ItemID, ts); 
                e2 = TALExpression.ItemID(t2.ID); 
                MatchOne(TokenType.Semicolon, ts); 
                return TALInstruction.Load(e1, e2);
            case TokenType.Unload: 
                MatchOne(TokenType.Unload, ts); 
                e1 = ParseIntExp(ts, w, train); 
                t2 = MatchOne(TokenType.ItemID, ts); 
                e2 = TALExpression.ItemID(t2.ID); 
                MatchOne(TokenType.Semicolon, ts); 
                return TALInstruction.Unload(e1, e2);
            case TokenType.Wait: 
                MatchOne(TokenType.Wait, ts); 
                MatchOne(TokenType.Semicolon, ts); 
                return TALInstruction.Wait(); 
            case TokenType.While: 
                MatchOne(TokenType.While, ts); 
                e1 = ParseConditionalExp(ts, w, train); 
                MatchOne(TokenType.OpenCurly, ts); 
                List<TALInstruction> instructions = ParseBody(ts, w, train);
                MatchOne(TokenType.CloseCurly, ts); 
                TALBody b = new TALBody(instructions, train); 
                return TALInstruction.While(e1.Condition, b); 
            default: 
                throw new InvalidOperationException($"Received unxpected token when parsing a statement"); 

        }
    }

    public static List<TALInstruction> ParseBody(List<TALToken> ts, World w, Train t) {
        TALToken t1 = Lookahead(ts); 
        List<TALInstruction> instructions = new(); 

        switch (t1.Type) {
            case TokenType.End: 
                return instructions; 
            case TokenType.CloseCurly: 
                return instructions; 
            default: 
                TALInstruction next = ParseStatement(ts, w, t); 
                instructions.Add(next); 
                instructions.AddRange(ParseBody(ts, w, t)); 
                return instructions; 
        }
    }

    public static TALBody ParseProgram(List<TALToken> ts, World w, Train t) {
        return new TALBody(ParseBody(ts, w, t), t);
    }

    public static TALBody ParseProgram(string program, World w, Train t) {
        List<TALToken> ts = TALLexer.Tokenize(program); 
        return new TALBody(ParseBody(ts, w, t), t);
    }
}

                
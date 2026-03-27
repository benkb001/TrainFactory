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
        if (ts.Count == 0) {
            throw new InvalidOperationException($"TAL parser expected {type} but ran out of input");
        }

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

    public static ITALExpression ParseIntExp<T, C>(List<TALToken> ts, ITrainWorld<T, C> w, 
        T train) where T : ITrain where C : ICity {
        
        TALToken t1 = Lookahead(ts); 
        ITALExpression e1;
        ITALExpression e2;
        TALToken tokItem; 
        
        switch (t1.Type) {
            case TokenType.OpenParen: 
                MatchOne(TokenType.OpenParen, ts); 
                e1 = ParseIntExp<T, C>(ts, w, train); 
                MatchOne(TokenType.CloseParen, ts); 
                break;
            case TokenType.Int: 
                MatchOne(TokenType.Int, ts); 
                e1 = new TALIntExpression(t1.IntVal);
                break; 
            case TokenType.Self: 
                MatchOne(TokenType.Self, ts); 
                MatchOne(TokenType.Access, ts); 
                tokItem = MatchOne(TokenType.ItemID, ts); 
                e1 = new TALAccessTrainExpression<T>(train, tokItem.ID); 
                break;
            case TokenType.Train: 
                MatchOne(TokenType.Train, ts); 
                MatchOne(TokenType.Access, ts); 
                tokItem = MatchOne(TokenType.ItemID, ts);

                T specifiedTrain = w.GetTrain(t1.ID);
                e1 = new TALAccessTrainExpression<T>(specifiedTrain, tokItem.ID);
                break; 
            case TokenType.City: 
                MatchOne(TokenType.City, ts); 
                MatchOne(TokenType.Access, ts); 
                tokItem = MatchOne(TokenType.ItemID, ts);
                C city = w.GetCity(t1.ID);
                e1 = new TALAccessCityExpression<C>(city, tokItem.ID);
                break; 
            default: 
                throw new InvalidOperationException("Did not received an expected token type when parsing integer expression"); 
        }

        TALToken t2 = Lookahead(ts); 
        switch (t2.Type) {
            case TokenType.Plus: 
                MatchOne(TokenType.Plus, ts);
                e2 = ParseIntExp<T, C>(ts, w, train); 
                return new TALAddExpression(e1, e2); 
            case TokenType.Minus: 
                MatchOne(TokenType.Minus, ts);
                e2 = ParseIntExp<T, C>(ts, w, train); 
                return new TALSubtractExpression(e1, e2); 
            case TokenType.Multiply: 
                MatchOne(TokenType.Multiply, ts);
                e2 = ParseIntExp<T, C>(ts, w, train); 
                return new TALMultiplyExpression(e1, e2); 
            case TokenType.Divide: 
                MatchOne(TokenType.Divide, ts);
                e2 = ParseIntExp<T, C>(ts, w, train); 
                return new TALDivideExpression(e1, e2); 
            default: 
                return e1; 
        }
    }

    public static ITALExpression ParseConditionalExp<T, C>(List<TALToken> ts, ITrainWorld<T, C> w, 
    T train) where T : ITrain where C : ICity {

        TALToken t1 = Lookahead(ts); 
        TALToken t2; 
        ITALExpression e1; 
        ITALExpression e2;

        if (t1.Type == TokenType.Not) {
            MatchOne(TokenType.Not, ts); 
            e1 = ParseConditionalExp<T, C>(ts, w, train); 
            return new TALNotExpression(e1);
        } else if (t1.Type == TokenType.OpenParen) {
            MatchOne(TokenType.OpenParen, ts); 
            e1 = ParseConditionalExp<T, C>(ts, w, train);
            MatchOne(TokenType.CloseParen, ts); 
        } else {
            e1 = ParseBooleanExp<T, C>(ts, w, train); 
        }

        t2 = Lookahead(ts); 

        switch (t2.Type) {
            case TokenType.And: 
                MatchOne(TokenType.And, ts); 
                e2 = ParseConditionalExp<T, C>(ts, w, train); 
                return new TALAndExpression(e1, e2); 
            case TokenType.Or: 
                MatchOne(TokenType.Or, ts); 
                e2 = ParseConditionalExp<T, C>(ts, w, train); 
                return new TALOrExpression(e1, e2); 
            case TokenType.Equal: 
                MatchOne(TokenType.Equal, ts); 
                e2 = ParseConditionalExp<T, C>(ts, w, train); 
                return new TALEqualExpression(e1, e2); 
            case TokenType.NotEqual: 
                MatchOne(TokenType.NotEqual, ts); 
                e2 = ParseConditionalExp<T, C>(ts, w, train); 
                return new TALNotEqualExpression(e1, e2); 
            default: 
                return e1; 
        }
    }

    public static ITALExpression ParseBooleanExp<T, C>(List<TALToken> ts, 
    ITrainWorld<T, C> w, T train) where T : ITrain where C : ICity {
        TALToken t1 = Lookahead(ts); 
        TALToken t2; 
        ITALExpression e1; 
        ITALExpression e2; 

        switch (t1.Type) {
            case TokenType.True: 
                MatchOne(TokenType.True, ts); 
                return new TALBoolExpression(true); 
            case TokenType.False: 
                MatchOne(TokenType.False, ts); 
                return new TALBoolExpression(false); 
            default: 
                e1 = ParseIntExp<T, C>(ts, w, train); 
                t2 = Lookahead(ts); 
                switch (t2.Type) {
                    case TokenType.Greater: 
                        MatchOne(TokenType.Greater, ts); 
                        e2 = ParseIntExp<T, C>(ts, w, train);
                        return new TALGreaterExpression(e1, e2); 
                    case TokenType.GreaterEqual: 
                        MatchOne(TokenType.GreaterEqual, ts); 
                        e2 = ParseIntExp<T, C>(ts, w, train);
                        return new TALGreaterEqualExpression(e1, e2); 
                    case TokenType.Less: 
                        MatchOne(TokenType.Less, ts); 
                        e2 = ParseIntExp<T, C>(ts, w, train);
                        return new TALLessExpression(e1, e2); 
                    case TokenType.LessEqual: 
                        MatchOne(TokenType.LessEqual, ts); 
                        e2 = ParseIntExp<T, C>(ts, w, train);
                        return new TALLessEqualExpression(e1, e2); 
                    case TokenType.Equal: 
                        MatchOne(TokenType.Equal, ts); 
                        e2 = ParseIntExp<T, C>(ts, w, train);
                        return new TALEqualExpression(e1, e2); 
                    case TokenType.NotEqual: 
                        MatchOne(TokenType.NotEqual, ts); 
                        e2 = ParseIntExp<T, C>(ts, w, train);
                        return new TALNotEqualExpression(e1, e2); 
                    default: 
                        throw new InvalidOperationException("Unexpected token when parsing an integer conditional");
                }
        }
    }

    public static ITALInstruction<T, C> ParseStatement<T, C>(List<TALToken> ts, ITrainWorld<T, C> w, 
    T train) where T : ITrain where C : ICity {
        TALToken t1 = Lookahead(ts);
        TALToken t2; 
        ITALExpression e1; 
        ITALExpression e2; 

        switch (t1.Type) {
            case TokenType.Go: 
                MatchOne(TokenType.Go, ts); 
                t2 = MatchOne(TokenType.City, ts); 
                MatchOne(TokenType.Semicolon, ts); 
                return (ITALInstruction<T, C>)new TALGoInstruction<T, C>(w.GetCity(t2.ID));
            case TokenType.Load: 
                MatchOne(TokenType.Load, ts); 
                e1 = ParseIntExp<T, C>(ts, w, train); 
                t2 = MatchOne(TokenType.ItemID, ts); 
                e2 = new TALItemIDExpression(t2.ID); 
                MatchOne(TokenType.Semicolon, ts); 
                return (ITALInstruction<T, C>)new TALLoadInstruction<T, C>(e1, e2);
            case TokenType.Unload: 
                MatchOne(TokenType.Unload, ts); 
                e1 = ParseIntExp<T, C>(ts, w, train); 
                t2 = MatchOne(TokenType.ItemID, ts); 
                e2 = new TALItemIDExpression(t2.ID); 
                MatchOne(TokenType.Semicolon, ts); 
                return (ITALInstruction<T, C>)new TALUnloadInstruction<T, C>(e1, e2);
            case TokenType.Wait: 
                MatchOne(TokenType.Wait, ts); 
                MatchOne(TokenType.Semicolon, ts); 
                return (ITALInstruction<T, C>)new TALWaitInstruction<T, C>();
            case TokenType.While: 
                MatchOne(TokenType.While, ts); 
                e1 = ParseConditionalExp<T, C>(ts, w, train); 
                MatchOne(TokenType.OpenCurly, ts); 
                List<ITALInstruction<T, C>> instructions = ParseBody<T, C>(ts, w, train);
                MatchOne(TokenType.CloseCurly, ts); 
                TALBody<T, C> b = new TALBody<T, C>(instructions, train); 
                return (ITALInstruction<T, C>)new TALWhileInstruction<T, C>(e1, b); 
            default: 
                throw new InvalidOperationException($"Received unxpected token when parsing a statement"); 

        }
    }

    public static List<ITALInstruction<T, C>> ParseBody<T, C>(List<TALToken> ts, ITrainWorld<T, C> w, 
    T t) where T : ITrain where C : ICity {
        TALToken t1 = Lookahead(ts); 
        List<ITALInstruction<T, C>> instructions = new();

        switch (t1.Type) {
            case TokenType.End: 
                return instructions; 
            case TokenType.CloseCurly: 
                return instructions; 
            default: 
                ITALInstruction<T, C> next = ParseStatement<T, C>(ts, w, t); 
                instructions.Add(next); 
                instructions.AddRange(ParseBody<T, C>(ts, w, t)); 
                return instructions; 
        }
    }

    public static TALBody<T, C> ParseProgram<T, C>(List<TALToken> ts, ITrainWorld<T, C> w, T t, 
    int nextInstruction = 0) where T : ITrain where C : ICity {
        return new TALBody<T, C>(ParseBody<T, C>(ts, w, t), t, nextInstruction);
    }

    public static TALBody<T, C> ParseProgram<T, C>(string program, ITrainWorld<T, C> w, T t, 
    int nextInstruction = 0) where T : ITrain where C : ICity {
        List<TALToken> ts = TALLexer.Tokenize(program); 
        return new TALBody<T, C>(ParseBody<T, C>(ts, w, t), t, nextInstruction);
    }
}

                
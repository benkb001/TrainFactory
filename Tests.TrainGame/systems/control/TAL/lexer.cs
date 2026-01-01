using TrainGame.Systems; 
using TrainGame.Constants; 

public class TALLexerTest {
    [Fact]
    public void CityRegexShouldMatchCities() {
        Assert.Matches(TALLexer.rxCity, CityID.Factory);
        Assert.DoesNotMatch(TALLexer.rxCity, "F");
        Assert.DoesNotMatch(TALLexer.rxCity, "CostCoast"); 
        Assert.DoesNotMatch(TALLexer.rxCity, "FactryFactory"); 
        Assert.DoesNotMatch(TALLexer.rxCity, $" {CityID.Factory}");
    }

    [Fact]
    public void OneCharRegexShouldMatch() {
        Assert.Matches(TALLexer.rxAccess, "."); 
        Assert.DoesNotMatch(TALLexer.rxAccess, "y."); 
        Assert.Matches(TALLexer.rxPlus, "+"); 
        Assert.DoesNotMatch(TALLexer.rxPlus, "1+"); 
        Assert.Matches(TALLexer.rxDivide, "/"); 
        Assert.DoesNotMatch(TALLexer.rxDivide, "1/"); 
        Assert.Matches(TALLexer.rxMultiply, "*"); 
        Assert.DoesNotMatch(TALLexer.rxMultiply, "1*"); 
        Assert.Matches(TALLexer.rxOpenCurly, "{"); 
        Assert.DoesNotMatch(TALLexer.rxOpenCurly, "E{"); 
    }

    [Fact]
    public void IntRegexShouldMatchNumbers() {
        Assert.Matches(TALLexer.rxInt, "123456789"); 
    }

    [Fact]
    public void IntRegexShouldNotMatchIfStringStartsWithSpace() {
        Assert.DoesNotMatch(TALLexer.rxInt, " 1"); 
    }

    [Fact]
    public void WhileRegexShouldMatchOnWhile() {
        Assert.Matches(TALLexer.rxWhile, "WHILE FACTORY"); 
    }

    [Fact]
    public void Lexer_ShouldDifferentiateBetweenAccessorAndTrain() {
        List<TALToken> ts = TALLexer.Tokenize("Train0.Iron"); 
        Assert.Equal(TokenType.Train, ts[0].Type); 
        Assert.Equal(TokenType.Access, ts[1].Type); 
        Assert.Equal(TokenType.ItemID, ts[2].Type); 
    }

    [Fact]
    public void LexerShouldLexBasicTokens() {
        string program = @"
        
        WHILE Factory.Iron < 100 {
            WAIT;
        }

        ";

        List<TALToken> ts = TALLexer.Tokenize(program); 
        Assert.Equal(TokenType.While, ts[0].Type); 
        Assert.Equal(TokenType.Int, ts[5].Type); 
        Assert.Equal(100, ts[5].IntVal);
    }
}
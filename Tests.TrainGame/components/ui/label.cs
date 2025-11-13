
using TrainGame.Components; 

public class LabelTest {
    [Fact]
    public void Label_ShouldRespectConstructorArguments() {
        Label l = new Label(2); 
        Assert.Equal(2, l.BodyEntity); 
    }
}
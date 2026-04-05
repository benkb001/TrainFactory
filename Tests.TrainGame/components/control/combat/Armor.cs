

using TrainGame.Constants; 
using TrainGame.Components;

public class ArmorTest {
    [Fact]
    public void Armor_DefenseShouldBeSumOfBaseAndTemp() {
        Armor a = new Armor(1); 
        a.AddTempDefense(1); 
        Assert.Equal(2, a.Defense);
    }
}
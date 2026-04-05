namespace TrainGame.Components; 

using System.Collections.Generic;
using System.Drawing; 
using System; 
using System.Linq; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Components; 
using TrainGame.ECS; 
using TrainGame.Utils; 
using TrainGame.Constants;

public class HealthTest {
    [Fact]
    public void Health_HPShouldBeSumOfBaseAndTemp() {
        Health h = new Health(1); 
        h.TempHP = 1; 
        Assert.Equal(2, h.HP); 
    }

    [Fact]
    public void Health_ReceiveDamageShouldDecreaseTempHPThenHP() {
        Health h = new Health(2); 
        h.TempHP = 1; 
        h.ReceiveDamage(2); 
        Assert.Equal(0, h.TempHP); 
        Assert.Equal(1, h.HP);
    }

    [Fact]
    public void Health_ResetHPShouldSetTempHPToZeroAndHPToMaxHP() {
        Health h = new Health(2); 
        h.ReceiveDamage(1); 
        h.TempHP = 2; 
        h.ResetHP();
        Assert.Equal(0, h.TempHP);
        Assert.Equal(2, h.HP);
    }

    [Fact]
    public void Health_SetHPShouldNotSetHPToNegative() {
        Health h = new Health(1); 
        h.SetHP(-1); 
        Assert.True(h.HP > -1);
    }

    [Fact]
    public void Health_ShouldSetHPToSpecifiedValue() {
        Health h = new Health(1);
        h.SetHP(2);
        Assert.Equal(2, h.HP);
        Assert.Equal(1, h.TempHP);
    }
}
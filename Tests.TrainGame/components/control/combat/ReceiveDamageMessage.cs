namespace TrainGame.Components;

using System;
using System.Linq;
using System.Collections.Generic; 

public class ReceiveDamageMessageTest {
    [Fact]
    public void ReceiveDamageMessage_AddDamageShouldntTakeNegativeNumbers() {
        ReceiveDamageMessage dm = new(0); 
        dm.AddDamage(-1);
        Assert.Equal(0, dm.DMG);
    }

    [Fact]
    public void ReceiveDamageMessage_DMGShouldBeTheSumOfDamages() {
        ReceiveDamageMessage dm = new(1); 
        dm.AddDamage(2);
        dm.AddDamage(3); 
        Assert.Equal(6, dm.DMG);
    }
}

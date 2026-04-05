namespace TrainGame.Components;

using System.Collections.Generic;
using System.Linq; 

using TrainGame.Utils;
using TrainGame.Constants;
using TrainGame.ECS;

public class EnemyConstTest {
    [Fact]
    public void EnemyConst_GetShooterShouldReturnNewComponentsEachTime() {
        EnemyConst ec = new EnemyConst(
            new Shooter(),
            new DefaultShootPattern(
                new BulletContainer(
                    new Bullet(1)
                )
            ),
            new DefaultMovePattern()
        );

        Assert.NotEqual(ec.GetShooter(), ec.GetShooter());
        Assert.NotEqual(ec.GetShootPattern(), ec.GetShootPattern()); 
        Assert.NotEqual(ec.GetMovement(), ec.GetMovement());
    }
}
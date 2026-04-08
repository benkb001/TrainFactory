namespace TrainGame.Components; 

using System.Collections.Generic;

public class RandomShotgunShootPattern : IShootPattern {
    public readonly double Radians; 
    public readonly BulletContainer BulletStrong; 
    public readonly BulletContainer BulletWeak; 
    public readonly int NumStrongBullets; 
    public readonly int NumWeakBullets; 

    public RandomShotgunShootPattern(BulletContainer strong, BulletContainer weak, double Radians,
    int NumStrongBullets, int NumWeakBullets) {
        this.BulletStrong = strong;
        this.BulletWeak = weak;
        this.Radians = Radians;
        this.NumStrongBullets = NumStrongBullets; 
        this.NumWeakBullets = NumWeakBullets; 
    }

    public IShootPattern Clone() {
        return new RandomShotgunShootPattern(BulletStrong.Clone(), BulletWeak.Clone(), Radians, NumStrongBullets, NumWeakBullets);
    }
    public IEnumerable<BulletContainer> GetBulletContainers() => new List<BulletContainer>(){ BulletStrong, BulletWeak };
}
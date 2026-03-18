namespace TrainGame.Components;

public class DefaultShootPattern : IShootPattern {
    public readonly float BulletSpeed; 
    public readonly int Damage;
    public int GetBulletsShot() => 1; 

    public DefaultShootPattern(float BulletSpeed = 2f, int Damage = 1) {
        this.BulletSpeed = BulletSpeed; 
        this.Damage = Damage;
    }

    public IEnumerable<BulletContainer> Shoot(Vector2 position, Vector2 targetPosition) {
        List<BulletContainer> bs = new();
        Vector2 velocity = ShooterWrap.Aim(position, targetPosition, BulletSpeed);
        Bullet b = new Bullet(Damage);
        bs.Add(new BulletContainer(b, position, velocity, Constants.DefaultBulletSize));
        return bs;
    }

    public IShootPattern Clone() {
        return new DefaultShootPattern(BulletSpeed, Damage);
    }
}
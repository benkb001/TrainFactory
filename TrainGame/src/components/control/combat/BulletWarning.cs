namespace TrainGame.Components;

using TrainGame.Utils;

public class BulletWarning {
    public WorldTime WhenToShoot; 
    public int BulletEnt; 

    public BulletWarning(WorldTime WhenToShoot, int bulletEnt) {
        this.WhenToShoot = WhenToShoot; 
        this.BulletEnt = bulletEnt; 
    }
}
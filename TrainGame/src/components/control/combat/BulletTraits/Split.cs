namespace TrainGame.Components;

public class Split : IBulletTrait, IEnemyTrait, ISplitter {
    public IShootPattern Pattern;
    public IShootPattern GetPattern() => Pattern; 

    public Split(IShootPattern Pattern) {
        this.Pattern = Pattern;
    }
}
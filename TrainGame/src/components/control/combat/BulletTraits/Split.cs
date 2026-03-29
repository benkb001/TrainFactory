namespace TrainGame.Components;

public class Split : IBulletTrait {
    public IShootPattern Pattern;
    public Split(IShootPattern Pattern) {
        this.Pattern = Pattern;
    }
}
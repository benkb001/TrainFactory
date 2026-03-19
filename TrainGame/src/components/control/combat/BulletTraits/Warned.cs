namespace TrainGame.Components;

using TrainGame.Utils;

public class Warned : IBulletTrait {
    public readonly WorldTime WarningDuration;

    public Warned(WorldTime WarningDuration) {
        this.WarningDuration = WarningDuration;
    }
}
namespace TrainGame.Systems;

using TrainGame.Components;

public static class RegisterShootPattern {
    public static void Default<T>() where T : IShootPattern {
        ShootPatternRegistry.Register<T>((w, sp, e) => {
            w.SetComponent<T>(e, sp);
        });
    }
}

public static class RegisterShootPatterns {
    public static void All() {
        RegisterShootPattern.Default<DefaultShootPattern>();
        RegisterShootPattern.Default<RadialShootPattern>();
        RegisterShootPattern.Default<MeleeShootPattern>();
    }
}
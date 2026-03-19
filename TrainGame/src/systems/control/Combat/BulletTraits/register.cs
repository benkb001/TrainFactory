namespace TrainGame.Systems;

public static class RegisterBulletTraits {
    public static void All() {
        HomingWrap.RegisterTrait(); 
        WarnedWrap.RegisterTrait();
    }
}

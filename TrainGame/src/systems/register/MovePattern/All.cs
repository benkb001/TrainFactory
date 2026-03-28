namespace TrainGame.Systems;

public static class RegisterMovementTypes {
    public static void All() {
        RegisterDefaultMovementType.Register();
        RegisterChaseMovementType.Register();
        RegisterCyclicalMovement.Register();
    }
}
namespace TrainGame.Components; 

public class Player : IFlag<Player> {
    private static Player p = null;

    public static Player Get() {
        if (p == null) {
            p = new Player();
        }
        return p;
    }
}
namespace TrainGame.Constants;

using Microsoft.Xna.Framework;
using TrainGame.Components;

public static class Colors {
    public static readonly Color Placebo = Color.DarkGray; 
    public static readonly Color UIBG = Color.LightGray; 
    public static readonly Color UIAccent = Color.DarkGray; 
    public static readonly Color BG = Color.CornflowerBlue;
    public static readonly Color InventoryHeld = Color.Red; 
    public static readonly Color InventoryNotHeld = Color.White; 

    public static readonly Color PlayerOutline = Color.Black; 
    public static readonly Color PlayerBackground = Color.White; 

    public static readonly Color Warning = Color.Red; 
    public static readonly Color Vampiric = Color.Purple;
    public static readonly Color PlayerBullet = new Color(203, 243, 249);
    public static readonly Color EnemyBullet = new Color(255, 255, 255);

    public static Color GetBulletColor<U>() {
        if (typeof(U) == typeof(Player)) {
            return PlayerBullet;
        } else if (typeof(U) == typeof(Enemy)) {
            return EnemyBullet;
        }
        return Color.White;
    }
}
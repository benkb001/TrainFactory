namespace TrainGame.Components;

using Microsoft.Xna.Framework;

public class GridShootPattern : IShootPattern {
    public int PatternIndex = 0; 
    public readonly BulletContainer BulletX; 
    public readonly BulletContainer BulletY; 
    public readonly int PatternLength; 
    public readonly float Dx; 
    public readonly float Dy; 
    public readonly int NumBulletsX; 
    public readonly int NumBulletsY; 
    public Vector2 Direction; 

    public GridShootPattern(BulletContainer BulletX, BulletContainer BulletY, float dx, 
        float dy, int cx, int cy, Vector2 direction, int patternLength) {
        this.Dx = dx; 
        this.Dy = dy; 
        this.NumBulletsX = cx; 
        this.NumBulletsY = cy; 
        this.PatternLength = patternLength; 
        this.BulletX = BulletX; 
        this.BulletY = BulletY; 
        this.Direction = Direction; 
    }

    public IShootPattern Clone() => new GridShootPattern(BulletX, BulletY, Dx, Dy, 
        NumBulletsX, NumBulletsY, Direction, PatternLength);
}
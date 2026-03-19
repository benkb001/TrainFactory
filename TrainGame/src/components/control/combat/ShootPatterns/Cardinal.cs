namespace TrainGame.Components;

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TrainGame.Systems;
using TrainGame.Constants;
using TrainGame.Utils;

public class CardinalShootPattern : IShootPattern {
    public readonly int Damage;
    public readonly float BulletSpeed; 
    private static int[] ds = {-1, 1};
    private bool up;
    private bool upRight;
    private bool right;
    private bool downRight;
    private bool down;
    private bool downLeft;
    private bool left;
    private bool upLeft;
    private int numShots;
    private BulletContainer bulletContainer;
 
    public CardinalShootPattern(BulletContainer bc,
    bool up = false, bool upRight = false, bool right = false, bool downRight = false, 
    bool down = false, bool downLeft = false, bool left = false, bool upLeft = false) {

        this.bulletContainer = bc;
        this.BulletSpeed = bc.GetBulletSpeed();
        this.up = up;
        this.upRight = upRight;
        this.right = right;
        this.downRight = downRight;
        this.down = down;
        this.downLeft = downLeft;
        this.left = left;
        this.upLeft = upLeft;

        int numShots = 0; 
        void add(bool b) {
            if (b) {
                numShots++;
            }
        }

        add(up);
        add(upRight);
        add(right);
        add(downRight);
        add(down);
        add(downLeft);
        add(left);
        add(upLeft);

        this.numShots = numShots;
    }

    public int GetBulletsShot() => numShots;
    
    public IShootPattern Clone() {
        return new CardinalShootPattern(bulletContainer.Clone(), up, upRight, 
            right, downRight, down, downLeft, left, upLeft);
    }

    public IEnumerable<BulletContainer> Shoot(Vector2 pos, Vector2 _) {
        List<BulletContainer> bs = new(); 

        void addV(Vector2 v) {
            BulletContainer b = bulletContainer.Clone();
            b.SetVelocity(v);
            b.SetPosition(pos);
            bs.Add(b);
        }

        void add(float x, float y) {
            addV(new Vector2(x, y));
        }

        if (up) {
            add(0f, -BulletSpeed);
        }
        if (upRight) {
            addV(Vector2.Normalize(new Vector2(1f, -1f)) * BulletSpeed);
        }
        if (right) {
            add(BulletSpeed, 0f); 
        }
        if (downRight) {
            addV(Vector2.Normalize(new Vector2(1f, 1f)) * BulletSpeed);
        }
        if (down) {
            add(0f, BulletSpeed);
        }
        if (downLeft) {
            addV(Vector2.Normalize(new Vector2(-1f, 1f)) * BulletSpeed);
        }
        if (left) {
            add(-BulletSpeed, 0f);
        }
        if (upLeft) {
            addV(Vector2.Normalize(new Vector2(-1f, -1f)) * BulletSpeed);
        }

        return bs;
    }
}
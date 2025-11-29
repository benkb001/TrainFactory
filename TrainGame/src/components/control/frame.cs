namespace TrainGame.Components;

using System.Collections.Generic;
using System.Drawing; 
using System; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using MonoGame.Extended.Shapes; 

public class Frame {
    private float x;
    private float y;
    private float width;
    private float height;
    private float rotation; 
    private Polygon p; 
    private static float touchThreshold = 2f; 

    public Vector2 Position => new Vector2(x, y); 

    public Frame(float x, float y, float width, float height, float rotation = 0f) {
        this.x = x;
        this.y = y;
        this.height = height;
        this.width = width;
        this.rotation = rotation; 
        
        List<Vector2> points = new(); 
        points.Add(new Vector2(x, y)); 
        points.Add(new Vector2(x + width, y)); 
        points.Add(new Vector2(x + width, y + height)); 
        points.Add(new Vector2(x, y + height)); 
        p = new Polygon(points);
    }

    public Frame(Vector2 pos, float width, float height, float rotation = 0f) : 
        this(pos.X, pos.Y, width, height, rotation) {}

    public Frame(List<Vector2> points) {
        p = new Polygon(points); 
        this.x = p.Left; 
        this.y = p.Top; 
        this.height = p.Bottom - p.Top; 
        this.width = p.Right - p.Left; 
    }

    public RectangleF GetRectangle() {
        return new RectangleF(x, y, width, height); 
    }

    public void SetCoordinates(float x, float y) {
        p.Offset(new Vector2(x - this.x, y - this.y)); 
        this.x = x; 
        this.y = y; 
    }

    public void SetCoordinates(Vector2 v) {
        SetCoordinates(v.X, v.Y); 
    }

    public void SetRotation(float r) {
        rotation = r; 
    }

    public float GetX() {
        return x; 
    }

    public float GetY() {
        return y; 
    }

    public float GetWidth() {
        return width; 
    }

    public float GetHeight() {
        return height; 
    }

    public float GetRotation() {
        return rotation; 
    }

    public bool Contains(float x, float y) {
        return p.Contains(x, y); 
    }

    public bool Contains(Vector2 point) {
        return p.Contains(point); 
    }

    public Vector2[] GetPoints() {
        return p.Vertices; 
    }

    public bool IsTouching(Frame other) {
        return 
           (Math.Abs(p.Left - other.p.Right) < touchThreshold) || 
           (Math.Abs(p.Top - other.p.Bottom) < touchThreshold) ||
           (Math.Abs(p.Right - other.p.Left) < touchThreshold) || 
           (Math.Abs(p.Bottom - other.p.Top) < touchThreshold); 
    }
}

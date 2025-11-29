namespace TrainGame.ECS; 

using System.Collections.Generic;
using System; 
using System.Linq;
using System.Drawing; 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color; 

using _Color = System.Drawing.Color; 
using _Rectangle = System.Drawing.Rectangle;

using TrainGame.Constants; 
using TrainGame.Components; 
using TrainGame.Utils; 

//this file is for the visual side of world as opposed to logic

public partial class World {
    public Texture2D GetTexture(string name) {
        //Consider throwign error instead
        if (!textures.ContainsKey(name)) {
            Console.WriteLine($"No {name} texture has been registered to world");
            return null; 
        }
        return textures[name]; 
    }

    public void SetFont(SpriteFont f) {
        font = f; 
    }

    public void SetTexture(string name, Texture2D tx) {
        textures[name] = tx; 
    }

    public void SetPixelTexture(Texture2D tx) {
        _pixel = tx; 
    }

    public void DrawLine(Vector2 point1, Vector2 point2, Color color, float depth = 0f, float thickness = 1f)
    {
        // Calculate distance and angle between the two points
        Vector2 edge = point2 - point1;
        float angle = (float)Math.Atan2(edge.Y, edge.X);

        // Draw the line
        _spriteBatch.Draw(
            _pixel,
            point1,
            null,
            color,
            angle,
            Vector2.Zero,
            new Vector2(edge.Length(), thickness),
            SpriteEffects.None,
            depth
        );
    }

    private void DrawRectangleOutline(Rectangle rect, int thickness, Color color) {
        // Top
        _spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
        // Left
        _spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
        // Right
        _spriteBatch.Draw(_pixel, new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height), color);
        // Bottom
        _spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness), color);
    }

    private void DrawOutline(Frame f, int thickness, Color c) {
        DrawRectangleOutline(Util.RectangleFromRectangleF(f.GetRectangle()), thickness, c);
    }   

    private void DrawRect(Frame f, Color color) {
        _spriteBatch.Draw(_pixel, Util.RectangleFromRectangleF(f.GetRectangle()), color);
    }

    public void LockCamera() {
        if (!isTest) {
            camera.Lock();
        }
    }

    public void UnlockCamera() {
        if (!isTest) {
            camera.Unlock(); 
        }
    }

    public Vector2 GetCameraPosition() {
        if (!isTest) {
            return camera.Position; 
        }
        return Vector2.Zero; 
    }

    public Vector2 GetCameraTopLeft() {
        if (!isTest) {
            return camera.Position - new Vector2(ScreenWidth / 2, ScreenHeight / 2); 
        }
        return Vector2.Zero; 
    }

    public void SetCameraPosition(Vector2 pos) {
        if (!isTest) {
            camera.SetPosition(pos);
            camera.UpdateCamera(graphicsDevice.Viewport, force: true); 
        }
    }

    public float GetCameraZoom() {
        if (!isTest) {
            return camera.Zoom; 
        }
        return 0f; 
    }

    public void SetCameraZoom(float zoom) {
        if (!isTest) {
            camera.SetZoom(zoom);
            camera.UpdateCamera(graphicsDevice.Viewport, force: true); 
        }
    }

    public void ResetCamera() {
        SetCameraPosition(Vector2.Zero); 
        SetCameraZoom(1f);
    }

    public Vector2 WorldVector(Vector2 screenVector) {
        if (!(camera is null)) {
            Matrix inverseCamera = Matrix.Invert(camera.GetTransform()); 
            return Vector2.Transform(screenVector, inverseCamera); 
        }
        return screenVector; 
    }

    public Vector2 ScreenVector(Vector2 worldVector) {
        return Vector2.Transform(worldVector, camera.GetTransform()); 
    }

    public Vector2 GetWorldMouseCoordinates() {
        return WorldVector(VirtualMouse.GetCoordinates()); 
    }

    public bool TrackEntity(int e) {
        if (cm.ComponentContainsEntity<Frame>(e)) {
            trackedEntity = e;
            return true; 
        }
        return false; 
    }

    public void StopTracking() {
        trackedEntity = -1; 
    }

    //unfortunately cannot just throw this in with a regular system because I don't want 
    //that to get called in the regular update step
    //could do a separate system manager for drawing i guesss but for now this is fine
    public void Draw() {
        graphicsDevice.Clear(Colors.BG);
        _spriteBatch.Begin( SpriteSortMode.BackToFront, BlendState.AlphaBlend, 
            null, null, null, null, camera.GetTransform());

        foreach (KeyValuePair<int, Sprite> entry in cm.GetComponentArray<Sprite>().GetEntities()) {
            int e = entry.Key; 
            Sprite s = entry.Value; 
            Frame frame = GetComponent<Frame>(e); 
            Texture2D tx = s.GetTexture(); 

            _spriteBatch.Draw(
                tx, 
                new Vector2(frame.GetX(), frame.GetY()), 
                null, //rectangle
                Color.White, 
                frame.GetRotation(),
                Vector2.Zero,
                frame.GetWidth() / tx.Width, //it will be the number of pixels in size of frame's width/height
                SpriteEffects.None,
                s.GetDepth()
            ); 
        }

        foreach (KeyValuePair<int, Outline> entry in cm.GetComponentArray<Outline>().GetEntities()) {
            Frame f = cm.GetComponent<Frame>(entry.Key); 
            Vector2[] points = f.GetPoints(); 
            for(int i = 0; i < points.Length; i++) {
                DrawLine(
                    points[i], 
                    points[(i + 1) % points.Length], 
                    entry.Value.GetColor(), 
                    thickness: entry.Value.GetThickness(), 
                    depth: entry.Value.Depth
                );
            }
        }

        foreach (KeyValuePair<int, Background> entry in cm.GetComponentArray<Background>().GetEntities()) {
            Frame f = cm.GetComponent<Frame>(entry.Key); 
            _spriteBatch.Draw(
                _pixel, 
                Util.RectangleFromRectangleF(f.GetRectangle()),
                null, 
                entry.Value.BackgroundColor, 
                0f, 
                Vector2.Zero, 
                SpriteEffects.None, 
                entry.Value.Depth
            );
        }

        foreach (KeyValuePair<int, Message> entry in cm.GetComponentArray<Message>().GetEntities()) {
            
            Message m = entry.Value; 
            Frame f = cm.GetComponent<Frame>(entry.Key); 
            int x = (int)f.GetX(); 
            int y = (int)f.GetY(); 

            Vector2 pos = new Vector2(x, y); 

            _spriteBatch.DrawString(
                 font,
                 m.message.Replace(" ", "  "), 
                 pos, 
                 m.color, 
                 m.rotation, 
                 Vector2.Zero,
                 m.scale,
                 SpriteEffects.None, 
                 m.depth, 
                 false
            );

        }

        foreach (KeyValuePair<int, TextBox> entry in cm.GetComponentArray<TextBox>().GetEntities()) {
            TextBox tb = entry.Value; 
            Frame f = cm.GetComponent<Frame>(entry.Key); 
            int x = (int)f.GetX(); 
            int y = (int)f.GetY(); 
            float width = f.GetWidth() - (tb.Padding * 2); 
            float height = f.GetHeight() - (tb.Padding * 2); 
            float used_height = 0; 

            int words_drawn = 0; 
            List<string> lines = []; 
            string[] words = tb.Text.Split(' ');

            //TODO THERE IS AN INFINITE LOOP BUG IF TEXT IS TOO LONG FOR BOX
            while (words_drawn < words.Length) {
                string cur = ""; 
                while (words_drawn < words.Length && 
                        (font.MeasureString((cur + words[words_drawn]).Replace(" ", "  ")).X * tb.Scale) < width) {
                    cur += words[words_drawn]; 
                    cur += " "; 
                    words_drawn++; 
                }
                float cur_height = font.MeasureString(cur).Y * tb.Scale; 
                used_height += cur_height; 
                if (used_height > height || cur_height <= 0.01f) {
                    used_height = 0; 
                    words_drawn = 0;
                    lines = [];
                    tb.Scale -= 0.1f; 
                } else {
                    lines.Add(cur); 
                }
            }

            float lineY = f.GetY(); 
            foreach (string line in lines) {
                Vector2 pos = new Vector2(x + tb.Padding, lineY + tb.Padding); 
                _spriteBatch.DrawString(
                    font,
                    line.Replace(" ", "  "), 
                    pos, 
                    tb.TextColor, 
                    0f, 
                    Vector2.Zero,
                    new Vector2(tb.Scale, tb.Scale),
                    SpriteEffects.None, 
                    tb.Depth, 
                    false
                );
                lineY += font.MeasureString(line).Y * tb.Scale; 
            }
        }

        foreach (KeyValuePair<int, Lines> entry in cm.GetComponentArray<Lines>().GetEntities()) {
            foreach((Vector2 v1, Vector2 v2, Color c) in entry.Value.Ls) {
                DrawLine(v1, v2, c); 
            }
        }
        
        _spriteBatch.End();
    }
}
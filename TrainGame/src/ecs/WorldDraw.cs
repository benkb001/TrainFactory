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
    //TODO: Add a function to get all the components of a certain type based on a HashSet<int> entities
    //ie get that component for all the entities in that hash-set

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

    private Vector2 WorldVector(Vector2 screenVector) {
        if (!(camera is null)) {
            Matrix inverseCamera = Matrix.Invert(camera.Transform); 
            return Vector2.Transform(screenVector, inverseCamera); 
        }
        return screenVector; 
    }

    private Vector2 ScreenVector(Vector2 worldVector) {
        return Vector2.Transform(worldVector, camera.Transform); 
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
        //graphicsDevice.SetRenderTarget(renderTarget); 
        graphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin( SpriteSortMode.BackToFront, BlendState.AlphaBlend, 
            null, null, null, null, camera.Transform );

        foreach (KeyValuePair<int, Sprite> entry in cm.GetComponentArray<Sprite>().GetEntities()) {
            int e = entry.Key; 
            Sprite s = entry.Value; 
            Frame frame = GetComponent<Frame>(e); 
            Texture2D tx = s.GetTexture(); 
            //need to update this to account for depth but the overload is so long and annoying lol 
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
                    thickness: entry.Value.GetThickness()
                );
            }
            
            //DrawOutline(cm.GetComponent<Frame>(entry.Key), entry.Value.GetThickness(), entry.Value.GetColor());
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
            List<string> lines = new List<string>(); 
            string[] words = tb.Text.Split(' ');

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
                    lines = new List<string>(); 
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
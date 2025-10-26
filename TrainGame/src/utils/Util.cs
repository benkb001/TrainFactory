namespace TrainGame.Utils; 

using System.Collections.Generic;
using System; 
using System.Linq;
using System.Drawing; 

using Rectangle = Microsoft.Xna.Framework.Rectangle;
using _Rectangle = System.Drawing.Rectangle;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using TrainGame.Constants; 
using TrainGame.Components; 

public static class Util {
    public static Rectangle RectangleFromRectangleF(RectangleF rectF) {
        return new Rectangle(
            (int)rectF.X,
            (int)rectF.Y,
            (int)rectF.Width,
            (int)rectF.Height
        );
    }

    public static bool FloatEqual(float f1, float f2) {
        return Math.Abs(f1 - f2) < 0.001;
    }
}
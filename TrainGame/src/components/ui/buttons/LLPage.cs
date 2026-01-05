namespace TrainGame.Components;

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Color = Microsoft.Xna.Framework.Color; 
using _Color = System.Drawing.Color; 

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Constants; 
using TrainGame.Systems; 

public class LLPageButton {
    public readonly LinearLayout LL; 
    private int delta; 
    public int Delta => delta; 
    public LLPageButton(LinearLayout ll, int delta) {
        this.delta = delta; 
        this.LL = ll; 
    }
}
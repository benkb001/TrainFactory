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

public class LinearLayoutContainer {
    private int llEnt; 
    private LinearLayout ll;
    private float llWidth; 
    private float llHeight;
    private int parentEntity;  
    private int labelEntity; 
    
    public float LLWidth => llWidth; 
    public float LLHeight => llHeight; 
    public int LLEnt => llEnt; 
    public int LabelEntity => labelEntity; 
    public int GetParentEntity() => parentEntity; 
    public LinearLayout LL => ll; 
    public List<int> GetChildren() => ll.GetChildren(); 

    public LinearLayoutContainer(int llEnt, int parentEnt, int labelEntity, 
        LinearLayout ll, float llWidth, float llHeight) {
        
        this.llEnt = llEnt; 
        this.parentEntity = parentEnt; 
        this.ll = ll; 
        this.llWidth = llWidth; 
        this.llHeight = llHeight; 
        this.labelEntity = labelEntity; 
    }
}
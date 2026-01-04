namespace TrainGame.Systems; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class DrawTrainsViewSystem() {
    public static void Draw(DrawTrainsViewMessage dm, World w) {
        List<Train> trains = dm.Trains; 

        int trainsViewEntity = EntityFactory.Add(w); 
        Frame trainsViewFrame = new Frame(dm.Position, dm.Width, dm.Height); 
        w.SetComponent<Frame>(trainsViewEntity, trainsViewFrame); 
        w.SetComponent<Outline>(trainsViewEntity, new Outline()); 
        LinearLayout tvLL = new LinearLayout("vertical", "alignlow"); 
        tvLL.Padding = dm.Padding; 
        w.SetComponent<LinearLayout>(trainsViewEntity, tvLL);

        //TODO: extend LinearLayout to have arrows so we can do paging
        //TODO: since train embarking is here, we should have a 
        //different component type to display trains as they are moving 
        foreach (Train train in trains) {
            int tEntity = EntityFactory.Add(w); 

            w.SetComponent<TrainUI>(tEntity, new TrainUI(train)); 
            w.SetComponent<Outline>(tEntity, new Outline()); 
            w.SetComponent<TextBox>(tEntity, new TextBox(train.Id)); 
            w.SetComponent<Button>(tEntity, new Button()); 
            tvLL.AddChild(tEntity); 
        }

        LinearLayoutWrap.ResizeChildren(trainsViewEntity, w);
    }

    public static void Register(World world) {

        Type[] ts = [typeof(DrawTrainsViewMessage)]; 
        Action<World, int> tf = (w, e) => {
            DrawTrainsViewMessage dm = w.GetComponent<DrawTrainsViewMessage>(e); 
            Draw(dm, w); 
            w.RemoveEntity(e); 
        }; 
        world.AddSystem(ts, tf); 

    }

}
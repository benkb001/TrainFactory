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

//needs to be trainInteract -> push -> drawTrainDetail
public class DrawEmbarkSystem() {

    public static void Register(World world) {

        Type[] ts = [typeof(DrawEmbarkMessage)]; 
        Action<World, int> tf = (w, e) => {
            DrawEmbarkMessage msg = w.GetComponent<DrawEmbarkMessage>(e); 
            City c = msg.GetCity(); 
            Train t = msg.GetTrain(); 
            float width = msg.Width; 
            float labelHeight = msg.Height * 0.2f; 
            float height = msg.Height - labelHeight;
            float padding = msg.Padding; 
            
            int labelEntity = EntityFactory.Add(w); 
            int llEntity = EntityFactory.Add(w); 
            w.SetComponent<Frame>(labelEntity, new Frame(0, 0, msg.Width / 2, labelHeight)); 
            w.SetComponent<Outline>(labelEntity, new Outline()); 
            w.SetComponent<TextBox>(labelEntity, new TextBox($"Send {t.Id} to a new city?")); 
            w.SetComponent<Label>(labelEntity, new Label(llEntity));
            Vector2 pos = msg.Position + new Vector2(0, labelHeight); 
            List<City> adjacentCities = c.AdjacentCities;

            w.SetComponent<Menu>(llEntity, Menu.Get()); 

            LinearLayout ll = new LinearLayout("vertical", "alignLow"); 
            ll.Padding = padding; 
            w.SetComponent<LinearLayout>(llEntity, ll); 
            Frame f = new Frame(pos, width, height); 

            w.SetComponent<Frame>(llEntity, f); 
            w.SetComponent<Outline>(llEntity, new Outline()); 
            
            foreach (City connected in adjacentCities) {
                int cellEntity = EntityFactory.Add(w); 
                ll.AddChild(cellEntity); 
                
                w.SetComponent<Outline>(cellEntity, new Outline()); 
                w.SetComponent<TextBox>(cellEntity, new TextBox(connected.CityId)); 
                w.SetComponent<Button>(cellEntity, new Button()); 
                w.SetComponent<EmbarkButton>(cellEntity, new EmbarkButton(connected, t)); 
            }

            LinearLayoutWrap.ResizeChildren(llEntity, w); 

            w.RemoveEntity(e); 
        }; 
        world.AddSystem(ts, tf); 
    }

}
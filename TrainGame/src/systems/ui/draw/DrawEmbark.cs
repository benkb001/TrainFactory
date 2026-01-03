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
public class DrawEmbarkSystem {

    public static int Draw(DrawEmbarkMessage msg, World w) {
        City c = msg.GetCity(); 
        Train t = msg.GetTrain(); 

        float width = msg.Width; 
        float height = msg.Height; 
        float padding = msg.Padding; 
        float buttonWidth = msg.Width - (2 * msg.Padding); 
        float buttonHeight = msg.Height / 6f;

        Vector2 pos = msg.Position; 

        int llEntity = EntityFactory.Add(w); 
        LinearLayout ll = new LinearLayout("vertical", "alignLow"); 
        ll.Padding = padding; 
        w.SetComponent<LinearLayout>(llEntity, ll); 
        w.SetComponent<Frame>(llEntity, new Frame(pos, width, height)); 
        w.SetComponent<Outline>(llEntity, new Outline()); 

        int labelEntity = EntityFactory.Add(w); 
        w.SetComponent<Outline>(labelEntity, new Outline()); 
        string summary = $"{t.Id}\nMPH: {t.MilesPerHour}\nCarts: {t.Carts.Count}\nSend to a new city?"; 
        w.SetComponent<TextBox>(labelEntity, new TextBox(summary)); 
        w.SetComponent<Frame>(labelEntity, new Frame(0, 0, buttonWidth, buttonHeight * 2)); 
        ll.AddChild(labelEntity); 
        
        List<City> adjacentCities = c.AdjacentCities;

        foreach (City connected in adjacentCities) {
            int cellEntity = EntityFactory.Add(w); 
            ll.AddChild(cellEntity); 
            
            w.SetComponent<Outline>(cellEntity, new Outline()); 
            w.SetComponent<TextBox>(cellEntity, new TextBox(connected.CityId)); 
            w.SetComponent<Button>(cellEntity, new Button()); 
            w.SetComponent<EmbarkButton>(cellEntity, new EmbarkButton(connected, t)); 
            w.SetComponent<Frame>(cellEntity, new Frame(0, 0, buttonWidth, buttonHeight)); 
        }

        if (c.HasPlayer) {
            int btnEntity = EntityFactory.Add(w); 
            LinearLayoutWrap.AddChild(btnEntity, llEntity, ll, w); 
            PlayerAccessTrainButton acBtn = new PlayerAccessTrainButton(t); 

            w.SetComponent<Outline>(btnEntity, new Outline()); 
            w.SetComponent<TextBox>(btnEntity, new TextBox(acBtn.GetMessage())); 
            w.SetComponent<Button>(btnEntity, new Button()); 
            w.SetComponent<PlayerAccessTrainButton>(btnEntity, acBtn); 
            w.SetComponent<Frame>(btnEntity, new Frame(0, 0, buttonWidth, buttonHeight)); 

        }

        return llEntity; 
    }

    public static void Register(World world) {

        Type[] ts = [typeof(DrawEmbarkMessage)]; 
        Action<World, int> tf = (w, e) => {
            DrawEmbarkMessage msg = w.GetComponent<DrawEmbarkMessage>(e); 
            Draw(msg, w);
            w.RemoveEntity(e); 
        }; 
        world.AddSystem(ts, tf); 

    }

}
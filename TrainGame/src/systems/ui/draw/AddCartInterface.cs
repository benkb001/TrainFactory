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

public static class DrawAddCartInterfaceSystem {
    public static Type[] Ts = [typeof(DrawAddCartInterfaceMessage)]; 
    public static Action<World, int> Tf = (w, e) => {
        SceneSystem.EnterScene(w, SceneType.CartInterface);

        
        DrawAddCartInterfaceMessage dm = w.GetComponent<DrawAddCartInterfaceMessage>(e); 
        Train CartDest = dm.CartDest; 
        City CartSource = dm.CartSource; 

        int menuEnt = EntityFactory.Add(w); 
        w.SetComponent<Menu>(menuEnt, new Menu(train: CartDest));

        LinearLayout ll = new LinearLayout("horizontal", "alignLow"); 

        int llEntity = EntityFactory.Add(w); 
        w.SetComponent<LinearLayout>(llEntity, ll); 

        Vector2 topleft = w.GetCameraTopLeft(); 

        float containerWidth = w.ScreenWidth - 20f; 
        float containerHeight = containerWidth / 4f; 
        float labelHeight = containerHeight / 4f; 
        float labelWidth = labelHeight * 4f; 

        ll.Padding = containerWidth / 10f; 

        Vector2 labelPosition = topleft + new Vector2(10f, 10f); 

        Vector2 containerPosition = labelPosition + new Vector2(0f, labelHeight); 

        int labelEntity = EntityFactory.Add(w); 
        w.SetComponent<Frame>(labelEntity, new Frame(labelPosition, labelWidth, labelHeight)); 
        w.SetComponent<TextBox>(labelEntity, new TextBox($"Click a cart to add it to {CartDest.Id}!"));
        w.SetComponent<Outline>(labelEntity, new Outline()); 
        
        w.SetComponent<Frame>(llEntity, new Frame(containerPosition, containerWidth, containerHeight)); 

        foreach (Cart cart in CartSource.Carts) {
            Console.WriteLine($"Added cart"); 
            string type = cart.Type.ToString(); 

            int cEntity = EntityFactory.Add(w); 
            w.SetComponent<Outline>(cEntity, new Outline()); 
            w.SetComponent<TextBox>(cEntity, new TextBox(type)); 
            w.SetComponent<Button>(cEntity, new Button()); 
            w.SetComponent<AddCartButton>(cEntity, new AddCartButton(CartDest, CartSource, cart)); 
            ll.AddChild(cEntity); 
        }

        LinearLayoutWrap.ResizeChildren(llEntity, w); 
        w.RemoveEntity(e); 
    }; 
}
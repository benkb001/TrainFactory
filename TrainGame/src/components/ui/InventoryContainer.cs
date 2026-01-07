namespace TrainGame.Components; 

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public class InventoryContainer<T> where T : IInventorySource {
    public int Index; 
    private T src; 
    private InventoryView invView; 
    private int pageForwardEntity; 
    private int pageBackwardEntity; 

    public int PageForwardEnt => pageForwardEntity; 
    public int PageBackwardEnt => pageBackwardEntity; 

    public InventoryContainer(T src, InventoryView invView, int Index = 0) {
        this.src = src; 
        this.invView = invView; 
        this.Index = Index; 
    }

    public List<Inventory> GetInventories() {
        return src.GetInventories();
    }

    public T GetSource() {
        return src; 
    }

    public Inventory GetCur() {
        return src.GetInventories()[Index];
    }

    public int GetParentEntity() {
        return invView.GetParentEntity();
    }

    public void Redraw(int index, World w) {
        invView.Clear(w); 
        Vector2 position = invView.GetPosition(w);

        Inventory inv = GetInventories()[index];
        (float width, float height) = InventoryWrap.GetUI(inv); 

        DrawInventoryContainerMessage<T> dm = new DrawInventoryContainerMessage<T>(
            src,
            position,
            Width: width, 
            Height: height, 
            Entity: invView.GetParentEntity(),
            Index: index
        ); 

        int dmEnt = EntityFactory.Add(w); 
        w.SetComponent<DrawInventoryContainerMessage<T>>(dmEnt, dm); 
    }

    public void SetPageForwardEntity(int pageForwardEntity) {
        this.pageForwardEntity = pageForwardEntity; 
    }

    public void SetPageBackwardEntity(int pageBackwardEntity) {
        this.pageBackwardEntity = pageBackwardEntity; 
    }
}
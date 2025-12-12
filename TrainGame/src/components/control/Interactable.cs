namespace TrainGame.Components;

public class Interactable {
    public bool Interacted;
    public string ItemId;
    public int ItemCount;

    public Interactable(bool Interacted = false, string ItemId = "", int ItemCount = 0) {
        this.Interacted = Interacted;
        this.ItemId = ItemId; 
        this.ItemCount = ItemCount;  
    }
}
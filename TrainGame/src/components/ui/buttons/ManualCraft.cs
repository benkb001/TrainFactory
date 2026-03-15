namespace TrainGame.Components;

//TODO: Remove
public class ManualCraftButton {
    public float Completion; 
    private int pbEntity; 
    public int PBEntity => pbEntity; 

    public ManualCraftButton(int pbEntity) {
        this.Completion = 0f; 
        this.pbEntity = pbEntity;
    }
}
namespace TrainGame.Components;

public class LLChild {
    public readonly int ParentEntity; 
    public int Depth = 0;

    public LLChild(int ParentEntity) {
        this.ParentEntity = ParentEntity; 
    }
}
namespace TrainGame.Components;

//the shooter should have bullet trait fractal, 
//and split, and the split shoot pattern's bullets 
//should have fractal as well 
public class Fractal : IBulletTrait {
    public float SplitChancePerFrame; 
    public float RecurseFactor; 

    public Fractal(float SplitChancePerFrame, float RecurseFactor) {
        this.SplitChancePerFrame = SplitChancePerFrame; 
        this.RecurseFactor = RecurseFactor;
    }

    public Fractal Clone() {
        return new Fractal(SplitChancePerFrame, RecurseFactor);
    }
}
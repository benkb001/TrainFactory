namespace TrainGame.Components;

public class NextDrawTestControl {
    private int curTest; 

    public NextDrawTestControl(int c) {
        curTest = c; 
    }

    public int GetCurTest() {
        return curTest; 
    }
}
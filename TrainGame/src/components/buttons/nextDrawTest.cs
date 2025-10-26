namespace TrainGame.Components;

public class NextDrawTestButton {
    private int curTest; 

    public NextDrawTestButton() {
        curTest = 0; 
    }

    public NextDrawTestButton(int t) {
        curTest = t; 
    }

    public int GetCurTest() {
        return curTest; 
    }
}
namespace TrainGame.Utils; 

using System.Diagnostics;

public class GameClock {
    private Stopwatch stopwatch;
    private double virtualSecondsPassed = 0;
    private double virtualMillisecondsPassed = 0; 

    public GameClock() {
        stopwatch = Stopwatch.StartNew(); // starts immediately
    }

    // Get total elapsed time in seconds
    public double TotalSeconds => stopwatch.Elapsed.TotalSeconds + virtualSecondsPassed;

    // Get total elapsed time in milliseconds
    public double TotalMilliseconds => stopwatch.Elapsed.TotalMilliseconds + virtualMillisecondsPassed;

    // Restart the clock if needed
    public void Reset() {
        stopwatch.Restart();
    }

    public void PassTime(double seconds = 0, double milliseconds = 0) {
        virtualSecondsPassed += seconds; 
        virtualMillisecondsPassed += milliseconds; 
    }
}
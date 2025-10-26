namespace TrainGame.Utils; 

using System.Diagnostics;

public class GameClock
{
    private Stopwatch stopwatch;

    public GameClock()
    {
        stopwatch = Stopwatch.StartNew(); // starts immediately
    }

    // Get total elapsed time in seconds
    public double TotalSeconds => stopwatch.Elapsed.TotalSeconds;

    // Get total elapsed time in milliseconds
    public double TotalMilliseconds => stopwatch.Elapsed.TotalMilliseconds;

    // Restart the clock if needed
    public void Reset() => stopwatch.Restart();
}
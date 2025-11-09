namespace TrainGame.Utils; 

//TODO: TEST ! ! !
public class WorldTime {
    public int Days => days; 
    public int Hours => hours; 
    public int Minutes => minutes; 

    private int days = 0; 
    private int hours = 0; 
    private int minutes = 0; 
    private int ticks = 0; 

    public WorldTime(int days = 0, int hours = 0, int minutes = 0, int ticks = 0) {
        this.days = days; 
        this.hours = hours; 
        this.minutes = minutes; 
        this.ticks = ticks; 
    }

    public void Update() {
        ticks++; 
        if (ticks >= 60) {
            ticks = 0; 
            minutes++; 
        }
        if (minutes >= 60) {
            minutes = 0; 
            hours++; 
        }
        if (hours >= 24) {
            hours = 0; 
            days++; 
        }
    }

    public static WorldTime operator -(WorldTime a, WorldTime b) {
        int ticks = a.ticks - b.ticks; 
        int minutes = a.minutes - b.minutes;
        int hours = a.hours - b.hours; 
        int days = a.days - b.days; 

        if (ticks < 0) {
            ticks += 60;
            minutes--; 
        }

        if (minutes < 0) {
            minutes += 60; 
            hours--; 
        }

        if (hours < 0) {
            hours += 24; 
            days--; 
        }

        return new WorldTime(days, hours, minutes, ticks); 
    }

    public static WorldTime operator +(WorldTime a, WorldTime b) {
        int ticks = a.ticks + b.ticks; 
        int minutes = a.minutes + b.minutes;
        int hours = a.hours + b.hours; 
        int days = a.days + b.days; 

        if (ticks >= 60) {
            ticks -= 60;
            minutes++; 
        }

        if (minutes >= 60) {
            minutes -= 60; 
            hours++; 
        }

        if (hours >= 24) {
            hours -= 24; 
            days++; 
        }

        return new WorldTime(days, hours, minutes, ticks); 
    }

    public WorldTime Clone() {
        return new WorldTime(days, hours, minutes, ticks); 
    }

    public int InTicks() {
        int ticks = 0; 
        ticks += this.ticks; 
        ticks += this.minutes * 60; 
        ticks += this.hours * 60 * 60; 
        ticks += this.days * 24 * 60 * 60; 
        return ticks; 
    }

    public float InHours() {
        return hours + (minutes / 60f) + (ticks / 3600f); 
    }

    public override string ToString() {
        return $"days: {days}, hours: {hours}, minutes: {minutes}, ticks: {ticks}"; 
    }
}
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
    private int militicks = 0;
    private int militicksPerUpdate = 1000; 

    private WorldTime(int days = 0, int hours = 0, int minutes = 0, int ticks = 0) {
        this.days = days + (hours / 24);
        this.hours = (hours % 24) + (minutes / 60); 
        this.minutes = (minutes % 60) + (ticks / 60); 
        this.ticks = ticks % 60; 
    }

    public WorldTime((int days, int hours, int minutes, int ticks) t) : 
        this(t.days, t.hours, t.minutes, t.ticks) {}

    private static (int, int, int, int) toInt(float days, float hours, float minutes, float ticks) {
        (int, float) separate(float time) {
            int whole = (int)time; 
            float part = time - whole; 
            return (whole, part); 
        }

        (int daysWhole, float daysPart) = separate(days); 
        hours += daysPart * 24f; 
        (int hoursWhole, float hoursPart) = separate(hours); 
        minutes += hoursPart * 60f; 
        (int minutesWhole, float minutesPart) = separate(minutes); 
        ticks += minutesPart * 60f; 
        (int ticksWhole, float ticksPart) = separate(ticks); 
        
        return (daysWhole, hoursWhole, minutesWhole, ticksWhole); 
    }

    public WorldTime(float days = 0f, float hours = 0f, float minutes = 0f, 
        float ticks = 0f) : this(toInt(days, hours, minutes, ticks)) {}

    public void SetMiliticksPerUpdate(int mt) {
        this.militicksPerUpdate = mt; 
    }

    public void Update() {
        militicks += militicksPerUpdate; 

        int dTicks = militicks / 1000; 
        militicks = militicks % 1000; 
        ticks += dTicks; 

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

    public bool IsAfterOrAt(WorldTime other) {
        return (days > other.days) || 
        (days >= other.days && hours > other.hours) || 
        (days >= other.days && hours >= other.hours && minutes > other.minutes ) || 
        (days >= other.days && hours >= other.hours && minutes >= other.minutes && ticks >= other.ticks); 
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

    public static int operator /(WorldTime a, WorldTime b) {
        int aTicks = a.InTicks(); 
        int bTicks = b.InTicks(); 
        return a / b; 
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
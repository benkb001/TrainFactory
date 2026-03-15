namespace TrainGame.Components;

using TrainGame.Utils;

public class Parrier {
    private WorldTime canParry; 
    private WorldTime cooldown; 
    private WorldTime duration; 
    private WorldTime parryDecrease; 
    private WorldTime endParry; 
    private WorldTime startedParry; 
    private bool parrying = false; 
    public bool Parrying => parrying; 

    public Parrier() {
        canParry = new WorldTime(); 
        endParry = new WorldTime(); 
        startedParry = new WorldTime(); 
        cooldown = new WorldTime(minutes: 3); 
        duration = new WorldTime(ticks: 30); 
        parryDecrease = new WorldTime(ticks: 30); 
    }

    public bool CanParry(WorldTime now) {
        return now.IsAfterOrAt(canParry); 
    }

    public void StartParry(WorldTime now) {
        startedParry = now.Clone(); 
        canParry = now + cooldown; 
        endParry = now + duration;
        parrying = true; 
    }

    public void Parry() {
        canParry = canParry - parryDecrease; 
    }

    public bool ParryEnded(WorldTime now) {
        if (parrying && now.IsAfterOrAt(endParry)) {
            parrying = false; 
            return true; 
        }
        return false; 
    }

    public float PercentCooldownComplete(WorldTime now) {
        WorldTime waitTime = canParry - startedParry;
        return ((now - startedParry) / waitTime); 
    }
}
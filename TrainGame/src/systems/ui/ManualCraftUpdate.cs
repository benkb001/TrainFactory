namespace TrainGame.Systems; 

using TrainGame.ECS; 
using TrainGame.Components; 

public class ManualCraftUpdateSystem {
    public static void Register(World w) {
        w.AddSystem([typeof(ManualCraftButton), typeof(Button), typeof(Machine), typeof(Active)], (w, e) => {
            
            Button b = w.GetComponent<Button>(e); 
            if (b.TicksHeld > 10) {
                ManualCraftButton craftBtn = w.GetComponent<ManualCraftButton>(e); 
                craftBtn.Completion += .1f; 
                if (craftBtn.Completion >= 1) {
                    Machine m = w.GetComponent<Machine>(e); 
                    m.Inv.Add(m.ProductItemId, 1);
                    craftBtn.Completion = 0f; 
                }
                ProgressBar pb = w.GetComponent<ProgressBar>(craftBtn.PBEntity); 
                pb.Completion = craftBtn.Completion; 
            }
        });
    }
}
namespace TrainGame.Systems; 

using TrainGame.ECS; 
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 

public static class SetTrainProgramClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<SetTrainProgramButton>(w, (w, e) => {
            SetTrainProgramButton btn = w.GetComponent<SetTrainProgramButton>(e); 

            Train t = btn.GetTrain(); 
            Inventory inv = t.ComingFrom.Inv; 
            bool hasMotherboard = false; 
            
            if (inv.ItemCount(ItemID.Motherboard) >= 1) {
                inv.Take(ItemID.Motherboard, 1); 
                hasMotherboard = true; 
            } else if (t.Inv.ItemCount(ItemID.Motherboard) >= 1) {
                t.Inv.Take(ItemID.Motherboard, 1); 
                hasMotherboard = true; 
            }

            if (hasMotherboard) {
                string program = TAL.Scripts[btn.ScriptName];
                TAL.SetTrainProgram(program, t, w); 
                MakeMessage.Add<DrawTrainInterfaceMessage>(w, new DrawTrainInterfaceMessage(t)); 
            }
        });
    }
}
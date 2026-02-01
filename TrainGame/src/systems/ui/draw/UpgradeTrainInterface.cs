namespace TrainGame.Systems;

using System; 
using System.Drawing; 
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TrainGame.ECS;  
using TrainGame.Components; 
using TrainGame.Utils; 
using TrainGame.Constants; 
using TrainGame.Systems;
using TrainGame.Callbacks; 

public class UpgradeTrainInterfaceData : IInterfaceData {
    private Train train; 

    public Train GetTrain() => train;

    public UpgradeTrainInterfaceData(Train train) {
        this.train = train;
    }

    public Menu GetMenu() {
        return new Menu(train);
    }

    public SceneType GetSceneType() => SceneType.UpgradeTrainInterface;
}

public class UpgradeTrainPowerButton {
    private Train train; 
    public Train GetTrain() => train;

    public UpgradeTrainPowerButton(Train t) {
        this.train = t; 
    }

    public bool TryUpgrade() {
        if (train.ComingFrom.Inv.Take(ItemID.Engine, 1).Count == 1) {
            train.UpgradePower(Constants.PowerPerEngine);
            return true;
        }
        return false;
    }
}

public class UpgradeFuelConsumptionButton {
    private Train train; 
    public Train GetTrain() => train;

    public UpgradeFuelConsumptionButton(Train t) {
        this.train = t; 
    }

    public bool TryUpgrade() {
        if (train.ComingFrom.Inv.Take(ItemID.CombustionController, 1).Count == 1) {
            train.UpgradeMassMilesPerFuel(Constants.MassMilesPerFuelPerCombustionController);
            return true;
        }
        return false;
    }
}

public static class UpgradeTrainPowerClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<UpgradeTrainPowerButton>(w, (w, e, b) => {
            if (b.TryUpgrade()) {
                DrawUpgradeTrainInterfaceSystem.AddMessage(w, b.GetTrain());
            }

        });
    }
}

public static class UpgradeFuelConsumptionClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<UpgradeFuelConsumptionButton>(w, (w, e, b) => {
            if (b.TryUpgrade()) {
                DrawUpgradeTrainInterfaceSystem.AddMessage(w, b.GetTrain());
            }
        });
    }
}

public static class DrawUpgradeTrainInterfaceSystem {
    public static void Register(World w) {
        DrawInterfaceSystem.Register<UpgradeTrainInterfaceData>(w, (w, e, d) => {
            Train t = d.GetTrain();
            LinearLayoutContainer outer = LinearLayoutWrap.AddOuter(w);
            
            string summary = t.GetSummary();
            int sumEnt = EntityFactory.AddUI(w, Vector2.Zero, 200, 200, setOutline: true, text: summary);
            outer.AddChild(sumEnt, w);

            LinearLayoutContainer stack = LinearLayoutWrap.Add(
                w, 
                Vector2.Zero,
                outer.LLWidth,
                outer.LLHeight / 2f,
                direction: "horizontal",
                outline: false
            );
            outer.AddChild(stack.GetParentEntity(), w);

            float btnWidth = 200; 
            float btnHeight = 100;
            int addCartBtnEnt = EntityFactory.AddUI(w, Vector2.Zero, btnWidth, btnHeight, setButton: true, 
                setOutline: true, text: "Add Cart?");
            w.SetComponent<AddCartInterfaceButton>(addCartBtnEnt, new AddCartInterfaceButton(t, t.ComingFrom));
            stack.AddChild(addCartBtnEnt, w);

            int upgradeFuelEnt = EntityFactory.AddUI(w, Vector2.Zero, btnWidth, btnHeight, setButton: true, 
                setOutline: true, text: "Upgrade Fuel Consumption? Requires 1 Combustion Controller"); 
            w.SetComponent<UpgradeFuelConsumptionButton>(upgradeFuelEnt, new UpgradeFuelConsumptionButton(t));
            stack.AddChild(upgradeFuelEnt, w);

            int upgradePowerEnt = EntityFactory.AddUI(w, Vector2.Zero, btnWidth, btnHeight, setButton: true, 
                setOutline: true, text: "Upgrade Speed? Requires 1 Engine");
            w.SetComponent<UpgradeTrainPowerButton>(upgradePowerEnt, new UpgradeTrainPowerButton(t));
            stack.AddChild(upgradePowerEnt, w);
        });
    }

    public static void AddMessage(World w, Train t) {
        MakeMessage.Add<DrawInterfaceMessage<UpgradeTrainInterfaceData>>(w, 
            new DrawInterfaceMessage<UpgradeTrainInterfaceData>(new UpgradeTrainInterfaceData(t)));
    }
}
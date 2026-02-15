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
    private bool exponential;
    public Train GetTrain() => train;

    public UpgradeTrainPowerButton(Train t, bool exponential = false) {
        this.train = t; 
        this.exponential = exponential; 
    }

    public bool TryUpgrade() {
        if (exponential && train.ComingFrom.Inv.Take(ItemID.AirResistor, 1).Count == 1) {
            train.UpgradePowerExponential();
            return true;
        } else if (train.ComingFrom.Inv.Take(ItemID.Engine, 1).Count == 1) {
            train.UpgradePower(Constants.PowerPerEngine);
            return true;
        }
        return false;
    }
}

public class UpgradeFuelConsumptionButton {
    private Train train; 
    private bool exponential;

    public bool Exponential => exponential;
    public Train GetTrain() => train;

    public UpgradeFuelConsumptionButton(Train t, bool exponential = false) {
        this.train = t; 
        this.exponential = exponential;
    }

    public bool TryUpgrade() {
        if (exponential && train.ComingFrom.Inv.Take(ItemID.AntiGravity, 1).Count == 1) {
            train.UpgradeMassMilesPerFuelExponential();
            return true;
        } else if (train.ComingFrom.Inv.Take(ItemID.CombustionController, 1).Count == 1) {
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

            LinearLayoutContainer row1 = LinearLayoutWrap.Add(
                w, 
                Vector2.Zero,
                outer.LLWidth,
                outer.LLHeight / 4f,
                direction: "horizontal",
                outline: false
            );
            outer.AddChild(row1.GetParentEntity(), w);

            float btnWidth = 150; 
            float btnHeight = 100;

            int addButton(string text) {
                return EntityFactory.AddUI(w, Vector2.Zero, btnWidth, btnHeight, setButton: true, 
                    setOutline: true, text: text);
            }
            int addCartBtnEnt = addButton("Add Cart?");
            w.SetComponent<AddCartInterfaceButton>(addCartBtnEnt, new AddCartInterfaceButton(t, t.ComingFrom));
            row1.AddChild(addCartBtnEnt, w);

            int upgradeFuelEnt = addButton("Upgrade Fuel Consumption? Requires 1 Combustion Controller");
            w.SetComponent<UpgradeFuelConsumptionButton>(upgradeFuelEnt, new UpgradeFuelConsumptionButton(t));
            row1.AddChild(upgradeFuelEnt, w);

            int upgradePowerEnt = addButton("Upgrade Speed? Requires 1 Engine");
            w.SetComponent<UpgradeTrainPowerButton>(upgradePowerEnt, new UpgradeTrainPowerButton(t));
            row1.AddChild(upgradePowerEnt, w);

            LinearLayoutContainer row2 = LinearLayoutWrap.Add(
                w,
                Vector2.Zero,
                outer.LLWidth,
                outer.LLHeight / 4f,
                direction: "horizontal",
                outline: false
            );
            outer.AddChild(row2.GetParentEntity(), w);

            int upgradeInvExpoEnt = addButton(
                $"Multiply Inventory Sizes By {Constants.ExponentialInvSizeUpgradeFactor}? Requires 1 {ItemID.PocketDimension}");
            w.SetComponent<UpgradeInventoryExponentialButton>(upgradeInvExpoEnt, new UpgradeInventoryExponentialButton(train: t));
            row2.AddChild(upgradeInvExpoEnt, w);

            int upgradeFuelExpoEnt = addButton(
                $"Multiply Miles Per Fuel By {Constants.ExponentialMilesPerFuelUpgradeFactor}? Requires 1 {ItemID.AntiGravity}");
            w.SetComponent<UpgradeFuelConsumptionButton>(upgradeFuelExpoEnt, new UpgradeFuelConsumptionButton(t, exponential: true));
            row2.AddChild(upgradeFuelExpoEnt, w);

            int upgradePowerExpoEnt = addButton(
                $"Multiply Power by {Constants.ExponentialTrainPowerUpgradeFactor}? Requires 1 {ItemID.AirResistor}");
            w.SetComponent<UpgradeTrainPowerButton>(upgradePowerExpoEnt, new UpgradeTrainPowerButton(t, exponential: true));
            row2.AddChild(upgradePowerExpoEnt, w);
        });
    }

    public static void AddMessage(World w, Train t) {
        MakeMessage.Add<DrawInterfaceMessage<UpgradeTrainInterfaceData>>(w, 
            new DrawInterfaceMessage<UpgradeTrainInterfaceData>(new UpgradeTrainInterfaceData(t)));
    }
}

public class UpgradeInventoryExponentialButton {
    private Inventory inv; 
    private Train train;
    public bool IsTrainInv => train != null; 
    public Train GetTrain() => train;

    public UpgradeInventoryExponentialButton(Inventory inv = null, Train train = null) {
        this.inv = inv; 
        this.train = train;
        if (inv == null) {
            if (train != null) {
                this.inv = train.ComingFrom.Inv;
            } else {
                throw new InvalidOperationException("""
                    Cannot initialize an 'UpgradeInventoryExponentialButton' without a 
                    non-null reference to either a Train or Inventory object
                    """
                );
            }
            
        }
    }

    public bool TryUpgrade() {
        if (inv.Take(ItemID.PocketDimension, 1).Count == 1) {
            if (train != null) {
                train.UpgradeInventoryExponential(); 
            } else {
                inv.UpgradeExponential();
            }
            return true;
        }
        return false;
    }
}

public static class UpgradeInventoryExponentialClickSystem {
    public static void Register(World w) {
        ClickSystem.Register<UpgradeInventoryExponentialButton>(w, (w, e, b) => {
            if (b.TryUpgrade() && b.IsTrainInv) {
                DrawUpgradeTrainInterfaceSystem.AddMessage(w, b.GetTrain());
            }
        });
    }
}
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
    public readonly int TrainEntity; 

    public Train GetTrain() => train;

    public UpgradeTrainInterfaceData(Train train, int trainEnt) {
        this.train = train;
        TrainEntity = trainEnt;
    }

    public Menu GetMenu() {
        return new Menu(train, TrainEntity: TrainEntity);
    }

    public SceneType GetSceneType() => SceneType.UpgradeTrainInterface;
}

public class UpgradeTrainPowerButton : IUpgradeTrainButton {
    private Train train; 
    private bool exponential;
    private int trainEntity;
    private Inventory depot;
    
    public int GetTrainEntity() => trainEntity;
    public Train GetTrain() => train;

    public UpgradeTrainPowerButton(Train t, int trainEnt, Inventory depot, bool exponential = false) {
        this.train = t; 
        this.exponential = exponential; 
        this.trainEntity = trainEnt;
        this.depot = depot;
    }

    public bool TryUpgrade() {
        if (exponential && depot.Take(ItemID.AirResistor, 1).Count == 1) {
            train.UpgradePowerExponential();
            return true;
        } else if (depot.Take(ItemID.Engine, 1).Count == 1) {
            train.UpgradePower(Constants.PowerPerEngine);
            return true;
        }
        return false;
    }
}

public class UpgradeFuelConsumptionButton : IUpgradeTrainButton {
    private Train train; 
    private bool exponential;
    private int trainEntity;
    private Inventory depot;

    public bool Exponential => exponential;
    public Train GetTrain() => train;
    public int GetTrainEntity() => trainEntity;

    public UpgradeFuelConsumptionButton(Train t, int trainEntity, Inventory depot, bool exponential = false) {
        this.train = t; 
        this.exponential = exponential;
        this.trainEntity = trainEntity;
        this.depot = depot;
    }

    public bool TryUpgrade() {
        if (exponential && depot.Take(ItemID.AntiGravity, 1).Count == 1) {
            train.UpgradeMassMilesPerFuelExponential();
            return true;
        } else if (depot.Take(ItemID.CombustionController, 1).Count == 1) {
            train.UpgradeMassMilesPerFuel(Constants.MassMilesPerFuelPerCombustionController);
            return true;
        }
        return false;
    }
}

public interface IUpgradeTrainButton {
    bool TryUpgrade();
    Train GetTrain();
    int GetTrainEntity();
}

public static class UpgradeTrainClickSystem {
    public static void Register<T>(World w) where T : IUpgradeTrainButton {
        ClickSystem.Register<T>(w, (w, e, b) => {
            if (b.TryUpgrade()) {
                DrawUpgradeTrainInterfaceSystem.AddMessage(w, b.GetTrain(), b.GetTrainEntity());
            }
        });
    }
}

public static class DrawUpgradeTrainInterfaceSystem {
    public static void Register(World w) {
        DrawInterfaceSystem.Register<UpgradeTrainInterfaceData>(w, (w, e, d) => {
            Train t = d.GetTrain();
            int trainEnt = d.TrainEntity;
            LinearLayoutContainer outer = LinearLayoutContainer.AddOuter(w);
            City comingFrom = w.GetComponent<ComingFromCity>(trainEnt);
            
            string summary = t.GetSummary();
            int sumEnt = EntityFactory.AddUI(w, Vector2.Zero, 200, 200, setOutline: true, text: summary);
            outer.AddChild(sumEnt, w);

            LinearLayoutContainer row1 = LinearLayoutContainer.Add(
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
            w.SetComponent<AddCartInterfaceButton>(addCartBtnEnt, new AddCartInterfaceButton(t, trainEnt, comingFrom));            
            row1.AddChild(addCartBtnEnt, w);

            int upgradeFuelEnt = addButton("Upgrade Fuel Consumption? Requires 1 Combustion Controller");
            w.SetComponent<UpgradeFuelConsumptionButton>(upgradeFuelEnt, new UpgradeFuelConsumptionButton(t, trainEnt, comingFrom.Inv));
            row1.AddChild(upgradeFuelEnt, w);

            int upgradePowerEnt = addButton("Upgrade Speed? Requires 1 Engine");
            w.SetComponent<UpgradeTrainPowerButton>(upgradePowerEnt, new UpgradeTrainPowerButton(t, trainEnt, comingFrom.Inv));
            row1.AddChild(upgradePowerEnt, w);

            LinearLayoutContainer row2 = LinearLayoutContainer.Add(
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
            w.SetComponent<UpgradeInventoryExponentialButton>(upgradeInvExpoEnt, 
                new UpgradeInventoryExponentialButton(comingFrom.Inv, train: t));
            row2.AddChild(upgradeInvExpoEnt, w);

            int upgradeFuelExpoEnt = addButton(
                $"Multiply Miles Per Fuel By {Constants.ExponentialMilesPerFuelUpgradeFactor}? Requires 1 {ItemID.AntiGravity}");
            w.SetComponent<UpgradeFuelConsumptionButton>(upgradeFuelExpoEnt, 
                new UpgradeFuelConsumptionButton(t, trainEnt, comingFrom.Inv, exponential: true));
            row2.AddChild(upgradeFuelExpoEnt, w);

            int upgradePowerExpoEnt = addButton(
                $"Multiply Power by {Constants.ExponentialTrainPowerUpgradeFactor}? Requires 1 {ItemID.AirResistor}");
            w.SetComponent<UpgradeTrainPowerButton>(upgradePowerExpoEnt, 
                new UpgradeTrainPowerButton(t, trainEnt, comingFrom.Inv, exponential: true));
            row2.AddChild(upgradePowerExpoEnt, w);
        });
    }

    public static void AddMessage(World w, Train t, int trainEnt) {
        MakeMessage.Add<DrawInterfaceMessage<UpgradeTrainInterfaceData>>(w, 
            new DrawInterfaceMessage<UpgradeTrainInterfaceData>(new UpgradeTrainInterfaceData(t, trainEnt)));
    }
}

public class UpgradeInventoryExponentialButton {
    private Inventory inv; 
    private Train train;
    public bool IsTrainInv => train != null; 
    public Train GetTrain() => train;
    public readonly int TrainEntity;

    //ICKY: Can this just be two separate classes and an IUpgradeInventoryExponential interface ??
    public UpgradeInventoryExponentialButton(Inventory inv, Train train = null, int trainEntity = -1) {
        this.inv = inv; 
        this.train = train;
        this.TrainEntity = trainEntity;
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
                DrawUpgradeTrainInterfaceSystem.AddMessage(w, b.GetTrain(), b.TrainEntity);
            }
        });
    }
}
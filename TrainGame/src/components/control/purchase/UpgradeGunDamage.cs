namespace TrainGame.Components;

using TrainGame.ECS;
using TrainGame.Constants;
using TrainGame.Utils;

public class PurchaseUpgradeGunDamage : IBuyable {
    public readonly string ID;

    public PurchaseUpgradeGunDamage(string ID) {
        this.ID = ID;
    }
}

public static class PurchaseUpgradeGunDamageWrap {
    public static void SetPurchaseText(World w, PlayerGun pg, string id, int e) {
        string msg = 
            pg.DamageLevel < pg.MaxDamageLevel ? 
            $"Upgrade {id} damage?\n{Util.FormatMap(VendorID.UpgradeGunDamageCost(id, pg.DamageLevel))}" :
            $"Maxed Out Damage for {id}";
        
        w.SetComponent<TextBox>(e, new TextBox(msg));
    }
}
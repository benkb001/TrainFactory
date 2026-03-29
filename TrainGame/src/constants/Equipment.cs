namespace TrainGame.Constants;

using System.Collections.Generic;
using TrainGame.Components;

public static class EquipmentID {
    public static List<string> Armor = new() {
        ItemID.Armor1, ItemID.Armor2, ItemID.Armor3
    };

    public static void InitMaps() {
        EquipmentSlot<Armor>.EquipmentMap = new() {
            [ItemID.Armor1] = new Armor(5),
            [ItemID.Armor2] = new Armor(20),
            [ItemID.Armor3] = new Armor(40)
        };
    }
}
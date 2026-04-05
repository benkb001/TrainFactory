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
using TrainGame.Callbacks; 

public class VendorInteractSystem {
    public static void Register(World w) {
        EnterInterfaceInteractSystem.Register<VendorInterfaceData>(w); 
    }
}

public class VendorWrap {
    private static VendorInterfaceData getData(World w, string vendorID) {
        Inventory Source;
        Inventory Destination; 
        City city; 
        switch (vendorID) {
            case VendorID.WeaponCraftsman:
                city = CityWrap.GetCityWithPlayer(w);
                Source = city.Inv;
                Destination = InventoryWrap.GetByID(w, Constants.EquipmentInvID<PlayerGun>());

                break;
            case VendorID.MineralCollector: 
            case VendorID.ArmorCraftsman:
                city = CityWrap.GetCityWithPlayer(w);
                Source = city.Inv;
                Destination = city.Inv;
                break;
            default: 
                throw new InvalidOperationException($"Vendor ID {vendorID} not handled");
        }
            
        return new VendorInterfaceData(vendorID, Source, Destination, city);
    }

    public static int Draw(World w, Vector2 pos, string vendorID) {
        VendorInterfaceData data = getData(w, vendorID);
        int vendorEnt = EntityFactory.AddUI(w, pos, Constants.TileWidth, Constants.TileWidth, 
        setOutline: true, setInteractable: true, setCollidable: true, text: vendorID);
        EnterInterfaceInteractable<VendorInterfaceData> interactable = 
            new EnterInterfaceInteractable<VendorInterfaceData>(data);
        w.SetComponent<EnterInterfaceInteractable<VendorInterfaceData>>(vendorEnt, interactable); 
        return vendorEnt; 
    }
}
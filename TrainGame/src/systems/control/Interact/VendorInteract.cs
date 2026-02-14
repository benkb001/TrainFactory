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
    public static int Draw(World w, Vector2 pos, City city, string vendorID) {
        int vendorEnt = EntityFactory.AddUI(w, pos, Constants.TileWidth, Constants.TileWidth, 
        setOutline: true, setInteractable: true, setCollidable: true, text: vendorID);
        EnterInterfaceInteractable<VendorInterfaceData> interactable = 
            new EnterInterfaceInteractable<VendorInterfaceData>(
                new VendorInterfaceData(city, vendorID));
        w.SetComponent<EnterInterfaceInteractable<VendorInterfaceData>>(vendorEnt, interactable); 
        return vendorEnt; 
    }
}
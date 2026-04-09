namespace TrainGame.Constants;

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using TrainGame.Components;
using TrainGame.ECS;
using TrainGame.Utils;
using TrainGame.Systems;

public static class Bootstrap {
    public static void InitWorld(World w) {
        EquipmentID.InitMaps();
        Dictionary<string, (int, City)> cities = new(); 
        Dictionary<string, (int, Machine)> machines = new(); 

        //initialize cities and machines 
        foreach (KeyValuePair<string, CityArg> kvp in CityID.CityMap) {
            string cityId = kvp.Key; 
            CityArg args = kvp.Value; 

            Inventory inv = new Inventory($"{cityId} Depot", Constants.CityInvRows, Constants.CityInvCols); 
            int invEnt = EntityFactory.AddData<Inventory>(w, inv); 
            w.SetComponent<Inventory>(invEnt, inv); 

            City c = new City(cityId, inv, args.UiX, args.UiY, args.RealX, args.RealY); 
            int cityEnt = EntityFactory.AddData<City>(w, c); 
            cities[cityId] = (cityEnt, c); 

            foreach (string machineID in args.Machines) {
                Machine m = Machines.Get(inv, machineID); 
                int machineEnt = EntityFactory.AddData<Machine>(w, m); 
                machines[machineID] = (machineEnt, m); 
                w.SetComponent<Machine>(machineEnt, m); 
                c.AddMachine(m); 
            }
        }

        //add city connections 
        foreach (KeyValuePair<string, (int, City)> kvp in cities) {
            string cityId = kvp.Key; 
            (int ent, City city) = kvp.Value; 
            
            foreach (string otherCityID in CityID.CityMap[cityId].AdjacentCities) {
                (int _, City otherCity) = cities[otherCityID]; 
                city.AddConnection(otherCity);
            }
        }

        //add player data

        PlayerWrap.AddData(w);
        PlayerWrap.SetRespawn(w, cities[CityID.Factory].Item2);

        //add one train to factory

        Inventory trainInv = new Inventory("T0", Constants.TrainRows, Constants.TrainCols); 
        int trainInvDataEnt = EntityFactory.AddData<Inventory>(w, trainInv); 

        (int _, City factory) = cities[CityID.Factory];
        
        TrainWrap.AssembleToWorld(w, factory);

        //add some fuel to factory
        factory.Inv.Add(new Inventory.Item(ItemId: ItemID.Fuel, Count: 50)); 
        factory.Inv.Add(VendorID.UpgradeShieldHealAmountCost[0]);

        //add weapons inv

        InventoryWrap.Add(w, Constants.WeaponsInvID, 1, 6, level: 1);

        //set assembler components
        (int locomotiveAssemblerEnt, Machine locomotiveAssembler) = machines[MachineID.LocomotiveAssembler]; 
        w.SetComponent<TrainAssembler>(locomotiveAssemblerEnt, new TrainAssembler(factory, locomotiveAssembler)); 

        (int cargoWagonAssemblerEnt, Machine cargoAssembler) = machines[MachineID.CargoWagonAssembler]; 
        w.SetComponent<CartAssembler>(cargoWagonAssemblerEnt, 
            new CartAssembler(factory, cargoAssembler, CartType.Freight));

        (int liquidAssemblerEnt, Machine liquidAssembler) = machines[MachineID.LiquidWagonAssembler]; 
        w.SetComponent<CartAssembler>(liquidAssemblerEnt, 
            new CartAssembler(factory, liquidAssembler, CartType.Liquid));

        //add player to factory 
        factory.HasPlayer = true; 
    }
}
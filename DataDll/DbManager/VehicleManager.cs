using RedWheels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace DataDll.DbManager
{
    //This is the manager for the VehiclesController
    public class VehicleManager
    {
        dbContext db = new dbContext();
        //Returns all vehicles available
        public List<Vehicles> GetAllVehicles()
        {
            return db.Vehicles.ToList();
        }
        //Returns all vehicles by {vehicleNumber}
        public List<Vehicles> GetVehiclesByModel(int modelId)
        {
            return (from v in db.Vehicles where v.ModelId == modelId select v).ToList();
        }
        //Returns a single vehicle by {vehicleNumber}
        public Vehicles GetVehicle(int vehicleNumber)
        {
            return db.Vehicles.FirstOrDefault(v => v.VehicleNumber == vehicleNumber);
        }
        //Returns counts available cars in by {vehicleNumber}
        public int GetAvailableCnt(int modelId)
        {
            int cnt = 0;
            foreach (Vehicles vehicle in GetVehiclesByModel(modelId))
                if (vehicle.IsAvailable && vehicle.IsFunctional)
                    cnt++;
            return cnt;
        }
        //Adds a new vehicle to the [Vehicles] table
        public Vehicles AddNewVehicle(Vehicles newVehicle)
        {
            if (db.Vehicles.FirstOrDefault
                (v => v.VehicleNumber == newVehicle.VehicleNumber) == null)
            {
                db.Vehicles.Add(newVehicle);
                db.SaveChanges();
                return newVehicle;
            }
            return null;
        }
        //Edits an existing vehicle's changeable fields in the [Vehicles] table
        public Vehicles EditVehicle(Vehicles vehicle)
        {
            Vehicles editedVehicle = db.Vehicles.FirstOrDefault
                (v => v.VehicleNumber == vehicle.VehicleNumber);
            if (editedVehicle != null)
            {
                editedVehicle.CurrentKilos = vehicle.CurrentKilos;
                editedVehicle.VehiclePicture = vehicle.VehiclePicture;
                editedVehicle.IsFunctional = vehicle.IsFunctional;
                editedVehicle.IsAvailable = vehicle.IsAvailable;
                editedVehicle.BranchId = vehicle.BranchId;
                db.SaveChanges();
                return editedVehicle;
            }
            else
                throw new Exception("Vehicle not found");
        }
        //Deletes a single vehicle by {vehicleNumber} if one is found
        public Vehicles DeleteVehicle(int vehicleNumber)
        {
            Vehicles vehicle = db.Vehicles.FirstOrDefault(v => v.VehicleNumber == vehicleNumber);
            if (vehicle != null)
            {
                db.Vehicles.Remove(vehicle);
                foreach (OrderList order in db.OrderList)
                {
                    if (order.VehicleNumber == vehicleNumber)
                        order.VehicleNumber = 0;
                }
                db.SaveChanges();
            }
            return vehicle;
        }
    }
}

using RedWheels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDll.DbManager
{
    //This is the manager for the VehicleTypesController
    public class VehicleTypeManager
    {
        VehicleManager vManager = new VehicleManager();
        dbContext db = new dbContext();
        //Returns all vehicle types available in a reverse order
        public List<VehicleTypes> GetAllVehicleTypes()
        {
            List<VehicleTypes> revList = db.VehicleTypes.ToList();
            revList.Reverse();
            return revList;
        }
        //Returns a single vehicle type by {modelId}
        public VehicleTypes GetVehicleType(int modelId)
        {
            return db.VehicleTypes.FirstOrDefault(v => v.ModelId == modelId);
        }
        /// <summary>Adds a new vehicle type to the [VehicleTypes] table</summary>
        public VehicleTypes AddNewVehicleType(VehicleTypes vehicleType)
        {
            if (db.VehicleTypes.FirstOrDefault
                (v => v.ModelId == vehicleType.ModelId) == null)
            {
                db.VehicleTypes.Add(vehicleType);
                db.SaveChanges();
                return vehicleType;
            }
            return null;
        }
        /// <summary>
        /// Edits an existing vehicle type's DailyCost, DailyDelay and Gear by ModelId
        ///in the [VehicleTypes] table
        /// </summary>
        /// <param name="vehicleType"></param>
        /// <returns>VehicleTypes class</returns>
        public VehicleTypes EditVehicleType(VehicleTypes vehicleType)
        {
            VehicleTypes editedVehicleType = db.VehicleTypes.FirstOrDefault
                (v => v.ModelId == vehicleType.ModelId);
            if (editedVehicleType != null)
            {
                editedVehicleType.Manufacturer = vehicleType.Manufacturer;
                editedVehicleType.ModelName = vehicleType.ModelName;
                editedVehicleType.ProdYear = vehicleType.ProdYear;
                editedVehicleType.DailyCost = vehicleType.DailyCost;
                editedVehicleType.DailyDelay = vehicleType.DailyDelay;
                editedVehicleType.Gear = vehicleType.Gear;
                db.SaveChanges();
                return editedVehicleType;
            }
            else
                throw new Exception("Vehicle type not found");
        }
        //Deletes a single vehicle type by {modelId} if one is found
        public VehicleTypes DeleteVehicleType(int modelId)
        {
            VehicleTypes vehicleTypes = db.VehicleTypes.FirstOrDefault(v => v.ModelId == modelId);
            if (vehicleTypes != null)
            {
                db.VehicleTypes.Remove(vehicleTypes);
                foreach (Vehicles vehicle in db.Vehicles)
                {
                    if (vehicle.ModelId == modelId)
                        vManager.DeleteVehicle(vehicle.VehicleNumber);
                }
                db.SaveChanges();
            }
            return vehicleTypes;
        }
    }
}

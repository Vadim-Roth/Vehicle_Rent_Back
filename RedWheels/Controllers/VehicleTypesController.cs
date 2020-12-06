using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataDll.DbManager;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RedWheels.Controllers
{
    //Extra properties on Vehicle types that is used for the GetAllVehicleTypes method
    public class VehicleTypeEx : VehicleTypes
    {
        //public List<Vehicles> vehicles { get; set; }
        public int available { get; set; }
    }

    /*This controller supports:
     * /VehicleTypes/GetAllVehicleTypes - Receiving all vehicle types existing
     * /VehicleTypes/GetVehicleType/{modelId} - Receiving a specific vehicle type by {modelId}
     * /VehicleTypes/AddNewVehicleType - Adds a new vehicle type to the [VehicleTypes] table
     * /VehicleTypes/EditVehicleType - Edits dailyCost,dailyDelay and gear from objects with existing {modelId}
     * /VehicleTypes/DeleteVehicleType/{modelId} - Deleting a specific vehicle type by {modelId}
    */
    [EnableCors("projCors")]
    [Route("[controller]")]
    [ApiController]

    public class VehicleTypesController : ControllerBase
    {
        VehicleTypeManager manager = new VehicleTypeManager();

        // GET: /VehicleTypes/GetAllVehicleTypes
        // Receiving all vehicle types existing
        [HttpGet][Route("[action]")]
        public IActionResult GetAllVehicleTypes()
        {
            var types = manager.GetAllVehicleTypes();
            var extTypes = new List<VehicleTypeEx>();

            foreach (var i in types)
            {
                var xt = new VehicleTypeEx
                {
                    ModelId = i.ModelId,
                    Manufacturer = i.Manufacturer,
                    ModelName = i.ModelName,
                    DailyCost = i.DailyCost,
                    DailyDelay = i.DailyDelay,
                    ProdYear = i.ProdYear,
                    Gear = i.Gear
                };
               // xt.vehicles = new VehicleManager().GetVehiclesByModel(i.ModelId);
                xt.available = new VehicleManager().GetAvailableCnt(i.ModelId);
                extTypes.Add(xt);
            }
            return Ok(extTypes);

        }
        // GET: /VehicleTypes/GetVehicleType/{modelId}
        // Receiving a specific vehicle type by {modelId} if one is found, if not returns NotFound
        [HttpGet("{modelId}")][Route("[action]/{modelId}")]
        public IActionResult GetVehicleType(int modelId)
        {
            VehicleTypes vehicleType = manager.GetVehicleType(modelId);
            if (vehicleType != null)
                return Ok(vehicleType);
            return NotFound();
        }

        // POST: /VehicleTypes/AddNewVehicleType
        // Adds a new vehicle type to the [VehicleTypes] table if the statement is valid,
        //check ↓ for example
        // {"manufacturer":"Peugeot", "modelName": "Peugeot 208", "dailyCost":200,
        //"dailyDelay":250, "prodYear":2013, "gear": "Manual"}
        [HttpPost][Route("[action]")]
        public IActionResult AddNewVehicleType([FromBody] VehicleTypes vehicleType)
        {
            if (ModelState.IsValid)
                if (manager.AddNewVehicleType(vehicleType) != null)
                    return Created("VehicleTypes/" + vehicleType.ModelId, vehicleType);
            return BadRequest(ModelState);
        }

        // PUT: /VehicleTypes/EditVehicleType
        // Edits dailyCost, dailyDelay and gear by modelId if one is found, check ↓ for example
        // { "modelId": 10,  "dailyCost": 600, "dailyDelay": 700, "gear": "Auto"}
        [HttpPut][Route("[action]")]
        public IActionResult EditVehicleType(VehicleTypes vehicleType)
        {
            var editedVehicleType = manager.EditVehicleType(vehicleType);
            if (ModelState.IsValid) 
                if (editedVehicleType != null)
                    return Ok(editedVehicleType);
                else
                    return NotFound();
            return BadRequest(ModelState);
        }

        // DELETE: /VehicleTypes/DeleteVehicleType/{modelId}
        // Deleting a specific vehicle type by {modelId} if one is found, if not returns NotFound
        [HttpDelete("{modelId}")][Route("[action]/{modelId}")]
        public IActionResult DeleteVehicleType(int modelId)
        {
            if (manager.DeleteVehicleType(modelId) != null)
                return Ok($"VehicleType {modelId} deleted");
            return NotFound();
        }
    }
}

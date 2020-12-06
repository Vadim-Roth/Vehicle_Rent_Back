using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text.Json;
using System.Threading.Tasks;
using DataDll.DbManager;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace RedWheels.Controllers
{
    //Extra properties on Vehicles that are used for GET methods
    public class VehicleEx : Vehicles
    {
        public string BranchLocation { get; set; }

        //public string image { get; set; }
        //public string imageDir { get; set; }
    }

    /*This controller supports:
    * /Vehicles/GetAllVehicles - Receiving all vehicles existing
    * /Vehicles/GetVehiclesByModel/{modelId} - Receiving all vehicles with {modelId}
    * /Vehicles/GetVehicle/{vehicleNumber} - Receiving a specific vehicle by {vehicleNumber}
    * /Vehicles/AddNewVehicle - Adds a new vehicle to the [Vehicles] table
    * /Vehicles/EditVehicle - Edits changeable fields from objects with existing {vehicleNumber}
    * /Vehicles/DeleteVehicle/{vehicleNumber} - Deleting a specific vehicle by {vehicleNumber}
    */
    [EnableCors("projCors")]
    [Route("[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        BranchManager bManager = new BranchManager();
        VehicleManager manager = new VehicleManager();

        //Extra class implementation
        private List<VehicleEx> Extended(List<Vehicles> vehicleList)
        {
            var extVehicle = new List<VehicleEx>();
            foreach (var i in vehicleList)
            {
                var extendedVehicle = new VehicleEx
                {
                    VehicleNumber = i.VehicleNumber,
                    ModelId = i.ModelId,
                    CurrentKilos = i.CurrentKilos,
                    IsFunctional = i.IsFunctional,
                    IsAvailable = i.IsAvailable,
                    BranchId = i.BranchId,
                    VehiclePicture = i.VehiclePicture
                };
                if (extendedVehicle.BranchId > 0)
                    extendedVehicle.BranchLocation =
                        bManager.GetBranch(extendedVehicle.BranchId).Location;
                /*
                if (i.VehiclePicture != null)
                {
                    extendedVehicle.imageDir = Directory.GetCurrentDirectory() + 
                        "\\vehicleImages\\" + i.VehiclePicture;
                    byte[] imageArray = System.IO.File.ReadAllBytes(extendedVehicle.imageDir);
                    string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                    extendedVehicle.image = base64ImageRepresentation;
                    //JsonSerializer.Serialize(xt.imageStream = new StreamReader(xt.imageDir));
                }
                */
                extVehicle.Add(extendedVehicle);
            }
            return extVehicle;
        }
        // GET: /Vehicles/GetAllVehicles
        // Receiving all vehicles existing
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllVehicles()
        {
            var allVehicles = manager.GetAllVehicles();
            return Ok(Extended(allVehicles));
        }
        // GET: /Vehicles/GetVehiclesByModel/{modelId}
        // Receiving all vehicles existing by of that model by {modelId}
        [HttpGet("{modelId}")]
        [Route("[action]/{modelId}")]
        public IActionResult GetVehiclesByModel(int modelId)
        {
            List<Vehicles> vList = manager.GetVehiclesByModel(modelId);
            if (vList != null)
                return Ok(Extended(vList));
            return NotFound();
        }
        // GET: /Vehicles/GetVehicle/{vehicleNumber}
        // Receiving a specific vehicle by {vehicleNumber} if one is found, if not returns NotFound
        [HttpGet("{vehicleNumber}")]
        [Route("[action]/{vehicleNumber}")]
        public IActionResult GetVehicle(int vehicleNumber)
        {
            Vehicles vehicle = manager.GetVehicle(vehicleNumber);
            if (vehicle != null)
            {
                List<Vehicles> list = new List<Vehicles>();
                list.Add(vehicle);
                return Ok(Extended(list)[0]);
            }
            return Ok(null);
        }
        // POST: /Vehicles/AddNewVehicle
        //{"vehicleNumber":12345678, "modelId":4, "currentKilos":1500, "isFunctional":true, "isAvailable":true, "branchId":3}
        // Adds a new vehicle to the [Vehicles] table if the statement is valid, check ↑ for example
        [HttpPost]
        [Route("[action]")]
        public IActionResult AddNewVehicle([FromBody] Vehicles vehicle)
        {
            if (ModelState.IsValid)
                if (manager.AddNewVehicle(vehicle) != null)
                    return Created("Vehicles/" + vehicle.VehicleNumber, vehicle);
            return BadRequest(ModelState);
        }

        // PUT: /Vehicles/EditVehicle
        // Edits vehicle's changeable fields by vehicleNumber if one is found, check ↓ for example
        //{"vehicleNumber":12345678, "modelId":4, "currentKilos":1500, "isFunctional":true, "isAvailable":true, "branchId":3}
        [HttpPut]
        [Route("[action]")]
        public IActionResult EditVehicle(Vehicles vehicle)
        {
            var editedVehicle = manager.EditVehicle(vehicle);
            if (ModelState.IsValid)
                if (editedVehicle != null)
                    return Ok(editedVehicle);
                else
                    return NotFound();
            return BadRequest(ModelState);
        }
        // DELETE: /Vehicles/DeleteVehicle/{vehicleNumber}
        // Deleting a specific vehicle by {vehicleNumber} if one is found, if not returns NotFound
        [HttpDelete("{vehicleNumber}")]
        [Route("[action]/{vehicleNumber}")]
        public IActionResult DeleteVehicle(int vehicleNumber)
        {
            if (manager.DeleteVehicle(vehicleNumber) != null)
                return Ok($"Vehicle number {vehicleNumber} deleted");
            return NotFound();
        }
    }
}

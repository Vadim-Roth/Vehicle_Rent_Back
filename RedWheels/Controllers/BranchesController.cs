using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataDll.DbManager;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/*This controller supports:
    
 * /Branches/GetAllBranches - Receiving all branches existing
 * /Branches/GetBranch/{branchId} - Receiving a specific branch by {branchId}
 * /Branches/GetBranchByLocation/{branchLocation} - Receiving a specific branch by {branchLocation}
 * /Branches/AddNewBranch - Adds a new branch to the [Branches] table
 * /Branches/EditBranch- Edits an existing branch in the [Branches] table
 * /Branches/DeleteBranch/{branchId} - Deleting a specific branch by {branchId}
    
*/
namespace RedWheels.Controllers
{
    [EnableCors("projCors")]
    [Route("[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        BranchManager manager = new BranchManager();
        // GET: /Branches/GetAllBranches
        // Receiving all branches existing
        [HttpGet][Route("[action]")]
        public IActionResult GetAllBranches()
        {
            return Ok(manager.GetAllBranches());
        }
        // GET: /Branches/GetBranch/{branchId}
        // Receiving a specific branch by {branchId} if one is found, if not returns NotFound
        [HttpGet("{branchId}")][Route("[action]/{branchId}")]
        public IActionResult GetBranch(int branchId)
        {
            Branches branch = manager.GetBranch(branchId);
            if (branch != null)
                return Ok(branch);
            return NotFound();
        }
        // GET: /Branches/GetBranchByLocation/{branchLocation}
        // Receiving a specific branch name by {branchId} if one is found, if not returns NotFound
        [HttpGet("{branchLocation}")]
        [Route("[action]/{branchLocation}")]
        public IActionResult GetBranchByLocation(string branchLocation)
        {
            Branches branch = manager.GetBranchByLocation(branchLocation);
            if (branch != null)
                return Ok(branch);
            return NotFound();
        }
        // POST: /Branches/AddNewBranch
        // {"location":"asdasd", "exactLocation":"asd:asd", "branchName":"asdLand"}
        // Adds a new branch to the [Brnaches] table if the statement is valid, check ↑ for example
        [HttpPost]
        [Route("[action]")]
        public IActionResult AddNewBranch([FromBody] Branches branch)
        {
            if (ModelState.IsValid)
                if (manager.AddNewBranch(branch) != null)
                    return Created("Branches/" + branch.BranchId, branch);
            return BadRequest(ModelState);
        }
        // PUT: /Branches/EditBranch
        // Edits some user's fields if one is found, check ↓ for example
        // { "branchId": 4, location": "Herzl 50, Rishon Le-Zion", 
        //"exactLocation": "51.962290/7.602260", "branchName": "DsRed" }
        [HttpPut][Route("[action]")]
        public IActionResult EditBranch(Branches branch)
        {
            var editedBranch = manager.EditBranch(branch);
            if (ModelState.IsValid)
                if (editedBranch != null)
                    return Ok(editedBranch);
                else
                    return NotFound();
            return BadRequest(ModelState);
        }
        // DELETE: Branches/DeleteBranch/{branchId}
        // Deleting a specific branch by {branchId} if one is found, if not returns NotFound
        [HttpDelete("{branchId}")][Route("[action]/{branchId}")]
        public IActionResult DeleteBranch(int branchId)
        {
            if (manager.DeleteBranch(branchId) != null)
                return Ok($"Branch {branchId} deleted");
            return NotFound();
        }
    }
}

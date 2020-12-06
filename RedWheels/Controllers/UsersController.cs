using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataDll.DbManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using review.Models;

/*This controller supports:
    
 * /Users/GetAllUsers - Receiving all users existing
 * /Users/GetUser/{userId} - Receiving a specific user by {userId}
 * /Users/GetUserByNick/{userNick} - Receiving a specific user by {userNick}
 * /Users/IsUserUnique/{userNick} - Recieving Available/Exists on {userNick}
 * /Users/Login/{logInfo} - Expecting {"useNick^password"} and returning the user if true
 * /Users/AddNewUser - Adds a new user to the [Users] table
 * /Users/EditUser - Edits an existing user in the [Users] table
 * /Users/DeleteUser/{userId} - Deleting a specific user by {userId}
*/
namespace RedWheels.Controllers
{
    [EnableCors("projCors")]
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        UserManager manager = new UserManager();

        // GET: /Users/GetAllUsers
        // Receiving all users existing
        [HttpGet][Route("[action]")]
        //[Authorize]
        public IActionResult GetAllUsers()
        {
            return Ok(manager.GetAllUsers());
        }
        // GET: /Users/IsUserUnique/{userNick}
        // Checks the existance of a user by {userNick} in [Users] table
        [HttpGet("{userNick}")][Route("[action]/{userNick}")]
        public IActionResult IsUserUnique(string userNick)
        {
            if (manager.IsUserUnique(userNick))
                return Ok("Available");
            return Ok("Exists");
        }
        // GET: /Users/GetUserByNick/{userNick}
        // Checks the existance of a user by {userNick} in [Users] table
        [HttpGet("{userNick}")][Route("[action]/{userNick}")]
        public IActionResult GetUserByNick(string userNick)
        {
            Users user = manager.GetUserByNick(userNick);
            if (user != null)
                return Ok(user);
            return Ok("Doesn't exist");
        }
        // GET: /Users/GetUser/{userId}
        // Receiving a specific user by {userId} if one is found, if not returns NotFound
        [HttpGet("{userId}")][Route("[action]/{userId}")]
        public IActionResult GetUser(int userId)
        {
            Users user = manager.GetUser(userId);
            if (user != null)
                return Ok(user);
            return Ok(null);
        }
        // GET: /Users/Login/{logInfo}
        // Receiving a specific user by {userId} if one is found, if not returns NotFound
        [HttpGet("{logInfo}")]
        [Route("[action]/{logInfo}")]
        public IActionResult Login(string logInfo)
        {
            string[] splitInfo = logInfo.Split("^",2);
            Users user = manager.Login(splitInfo[0], splitInfo[1]);
            if (user == null)
                return Ok(null);
            return Ok(user);
        }
        // POST: /Users/AddNewUser
        //{"userName":"Mima", "userNick":"Mamoo", "userGender":"Female", "userEmail":"asd@asd.asd", "userPass":"passsss", "userRole":"customer"}
        // Adds a new user to the [Users] table if the statement is valid, check ↑ for example
        [HttpPost][Route("[action]")]
        public IActionResult AddNewUser([FromBody] Users user)
        {
            if (ModelState.IsValid)
                if (manager.AddNewUser(user) != null)
                    return Created("Users/" + user.UserId, user);
            return BadRequest(ModelState);
        }
        // POST: /Users/UploadImage
        [HttpPost][Route("[action]")]
        public IActionResult UploadImage([FromBody] IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return Ok(target.ToArray());
            }
        }
        // PUT: /Users/EditUser
        // Edits some user's fields if one is found, check ↓ for example
        // {"userId": 3,"userName": "crhissy","userGender": "Male","userEmail": "ler@asd.com",
        // "userPass": "zxczxczxcx", "userPicture": "ChrisW.jpg"}
        [HttpPut][Route("[action]")]
        public IActionResult EditUser(Users user)
        {
            var editedUser = manager.EditUser(user);
            if (ModelState.IsValid)
                if (editedUser != null)
                    return Ok(editedUser);
                else
                    return NotFound();
            return BadRequest(ModelState);
        }

        // DELETE: /Users/DeleteUser/{userId}
        // Deleting a specific user by {userId} if one is found, if not returns NotFound
        [HttpDelete("{userId}")][Route("[action]/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            if (manager.DeleteUser(userId) != null)
                return Ok($"User {userId} deleted");
            return NotFound();
        }
    }
}

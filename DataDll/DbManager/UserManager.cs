using RedWheels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDll.DbManager
{
    //This is the manager for the UsersController
    public class UserManager
    {
        dbContext db = new dbContext();
        //Returns all users available
        public List<Users> GetAllUsers()
        {
            return db.Users.ToList();
        }
        //Checks whether a user {userNick} exists and return true or false
        public bool IsUserUnique(string userNick)
        {
            if (GetUserByNick(userNick) != null)
                return true;
            return false;
        }
        //Checks whether a user {userNick} exists and return him if so
        public Users GetUserByNick(string userNick)
        {
            Users user = db.Users.FirstOrDefault(u => u.UserNick == userNick);
            if(user != null)
                return user;
            return null;
        }
        //Returns a single user by {userId}
        public Users GetUser(int userId)
        {
            return db.Users.FirstOrDefault(u => u.UserId == userId);
        }
        //Logs in via userName and password
        public Users Login(string userName, string password)
        {
            Users logging = db.Users.FirstOrDefault(u => u.UserNick == userName);
            if (logging != null && logging.UserPass == password)
                return logging;
            return null;
        }
        //
        public Users GetUserPicture(string pictureName)
        {
            return db.Users.FirstOrDefault(u => u.UserPicture == pictureName);
        }
        //Adds a new user to the [Users] table
        public Users AddNewUser(Users newUser)
        {
            if (db.Users.FirstOrDefault
                (u => u.UserId == newUser.UserId) == null)
            {
                db.Users.Add(newUser);
                db.SaveChanges();
                return newUser;
            }
            return null;
        }
        //Edits an existing user by userId in the [Users] table
        public Users EditUser(Users user)
        {
            Users editedUser = db.Users.FirstOrDefault
                (u => u.UserId == user.UserId);
            if (editedUser != null)
            {
                editedUser.UserName = user.UserName;
                editedUser.UserNick = user.UserNick;
                editedUser.UserEmail = user.UserEmail;
                editedUser.UserGender = user.UserGender;
                editedUser.UserPass = user.UserPass;
                editedUser.UserPicture = user.UserPicture;
                editedUser.UserRole = user.UserRole;
                db.SaveChanges();
                return editedUser;
            }
            else
                throw new Exception("User not found");
        }
        //Deletes a single user by {userId} if one is found
        //Also removes the user from Order list 
        public Users DeleteUser(int userId)
        {
            Users user = db.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                db.Users.Remove(user);
                foreach (OrderList order in db.OrderList) {
                    if (order.CustId == user.UserId)
                    {
                        order.CustId = 0;
                        order.EmpRegister = -1;
                    }
                }
                db.SaveChanges();
            }
            return user;
        }

    }
}

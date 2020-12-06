using RedWheels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDll.DbManager
{
    //This is the manager for the BranchesController
    public class BranchManager
    {
        dbContext db = new dbContext();
        //Returns all branches available
        public List<Branches> GetAllBranches()
        {
            return db.Branches.ToList();
        }
        //Returns a single branch by {branchId}
        public Branches GetBranch(int branchId)
        {
            return db.Branches.FirstOrDefault(b => b.BranchId == branchId);
        }
        //Returns a single branch by {branchLocation}
        public Branches GetBranchByLocation(string branchLocation)
        {
            Branches branch = db.Branches.FirstOrDefault(b => b.Location == branchLocation);
            return branch;
        }
        //Adds a new branch to the [Branches] table
        public Branches AddNewBranch(Branches newBranch)
        {
            db.Branches.Add(newBranch);
            db.SaveChanges();
            return newBranch;
        }
        //Edits an existing branch's changeable fields in the [Branches] table
        public Branches EditBranch(Branches branch)
        {
            Branches editedBranch = db.Branches.FirstOrDefault
                (b => b.BranchId == branch.BranchId);
            if (editedBranch != null)
            {
                editedBranch.BranchId = branch.BranchId;
                editedBranch.Location = branch.Location;
                editedBranch.ExactLocation = branch.ExactLocation;
                editedBranch.BranchName = branch.BranchName;
                db.SaveChanges();
                return editedBranch;
            }
            else
                throw new Exception("Branch not found");
        }
        //Deletes a single branch by {branchId} if one is found
        public Branches DeleteBranch(int branchId)
        {
            Branches branch = db.Branches.FirstOrDefault(b => b.BranchId == branchId);
            if (branch != null)
            {
                db.Branches.Remove(branch);
                foreach (Vehicles vehicle in db.Vehicles)
                {
                    if (vehicle.BranchId == branch.BranchId) {
                        vehicle.BranchId = 0;
                        break;
                    }
                }
                db.SaveChanges();
            }
            return branch;
        }
    }
}

using RedWheels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDll.DbManager
{
    //This is the manager for the OrdersController
    public class OrderManager
    {
        VehicleManager vManager = new VehicleManager();
        dbContext db = new dbContext();
        //Returns all orders available
        public List<OrderList> GetAllOrders()
        {
            return db.OrderList.ToList();
        }
        //Returns a single order by {orderId}
        public OrderList GetOrder(int orderId)
        {
            return db.OrderList.FirstOrDefault(o => o.OrderId == orderId);
        }
        //Returns all orders by {orderId}
        public List<OrderList> GetAllUserOrders(int userId)
        {
            Users user = db.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                List<OrderList> userOrders = new List<OrderList>();
                foreach(OrderList order in GetAllOrders())
                {
                    if (order.CustId == user.UserId)
                        userOrders.Add(order);
                }
                userOrders.Reverse();
                return userOrders;
            }
            return null;
        }
        //Returns the last order by {orderId}
        public OrderList GetLastOrder(int custId)
        {
            Users user = db.Users.FirstOrDefault(u => u.UserId == custId);
            if(user != null)
            {
                OrderList order = db.OrderList.LastOrDefault(o => o.CustId == custId);
                if(order != null)
                    return order;
            }
            return null;
        }
        //Adds a new open order to the [OrderList] table
        public OrderList StartNewOrder(OrderList order)
        {
            if (db.OrderList.FirstOrDefault
                (o => o.OrderId == order.OrderId) == null)
            {
                db.Vehicles.Find(order.VehicleNumber).IsAvailable = false;
                db.OrderList.Add(order);
                db.SaveChanges();
                return order;
            }
            return null;
        }
        //Ends an existing order and adds values in the orderRealEnd and empRegister fields
        //in the [OrderList] table
        public OrderList EndExistingOrder(OrderList order)
        {
            OrderList finishedOrder = db.OrderList.FirstOrDefault
                (o => o.OrderId == order.OrderId);
            if (finishedOrder != null)
            {
                finishedOrder.EmpRegister = order.EmpRegister;
                finishedOrder.OrderRealEnd = order.OrderRealEnd;
                db.Vehicles.Find(finishedOrder.VehicleNumber).IsAvailable = true;
                db.SaveChanges();
                return finishedOrder;
            }
            else
                throw new Exception("Order not found");
        }
        //Edits an existing order in the [OrderList] table
        public OrderList EditOrder(OrderList order)
        {
            OrderList editedOrder = db.OrderList.FirstOrDefault
                (o => o.OrderId == order.OrderId);
            if (editedOrder != null)
            {
                editedOrder.OrderStart = order.OrderStart;
                editedOrder.OrderEnd = order.OrderEnd;
                editedOrder.OrderRealEnd = order.OrderRealEnd;
                editedOrder.CustId = order.CustId;
                editedOrder.VehicleNumber = order.VehicleNumber;
                editedOrder.EmpRegister = order.EmpRegister;
                db.SaveChanges();
                return editedOrder;
            }
            else
                throw new Exception("Order not found");
        }
        //Deletes a single order by {orderId} if one is found
        public OrderList DeleteOrder(int orderId)
        {
            OrderList order = db.OrderList.FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                db.OrderList.Remove(order);
                if(order.VehicleNumber > 0 && order.EmpRegister <= 0)
                {
                    foreach (Vehicles vehicle in db.Vehicles)
                    {
                        if (vehicle.VehicleNumber == order.VehicleNumber)
                        {
                            vehicle.IsAvailable = true;
                            break;
                        }
                    }
                }
                db.SaveChanges();
            }
            return order;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataDll.DbManager;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/*This controller supports:
    
 * /Orders/GetAllOrders - Receiving all orders existing
 * /Orders/GetOrder/{orderId} - Receiving a specific order by {orderId}
 * /Orders/StartNewOrder - Adds a new order to the [OrderList] table
 * /Orders/EndExistingOrder - Ends existing order and fills empRegister and OrderRealEnd in [OrderList]
 * /Orders/DeleteOrder/{orderId} - Deleting a specific order by {orderId}
*/
namespace RedWheels.Controllers
{
    public class OrderEx : OrderList
    {
        public VehicleTypes VehicleType { get; set; }
        public Users Customer { get; set; }
        public Users Employee { get; set; }
    }

    [EnableCors("projCors")]
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private List<OrderEx> Extended(List<OrderList> orderList)
        {
            var extOrder = new List<OrderEx>();
            foreach (var i in orderList)
            {
                var extendedOrder = new OrderEx
                {
                    OrderId = i.OrderId,
                    OrderStart = i.OrderStart,
                    OrderEnd = i.OrderEnd,
                    OrderRealEnd = i.OrderRealEnd,
                    CustId = i.CustId,
                    VehicleNumber = i.VehicleNumber,
                    EmpRegister = i.EmpRegister
                };
                extendedOrder.Customer = uManager.GetUser(extendedOrder.CustId);
                extendedOrder.Employee = uManager.GetUser(extendedOrder.EmpRegister);
                if (extendedOrder.VehicleNumber > 0)
                {
                    var model = vManager.GetVehicle(extendedOrder.VehicleNumber).ModelId;
                    extendedOrder.VehicleType = vtManager.GetVehicleType(model);
                }
                extOrder.Add(extendedOrder);
            }
            return extOrder;
        }

        VehicleTypeManager vtManager = new VehicleTypeManager();
        VehicleManager vManager = new VehicleManager();
        UserManager uManager = new UserManager();
        OrderManager manager = new OrderManager();
        // GET: /Orders/GetAllOrders
        // Receiving all orders existing
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllOrders()
        {
            List<OrderList> list = manager.GetAllOrders();
            return Ok(Extended(list));
        }
        // GET: /Orders/GetOrder/{orderId}
        // Receiving a specific order by {orderId} if one is found, if not returns NotFound
        [HttpGet("{orderId}")]
        [Route("[action]/{orderId}")]
        public IActionResult GetOrder(int orderId)
        {
            OrderList order = manager.GetOrder(orderId);
            if (order != null)
            {
                List<OrderList> list = new List<OrderList>();
                list.Add(order);
                return Ok(Extended(list)[0]);
            }
            return NotFound();
        }
        // GET: /Orders/GetAllUserOrders/{custId}
        // Receiving the last order by {custId} if one is found, if not returns NotFound
        [HttpGet("{userId}")]
        [Route("[action]/{userId}")]
        public IActionResult GetAllUserOrders(int userId)
        {
            List<OrderList> list = manager.GetAllUserOrders(userId);
            return Ok(Extended(list));
        }
        // GET: /Orders/GetLastOrder/{custId}
        // Receiving the last order by {custId} if one is found, if not returns NotFound
        [HttpGet("{custId}")]
        [Route("[action]/{custId}")]
        public IActionResult GetLastOrder(int custId)
        {
            return Ok(manager.GetAllUserOrders(custId).FirstOrDefault());
        }
        // POST: /Orders/StartNewOrder
        //{"orderStart":"11/8/2020", "orderEnd":"16/9/2020", "custId":3, "vehicleNumber":11555448}
        // Adds a new branch to the [Brnaches] table if the statement is valid, check ↑ for example
        [HttpPost]
        [Route("[action]")]
        public IActionResult StartNewOrder([FromBody] OrderList order)
        {
            if (ModelState.IsValid)
                if (manager.StartNewOrder(order) != null)
                    return Created("Orders/" + order.OrderId, order);
            return BadRequest(ModelState);
        }

        // PUT: /Orders/EndExistingOrder
        // Ends existing order and adds empRegister and OrderRealEnd by orderId if one is found
        // { "orderId": 3, "OrderRealEnd": "28/8/1010", "empRegister": 10 }
        [HttpPut]
        [Route("[action]")]
        public IActionResult EndExistingOrder(OrderList order)
        {
            var finishedOrder = manager.EndExistingOrder(order);
            if (ModelState.IsValid)
                if (finishedOrder != null)
                    return Ok(finishedOrder);
                else
                    return NotFound();
            return BadRequest(ModelState);
        }

        // PUT: /Orders/EditOrder
        // Ends existing order and adds empRegister and OrderRealEnd by orderId if one is found
        // { "orderId": 3, "OrderRealEnd": "28/8/1010", "empRegister": 10 }
        [HttpPut]
        [Route("[action]")]
        public IActionResult EditOrder(OrderList order)
        {
            var editedOrder = manager.EditOrder(order);
            if (ModelState.IsValid)
                if (editedOrder != null)
                    return Ok(editedOrder);
                else
                    return NotFound();
            return BadRequest(ModelState);
        }

        // DELETE: /Orders/DeleteOrder/{orderId}
        // Deleting a specific Order by {orderId} if one is found, if not returns NotFound
        [HttpDelete("{orderId}")]
        [Route("[action]/{orderId}")]
        public IActionResult DeleteOrder(int orderId)
        {
            if (manager.DeleteOrder(orderId) != null)
                return Ok($"Order {orderId} deleted");
            return NotFound();
        }
    }
}

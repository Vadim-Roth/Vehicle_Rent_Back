using System;
using System.Collections.Generic;

namespace RedWheels
{
    public partial class OrderList
    {
        public int OrderId { get; set; }
        public string OrderStart { get; set; }
        public string OrderEnd { get; set; }
        public string OrderRealEnd { get; set; }
        public int CustId { get; set; }
        public int VehicleNumber { get; set; }
        public int EmpRegister { get; set; }
    }
}

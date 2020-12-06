using System;
using System.Collections.Generic;

namespace RedWheels
{
    public partial class VehicleTypes
    {
        public int ModelId { get; set; }
        public string Manufacturer { get; set; }
        public string ModelName { get; set; }
        public double DailyCost { get; set; }
        public double DailyDelay { get; set; }
        public int ProdYear { get; set; }
        public string Gear { get; set; }
    }
}

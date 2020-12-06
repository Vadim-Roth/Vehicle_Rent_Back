using System;
using System.Collections.Generic;

namespace RedWheels
{
    public partial class Vehicles
    {
        public int VehicleNumber { get; set; }
        public int ModelId { get; set; }
        public int CurrentKilos { get; set; }
        public string VehiclePicture { get; set; }
        public bool IsFunctional { get; set; }
        public bool IsAvailable { get; set; }
        public int BranchId { get; set; }
    }
}

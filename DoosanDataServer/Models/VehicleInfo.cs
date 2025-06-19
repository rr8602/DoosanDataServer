using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoosanDataServer.Models
{
    public class VehicleInfo
    {
        public string ChassisNumber { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }

        public VehicleInfo(string chassisNumber, string model, string manufacturer)
        {
            ChassisNumber = chassisNumber;
            Model = model;
            Manufacturer = manufacturer;
        }
    }
}

using System;

namespace Incline.Models
{
    public class InclineMeasurement
    {
        public string AcceptNo { get; set; }
        public string VinNo { get; set; }
        public string Model { get; set; }
        public double InclineAngle { get; set; }
        public bool InspectionStatus { get; set; }
        public bool OkNg { get; set; }
        public DateTime MeaDate { get; set; }

        public InclineMeasurement()
        {
            MeaDate = DateTime.Now;
            InspectionStatus = false;
            OkNg = false;
            AcceptNo = string.Empty;
            VinNo = string.Empty;
            Model = string.Empty;
        }
    }
}

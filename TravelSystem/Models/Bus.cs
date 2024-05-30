using System.ComponentModel.DataAnnotations;

namespace TravelSystem.Models
{
    public class Bus
    {
        public Bus()
        {
            Trips = new List<Trip>();
        }
        [Key]
        public string Bus_Plate { get; set; } = string.Empty;
        public List<Trip> Trips { get; set; }
    }
}

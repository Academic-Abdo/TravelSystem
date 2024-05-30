using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelSystem.Models
{
    public class Trip
    {
        public int Id { get; set; }
        [StringLength(450)]
        public string BusId { get; set; }
        public int DriverId { get; set; }
        public int TripKindId { get; set; }
        public int AvailableSeats { get; set; }
        public int ReservedSeats { get; set; }
        public DateTime TripDate { get; set; }
        public Driver? Driver { get; set; }
        [ForeignKey(nameof(BusId))]
        public Bus? Bus { get; set; }
        public TripKind? TripKind { get; set; }
    }
}

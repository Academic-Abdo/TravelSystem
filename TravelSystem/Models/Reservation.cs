namespace TravelSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int ClintId { get; set; }
        public int TripId { get; set; }
        public Clint? Clint { get; set; }
        public Trip? Trip { get; set; }
        public DateTime ReservationDate { get; set; }
    }
}

namespace TravelSystem.Models
{
    public class TripKind
    {
        public TripKind()
        {
            Trips = new List<Trip>();
        }
        public int TripKindId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Trip> Trips { get; set; }
    }
}

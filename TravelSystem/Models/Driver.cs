namespace TravelSystem.Models
{
    public class Driver
    {
        public Driver()
        {
            Trips = new List<Trip>();
        }
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public List<Trip> Trips { get; set; }
    }
}

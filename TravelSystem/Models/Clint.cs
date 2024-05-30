using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelSystem.Models
{
    public class Clint
    {
        public Clint()
        {
            Reservations = new List<Reservation>();
        }
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string DocumntPath { get; set; } = string.Empty ;
        [NotMapped]
        public IFormFile? Documnt { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}

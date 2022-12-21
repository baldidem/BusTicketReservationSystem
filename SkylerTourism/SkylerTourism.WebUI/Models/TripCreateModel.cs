using SkylerTourism.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkylerTourism.WebUI.Models
{
    public class TripCreateModel
    {
        public int TripId { get; set; }
        [Required]
        public int DepartureCityId { get; set; }
        public City DepartureCity { get; set; }
        [Required]
        public int ArrivalCityId { get; set; }
        public City ArrivalCity { get; set; }
        [Required]
        public DateTime TripDate { get; set; }
        [Required]
        public DateTime TripTime { get; set; }
        [Required]
        public decimal Price { get; set; }
        public List<Bus> Buses { get; set; }
        [Required]
        public int BusId { get; set; }
        public List<City> Cities { get; set; } = null!;

    }
}

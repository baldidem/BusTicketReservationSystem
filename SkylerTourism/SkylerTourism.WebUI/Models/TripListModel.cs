using SkylerTourism.Entity;
using System.ComponentModel.DataAnnotations;

namespace SkylerTourism.WebUI.Models
{
    public class TripListModel
    {
        public int TripId { get; set; }
        public DateTime TripDate { get; set; }
        public DateTime TripTime { get; set; }
        public decimal Price { get; set; }
        public int DepartureCityId { get; set; }
        public City DepartureCity { get; set; }
        public int ArrivalCityId { get; set; }
        public City ArrivalCity { get; set; }


    }
}

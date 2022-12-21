using SkylerTourism.Entity;
using System.ComponentModel.DataAnnotations;

namespace SkylerTourism.WebUI.Models
{
    public class SearchTripModel
    {
        public int DepartureCityId { get; set; }
        public int ArrivalCityId { get; set; }
        public DateTime TripDate { get; set; }
        public List<City> Cities { get; set; } = null!;

    }
}
